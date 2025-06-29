using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Enums;
using Autotech.Desktop.Core.Models;
using MetroSet_UI.Controls;
using MetroSet_UI.Forms;
using System.ComponentModel;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Autotech.Desktop.Main.View
{
    public partial class MainForm : MetroSetForm
    {
        #region Initialize
        public MainForm()
        {
            InitializeComponent();
            if (LoginHelper.isLoggedIn == true)
            {
                this.Enabled = true;
                if (SessionManager.AgentDetails.AgentRole != "Admin")
                {
                    metroSetTabControl1.TabPages.Remove(tabPageMaintenance);
                }
                InitializeTimer();
                GetUser();
                SetLocation();
                InitializeDataGridView();
                LoadItemsIntoGrid();
                InitializeOrderCartGrid();
                dataGridViewItemList.CellFormatting += DataGridViewItemList_CellFormatting;
                txtPaidAmount.TextChanged += txtPaidAmount_TextChanged;
                comboAccount.SelectedIndexChanged += comboAccount_SelectedIndexChanged;
                InitializePaymentMethods();
                this.Load += MainForm_Load;
                PopulateFilterCombo();
                
            }
        }
        #endregion

        #region Variables
        private System.Windows.Forms.Timer dateTimer;
        private List<Items> allItems = new List<Items>();
        private int currentPage = 1;
        private int pageSize = 20;
        private List<Items> currentPageItems = new();
        private HashSet<Guid> selectedItemIds = new();
        private List<Items> orderCartItems = new();
        private List<SalesDTO> allInvoices = new();
        #endregion

        #region Props
        public string AgentName
        {
            get { return lblAgentName.Text; }
            set { lblAgentName.Text = value; }
        }
        public string DateandSales
        {
            get { return lblSalesInfo.Text; }
            set { lblSalesInfo.Text = value; }
        }
        public int SalesNumber { get; set; }

        #endregion

        #region Events
        private void lblSearchItem_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SessionManager.ClearSession();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
        #endregion

        #region Methods

        private void GetUser()
        {
            if (SessionManager.AgentDetails != null)
            {
                Thread.Sleep(1000);
                AgentName = SessionManager.AgentDetails.AgentName;
            }
            else
            {
                //for future logic
            }
        }

        private void InitializeTimer()
        {
            dateTimer = new System.Windows.Forms.Timer();
            dateTimer.Interval = 1000;
            dateTimer.Tick += new EventHandler(UpdateDateAndSalesTick);
            dateTimer.Start();
        }

        private void UpdateDateAndSalesTick(object sender, EventArgs e)
        {
            DateandSales = $"{DateTime.Now:dddd, dd MMMM yyyy HH:mm:ss}\nSales Number: {SalesNumber}";
        }

        private void SetLocation()
        {
            try
            {
                if (SessionManager.AgentDetails != null)
                {
                    var location = SessionManager.AgentDetails.Location.LocationName;
                    if (location == "Bataan")
                    {
                        radioBataan.Checked = true;
                    }
                    else if (location == "Zambales")
                    {
                        radioZambales.Checked = true;
                    }
                    else if (location == "Upper Pampanga" || location == "Lower Pampanga")
                    {
                        radioPampanga.Checked = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Paging
        private async void LoadItemsIntoGrid(int page = 1)
        {
            try
            {
                var itemService = new ItemServices();
                var pagedResult = await itemService.GetPaginatedItemsAsync(page, pageSize);

                currentPageItems = pagedResult;

                // Merge paged items into allItems (avoiding duplicates)
                foreach (var item in pagedResult)
                {
                    if (!allItems.Any(i => i.Id == item.Id))
                        allItems.Add(item);
                }

                dataGridViewItemList.DataSource = null;
                dataGridViewItemList.DataSource = currentPageItems;

                foreach (DataGridViewRow row in dataGridViewItemList.Rows)
                {
                    if (row.DataBoundItem is Items item && selectedItemIds.Contains(item.Id))
                    {
                        row.Cells["selectColumn"].Value = true;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading items: {ex.Message}", "Error");
            }
        }
        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadItemsIntoGrid(currentPage);
                lblPage.Text = currentPage.ToString();
                RestoreCheckboxStates();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadItemsIntoGrid(currentPage);
            lblPage.Text = currentPage.ToString();
            RestoreCheckboxStates();
        }
        #endregion

        #region Datagrid
        private void InitializeDataGridView()
        {
            dataGridViewItemList.AutoGenerateColumns = false;
            var checkboxColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "",
                Name = "selectColumn",
                Width = 30,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dataGridViewItemList.Columns.Add(checkboxColumn);

            dataGridViewItemList.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item Name",
                DataPropertyName = "ItemName",
                Name = "itemNameColumn"
            });

            dataGridViewItemList.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity Per Box",
                DataPropertyName = "QuantityPerBox",
                Name = "itemQuantityColumn"
            });

            //qty per box here

            dataGridViewItemList.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Onhand",
                DataPropertyName = "itemDetails.PropertyName",
                Name = "itemDetailsColumn"
            });

            dataGridViewItemList.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item Description",
                DataPropertyName = "ItemDescription",
                Name = "itemDescriptionColumn"
            });

            dataGridViewItemList.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item Code",
                DataPropertyName = "ItemCode",
                Name = "itemCodeColumn"
            });

            dataGridViewItemList.CellValueChanged += dataGridViewItemList_CellValueChanged;

            dataGridViewItemList.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dataGridViewItemList.IsCurrentCellDirty)
                    dataGridViewItemList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            dataGridViewItemList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewItemList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewItemList.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewItemList.DefaultCellStyle.Padding = new Padding(5);
            dataGridViewItemList.AutoResizeColumns();
        }
        private void DataGridViewItemList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var item = dataGridViewItemList.Rows[e.RowIndex].DataBoundItem as Items;

            if (dataGridViewItemList.Columns[e.ColumnIndex].Name == "itemDetailsColumn" && item != null)
            {
                if (item.itemDetails != null)
                {
                    e.Value = item.itemDetails.OnHand;
                }
                else
                {
                    e.Value = "N/A";
                }
            }
        }

        private void dataGridViewItemList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridViewItemList.Columns[e.ColumnIndex].Name == "selectColumn")
            {
                var row = dataGridViewItemList.Rows[e.RowIndex];
                if (row.DataBoundItem is Items item)
                {
                    bool isChecked = Convert.ToBoolean(row.Cells["selectColumn"].Value);

                    if (isChecked)
                        selectedItemIds.Add(item.Id);
                    else
                        selectedItemIds.Remove(item.Id);
                }
            }
        }
        private void RestoreCheckboxStates()
        {
            foreach (DataGridViewRow row in dataGridViewItemList.Rows)
            {
                if (row.DataBoundItem is Items item && selectedItemIds.Contains(item.Id))
                {
                    row.Cells["selectColumn"].Value = true;
                }
            }
        }

        #endregion

        #region SearchItems
        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchItem.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(searchText))
            {
                // Keep checked rows before filtering
                var checkedIds = new List<Guid>();
                foreach (DataGridViewRow row in dataGridViewItemList.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["selectColumn"].Value))
                    {
                        if (row.DataBoundItem is Items item)
                            checkedIds.Add(item.Id);
                    }
                }

                // Filter allItems (not currentPageItems)
                var filtered = allItems.Where(item =>
                    item.ItemCode.ToLower().Contains(searchText) ||
                    item.ItemName.ToLower().Contains(searchText) ||
                    item.ItemDescription.ToLower().Contains(searchText)).ToList();

                dataGridViewItemList.DataSource = filtered;

                // Restore checkboxes
                foreach (DataGridViewRow row in dataGridViewItemList.Rows)
                {
                    if (row.DataBoundItem is Items item && checkedIds.Contains(item.Id))
                    {
                        row.Cells["selectColumn"].Value = true;
                    }
                }
            }
            else
            {
                // Reset to paginated view
                dataGridViewItemList.DataSource = currentPageItems;
            }
        }

        #endregion

        #region AddtoCart
        private void InitializeOrderCartGrid()
        {
            dataGridViewOrderCart.AutoGenerateColumns = false;
            dataGridViewOrderCart.Columns.Clear();
            dataGridViewOrderCart.RowHeadersVisible = false;

            // ✅ Checkbox column
            var checkboxColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "",
                Name = "selectCartItem",
                Width = 30,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dataGridViewOrderCart.Columns.Add(checkboxColumn);

            // ✅ Item Code
            dataGridViewOrderCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item Code",
                DataPropertyName = "ItemCode",
                Name = "cartItemCode",
                ReadOnly = true
            });

            // ✅ Item Name
            dataGridViewOrderCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item Name",
                DataPropertyName = "ItemName",
                Name = "cartItemName",
                ReadOnly = true
            });

            // ✅ Unit Price (readonly)
            dataGridViewOrderCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Unit Price",
                Name = "cartPrice",
                ReadOnly = true
            });

            // ✅ Quantity (editable)
            dataGridViewOrderCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Qty",
                Name = "cartQuantity",
                ReadOnly = false
            });

            // ✅ Discount % (editable)
            dataGridViewOrderCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Discount",
                Name = "cartDiscount",
                ReadOnly = false
            });

            // ✅ Subtotal (readonly)
            dataGridViewOrderCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Subtotal",
                Name = "cartSubtotal",
                ReadOnly = true
            });

            // ✅ Event handlers
            dataGridViewOrderCart.CellFormatting += dataGridViewOrderCart_CellFormatting;

            dataGridViewOrderCart.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dataGridViewOrderCart.IsCurrentCellDirty)
                    dataGridViewOrderCart.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            dataGridViewOrderCart.CellValueChanged += DataGridViewOrderCart_CellValueChanged;
            dataGridViewOrderCart.EditingControlShowing += dataGridViewOrderCart_EditingControlShowing;

            dataGridViewOrderCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewItemList.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["selectColumn"].Value);
                if (isChecked && row.DataBoundItem is Items item)
                {
                    // Avoid duplicates
                    if (!orderCartItems.Any(i => i.Id == item.Id))
                    {
                        orderCartItems.Add(item);

                        // Add row manually
                        int rowIndex = dataGridViewOrderCart.Rows.Add();
                        var cartRow = dataGridViewOrderCart.Rows[rowIndex];

                        double unitPrice = GetPriceBasedOnSelection(item);

                        cartRow.Cells["selectCartItem"].Value = false;
                        cartRow.Cells["cartItemCode"].Value = item.ItemCode;
                        cartRow.Cells["cartItemName"].Value = item.ItemName;
                        cartRow.Cells["cartPrice"].Value = unitPrice;
                        cartRow.Cells["cartQuantity"].Value = 1;
                        cartRow.Cells["cartDiscount"].Value = 0;
                        cartRow.Cells["cartSubtotal"].Value = unitPrice; // default qty = 1, no discount
                    }
                }
            }

            CalculateCartSubtotal();
        }

        private void dataGridViewOrderCart_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewOrderCart.Columns[e.ColumnIndex].Name == "cartPrice" &&
                dataGridViewOrderCart.Rows[e.RowIndex].DataBoundItem is Items item &&
                item.itemDetails != null)
            {
                e.Value = GetPriceBasedOnSelection(item).ToString("C");
                e.FormattingApplied = true;
            }
        }
        private double GetPriceBasedOnSelection(Items item)
        {
            if (item.itemDetails == null)
                return 0;

            bool isRetail = radioRetail.Checked;
            string location = "";

            if (radioBataan.Checked) location = "Bataan";
            else if (radioPampanga.Checked) location = "Pampanga";
            else if (radioZambales.Checked) location = "Zambales";

            return location switch
            {
                "Bataan" => isRetail ? item.itemDetails.BataanRetail : item.itemDetails.BataanWholeSale,
                "Pampanga" => isRetail ? item.itemDetails.PampangaRetail : item.itemDetails.PampangaWholeSale,
                "Zambales" => isRetail ? item.itemDetails.ZambalesRetail : item.itemDetails.ZambalesWholeSale,
                _ => 0
            };
        }
        private void CalculateCartSubtotal()
        {
            decimal subtotal = 0;

            foreach (DataGridViewRow row in dataGridViewOrderCart.Rows)
            {
                if (row.Cells["cartSubtotal"].Value != null &&
                    decimal.TryParse(row.Cells["cartSubtotal"].Value.ToString(), out decimal rowSubtotal))
                {
                    subtotal += rowSubtotal;
                }
            }

            txtSubtotal.Text = subtotal.ToString("C");
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            try
            {
                // Parse values from textboxes
                decimal subtotal = ParseCurrency(txtSubtotal.Text);
                decimal tax = ParseCurrency(txtTax.Text);
                decimal discount = ParseCurrency(txtDiscount.Text);

                decimal total = subtotal + tax - discount;
                txtTotal.Text = total.ToString("C");
                UpdateRemainingBalance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total: " + ex.Message);
            }
        }

        private decimal ParseCurrency(string text)
        {
            if (decimal.TryParse(text, System.Globalization.NumberStyles.Currency, null, out decimal value))
                return value;
            return 0;
        }

        private void DataGridViewOrderCart_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 &&
                (dataGridViewOrderCart.Columns[e.ColumnIndex].Name == "cartQuantity" ||
                 dataGridViewOrderCart.Columns[e.ColumnIndex].Name == "cartDiscount"))
            {
                var row = dataGridViewOrderCart.Rows[e.RowIndex];

                if (decimal.TryParse(row.Cells["cartPrice"].Value?.ToString(), out decimal price) &&
                    int.TryParse(row.Cells["cartQuantity"].Value?.ToString(), out int qty) &&
                    decimal.TryParse(row.Cells["cartDiscount"].Value?.ToString(), out decimal discount))
                {
                    var discountAmount = price * (discount / 100);
                    var subtotal = (price - discountAmount) * qty;
                    row.Cells["cartSubtotal"].Value = subtotal;
                }

                // ✅ This keeps subtotal and total updated in real-time
                CalculateCartSubtotal();
            }
        }

        private void UpdateSubtotalFromCart()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dataGridViewOrderCart.Rows)
            {
                if (decimal.TryParse(row.Cells["cartSubtotal"].Value?.ToString(), out decimal subtotal))
                {
                    total += subtotal;
                }
            }

            txtSubtotal.Text = total.ToString("0.00");
        }

        private void dataGridViewOrderCart_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox textBox)
            {
                textBox.KeyPress -= TextBox_KeyPress_NumericOnly;
                textBox.KeyPress += TextBox_KeyPress_NumericOnly;
            }
        }

        private void TextBox_KeyPress_NumericOnly(object sender, KeyPressEventArgs e)
        {
            // Allow only digits, one dot, and backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;

            // Block multiple dots
            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
                e.Handled = true;
        }

        private void radioWholesale_CheckedChanged(object sender)
        {
            if (radioWholesale.Checked) CalculateCartSubtotal();
        }

        private void radioRetail_CheckedChanged(object sender)
        {
            if (radioRetail.Checked) CalculateCartSubtotal();
        }

        private void radioPampanga_CheckedChanged(object sender)
        {
            if (radioPampanga.Checked) CalculateCartSubtotal();
        }

        private void radioBataan_CheckedChanged(object sender)
        {
            if (radioBataan.Checked) CalculateCartSubtotal();
        }

        private void radioZambales_CheckedChanged(object sender)
        {
            if (radioZambales.Checked) CalculateCartSubtotal();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrderCart.Rows.Count == 0 || orderCartItems.Count == 0)
                return;

            var idsToRemove = new List<Guid>();

            foreach (DataGridViewRow row in dataGridViewOrderCart.Rows)
            {
                var cell = row.Cells["selectCartItem"];
                if (cell != null && cell.Value is bool isChecked && isChecked)
                {
                    if (row.DataBoundItem is Items item)
                    {
                        idsToRemove.Add(item.Id);
                    }
                }
            }

            if (idsToRemove.Count == 0)
            {
                MessageBox.Show("No items selected.");
                return;
            }

            // Actually remove from cart list
            orderCartItems = orderCartItems
                .Where(item => !idsToRemove.Contains(item.Id))
                .ToList();

            // Rebind
            dataGridViewOrderCart.DataSource = null;
            dataGridViewOrderCart.DataSource = orderCartItems;

            CalculateCartSubtotal();
        }

        private void btnEmptyCart_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(
                "Are you sure you want to empty the cart?",
                "Confirm Empty Cart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                // Clear the backing list
                orderCartItems.Clear();

                // Clear the grid binding
                dataGridViewOrderCart.DataSource = null;

                // Reset subtotal
                txtSubtotal.Text = "₱0.00";

                // Optional toast message
                new ToastMessageForm("Cart has been emptied.").Show();
            }
        }

        private void txtTax_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }
        #endregion

        #region Payment
        private void InitializePaymentMethods()
        {
            comboPaymentMethod.DisplayMember = "Value";  // Shown in dropdown
            comboPaymentMethod.ValueMember = "Key";      // Actual enum
            comboPaymentMethod.DataSource = EnumHelper.GetPaymentMethodDescriptions();
            comboPaymentMethod.SelectedIndex = 0;
        }

        private void CalculateChange()
        {
            decimal paidAmount = ParseCurrency(txtPaidAmount.Text);
            decimal totalAmount = ParseCurrency(txtTotal.Text);

            if (paidAmount >= totalAmount)
            {
                decimal change = paidAmount - totalAmount;
                txtChange.Text = change.ToString("C"); // format as currency
            }
            else
            {
                txtChange.Text = "₱0.00";
            }
        }

        private async void btnPay_Click(object sender, EventArgs e)
        {
            // 1. Validate Inputs
            if (comboAccount.SelectedItem == null)
            {
                MessageBox.Show("Please select an account.");
                return;
            }

            if (comboPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment method.");
                return;
            }

            if (orderCartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty.");
                return;
            }

            // ✅ Fix for selected account
            var selectedAccount = comboAccount.SelectedItem as Accounts;
            if (selectedAccount == null)
            {
                MessageBox.Show("Please select a valid account.");
                return;
            }

            var accountId = selectedAccount.Id;
            var accountName = selectedAccount.Name;

            // ✅ Payment method
            var selectedPaymentMethod = (KeyValuePair<PaymentMethod, string>)comboPaymentMethod.SelectedItem;
            var paymentMethod = selectedPaymentMethod.Key.ToString();

            // 2. Parse payment values
            decimal total = ParseCurrency(txtTotal.Text);
            decimal paidAmount = ParseCurrency(txtPaidAmount.Text);
            decimal remaining = ParseCurrency(txtRemaining.Text);
            decimal tax = ParseCurrency(txtTax.Text);
            decimal discount = ParseCurrency(txtDiscount.Text);
            decimal discountPercent = discount / (total + discount - tax) * 100;

            // 3. Build Invoice DTO
            var invoiceDto = new InvoiceDTO
            {
                DateSold = DateTime.Now,
                Agent = SessionManager.AgentDetails.AgentName,
                DiscountPercent = (double)discountPercent,
                DiscountPeso = (double)discount,
                Tax = (double)tax,
                TotalSales = (double)total,
                AccountName = accountName,
                PaymentType = paymentMethod,
                Terms = int.TryParse(txtTerms.Text, out var termsVal) ? termsVal : 0,
                DueDate = DateTime.Now.AddDays(int.TryParse(txtTerms.Text, out var dVal) ? dVal : 0),
                RemainingBalance = Math.Round((double)remaining),
                Status = "For approval",
                TotalLiters = 0,
                Cluster = "",
                AccountId = accountId,
                LocationId = SessionManager.AgentDetails.Location.Id,
                strInvoiceNumber = "",
                PurchasedItems = dataGridViewOrderCart.Rows
                    .Cast<DataGridViewRow>()
                    .Select(row =>
                    {
                        var itemCode = row.Cells["cartItemCode"].Value?.ToString();
                        var item = orderCartItems.FirstOrDefault(i => i.ItemCode == itemCode);
                        if (item == null) return null;

                        double.TryParse(row.Cells["cartQuantity"].Value?.ToString(), out double quantity);
                        double.TryParse(row.Cells["cartPrice"].Value?.ToString(), out double price);
                        double.TryParse(row.Cells["cartSubtotal"].Value?.ToString(), out double subtotal);
                        double.TryParse(row.Cells["cartDiscount"].Value?.ToString(), out double discountPerItem);
                        double totalDiscount = price - subtotal;
                        totalDiscount = Math.Round(totalDiscount, 2);

                        return new InvoiceItemDTO
                        {
                            ItemId = item.Id,
                            Quantity = quantity,
                            ItemPrice = price,
                            TotalPrice = subtotal,
                            ItemName = "",
                            Discount = totalDiscount
                        };
                    })
                    .Where(i => i != null)
                    .ToList()
            };

            // 4. Call backend
            try
            {
                var service = new SalesService();
                var invoiceId = await service.CreateInvoiceAsync(invoiceDto);

                new ToastMessageForm("Invoice created successfully!").Show();

                // ✅ Clear cart and reset
                orderCartItems.Clear();
                dataGridViewOrderCart.DataSource = null;
                dataGridViewOrderCart.Rows.Clear();
                dataGridViewOrderCart.Refresh();

                // ✅ Clear input fields
                txtSubtotal.Text = "";
                txtTax.Text = "";
                txtDiscount.Text = "";
                txtTotal.Text = "";
                txtPaidAmount.Text = "";
                txtChange.Text = "";
                txtRemaining.Text = "";

                SalesNumber++;

                // Optional: Uncheck item list selections if needed
                foreach (DataGridViewRow row in dataGridViewItemList.Rows)
                {
                    if (dataGridViewItemList.Columns.Contains("selectColumn"))
                    {
                        row.Cells["selectColumn"].Value = false;
                    }
                }
                LoadItemsIntoGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create invoice: " + ex.Message);
            }
        }

        private double GetCartValue<T>(Items item, string columnName)
        {
            foreach (DataGridViewRow row in dataGridViewOrderCart.Rows)
            {
                if (row.DataBoundItem is Items currentItem && currentItem.Id == item.Id)
                {
                    return double.TryParse(row.Cells[columnName].Value?.ToString(), out var value) ? value : 0;
                }
            }

            return 0;
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            CalculateChange();
            UpdateRemainingBalance();
        }
        private void UpdateRemainingBalance()
        {
            try
            {
                decimal total = ParseCurrency(txtTotal.Text);
                decimal paidAmount = ParseCurrency(txtPaidAmount.Text);

                decimal remaining = total - paidAmount;
                txtChange.Text = (paidAmount > total) ? (paidAmount - total).ToString("C") : "₱0.00";
                txtRemaining.Text = (remaining > 0 ? remaining : 0).ToString("C");
            }
            catch
            {
                txtRemaining.Text = "₱0.00";
            }
        }
        #endregion

        #region Accounts
        private async void MainForm_Load(object sender, EventArgs e)
        {
            await InitializeAccountsAsync();
        }
        private async Task InitializeAccountsAsync()
        {
            try
            {
                var service = new AccountService();
                var accounts = await service.GetAllAccountsAsync();

                comboAccount.DisplayMember = "Name";
                comboAccount.ValueMember = "Id";
                comboAccount.DataSource = accounts;
                comboAccount.SelectedIndex = -1;

                comboAccount.DropDownStyle = ComboBoxStyle.DropDown;
                comboAccount.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboAccount.AutoCompleteSource = AutoCompleteSource.CustomSource;

                // Enable autocomplete search
                comboAccount.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboAccount.AutoCompleteSource = AutoCompleteSource.CustomSource;

                var autoCompleteSource = new AutoCompleteStringCollection();
                autoCompleteSource.AddRange(accounts.Select(a => a.Name).ToArray());
                comboAccount.AutoCompleteCustomSource = autoCompleteSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load accounts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboAccount.SelectedItem is Accounts selectedAccount)
            {
                txtContactNumber.Text = selectedAccount.ContactNumber ?? "";
                txtTerms.Text = selectedAccount.Terms.ToString(); // Optional: also set terms
                txtContactNumber.ReadOnly = true;
            }
        }
        #endregion

        #region Invoice
        public async Task LoadInvoicesAsync()
        {
            ToastMessageForm loadingToast = null;
            try
            {
                // ✅ Show loading toast
                loadingToast = new ToastMessageForm("Loading invoices...");
                loadingToast.Show();
                loadingToast.TopMost = true;
                loadingToast.BringToFront();
                dataGridViewInvoice.Enabled = false;

                await Task.Run(async () =>
                {
                    await Task.Delay(1000);

                    var salesService = new SalesService();
                    var invoices = await salesService.GetAllInvoicesAsync();
                    allInvoices = invoices;

                    // Switch back to UI thread to update UI controls
                    Invoke(new Action(() =>
                    {
                        
                        dataGridViewInvoice.DataSource = null;
                        dataGridViewInvoice.DataSource = invoices;

                        dataGridViewInvoice.Columns["Id"].Visible = false;

                        foreach (DataGridViewColumn column in dataGridViewInvoice.Columns)
                        {
                            column.Visible = false;
                        }

                        ShowColumn("strInvoiceNumber", "Invoice #");
                        ShowColumn("DateSold", "Date Sold");
                        ShowColumn("Agent", "Agent Name");
                        ShowColumn("AccountName", "Customer Name");
                        ShowColumn("PaymentType", "Payment Method");
                        ShowColumn("TotalSales", "Total Sales");
                        ShowColumn("Tax", "Tax Amount");
                        ShowColumn("DiscountPeso", "Discount (₱)");
                        ShowColumn("Terms", "Terms (Days)");
                        ShowColumn("DueDate", "Due Date");
                        ShowColumn("RemainingBalance", "Balance Remaining");
                        ShowColumn("Status", "Status");
                        ShowColumn("Cluster", "Cluster");

                        dataGridViewInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        dataGridViewInvoice.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        dataGridViewInvoice.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        dataGridViewInvoice.DefaultCellStyle.Padding = new Padding(5);

                        dataGridViewInvoice.RowPrePaint -= dataGridViewInvoice_RowPrePaint;
                        dataGridViewInvoice.RowPrePaint += dataGridViewInvoice_RowPrePaint;
                    }));
                });

                dataGridViewInvoice.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading invoices: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                
                // ✅ Close loading toast
                if (loadingToast != null && !loadingToast.IsDisposed)
                {
                    
                    loadingToast.Close();
                }
            }

            void ShowColumn(string columnName, string header)
            {
                if (dataGridViewInvoice.Columns.Contains(columnName))
                {
                    var column = dataGridViewInvoice.Columns[columnName];
                    column.Visible = true;
                    column.HeaderText = header;
                }
            }

            ApplyInvoiceFilterAndSorting();
        }

        private void PopulateFilterCombo()
        {
            cboFilterInvoice.DataSource = Enum.GetValues(typeof(InvoiceFilterOption))
                .Cast<InvoiceFilterOption>()
                .Select(val => new
                {
                    Key = val,
                    Value = val.GetType()
                        .GetField(val.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .Cast<DescriptionAttribute>()
                        .FirstOrDefault()?.Description ?? val.ToString()
                }).ToList();

            cboFilterInvoice.DisplayMember = "Value";
            cboFilterInvoice.ValueMember = "Key";
        }

        private void dataGridViewInvoice_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var dgv = sender as DataGridView;
            var row = dgv.Rows[e.RowIndex];

            if (row.Cells["Status"].Value?.ToString() == "Fully paid")
            {
                row.DefaultCellStyle.BackColor = Color.LightGreen;
            }
            else if (row.Cells["Status"].Value?.ToString() == "Incomplete")
            {
                row.DefaultCellStyle.BackColor = Color.DarkRed;
            }
            else if (row.Cells["Status"].Value?.ToString() == "For approval")
            {
                row.DefaultCellStyle.BackColor = Color.Yellow;
            }
            else if (row.Cells["Status"].Value?.ToString() == "Denied")
            {
                row.DefaultCellStyle.BackColor = Color.DarkGray;
            }
        }

        private async void metroSetTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroSetTabControl1.SelectedTab == tabPageInvoice)
            {
                await LoadInvoicesAsync();
            }
            else if (metroSetTabControl1.SelectedTab == tabPageMaintenance)
            {
                await LoadMaintenanceTab();
            }
        }

        private void txtSearchInvoice_TextChanged(object sender, EventArgs e)
        {
            ApplyInvoiceFilterAndSorting();
        }

        private async void btnOpenInvoice_Click(object sender, EventArgs e)
        {
            if (dataGridViewInvoice.CurrentRow != null)
            {
                var selectedRow = dataGridViewInvoice.CurrentRow;
                var invoiceId = (Guid)selectedRow.Cells["Id"].Value;

                try
                {
                    var salesService = new SalesService();
                    var invoice = await salesService.GetInvoiceByIdAsync(invoiceId);

                    var detailsForm = new InvoiceDetailsForm(invoice, invoiceId, this);
                    detailsForm.ShowDialog(); // or .Show() if you prefer
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading invoice: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select an invoice first.");
            }
        }

        private void btnInvoiceExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var package = new OfficeOpenXml.ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Invoices");

                    // Add headers
                    int colIndex = 1;
                    foreach (DataGridViewColumn col in dataGridViewInvoice.Columns)
                    {
                        if (col.Visible)
                        {
                            worksheet.Cells[1, colIndex].Value = col.HeaderText;
                            colIndex++;
                        }
                    }

                    // Add data
                    int rowIndex = 2;
                    foreach (DataGridViewRow row in dataGridViewInvoice.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            colIndex = 1;
                            foreach (DataGridViewColumn col in dataGridViewInvoice.Columns)
                            {
                                if (col.Visible)
                                {
                                    worksheet.Cells[rowIndex, colIndex].Value = row.Cells[col.Name].Value?.ToString();
                                    colIndex++;
                                }
                            }
                            rowIndex++;
                        }
                    }

                    // Prompt to save file
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel Files|*.xlsx";
                        saveFileDialog.FileName = "InvoiceExport.xlsx";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(saveFileDialog.FileName, package.GetAsByteArray());
                            MessageBox.Show("Export successful!", "Excel Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboFilterInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFilterInvoice.SelectedItem == null)
            {
                FilterComboVisibility(false);
                return;
            }

            var selectedKey = ((dynamic)cboFilterInvoice.SelectedItem).Key.ToString();

            // Show cboAddedOption only if filter is on a date column
            if (selectedKey == "DateSold" || selectedKey == "DueDate")
            {
                FilterComboVisibility(true);
                PopulateAddedOptionCombo(); // Optional: reload options
            }
            else
            {
                FilterComboVisibility(false);
            }
        }

        private void FilterComboVisibility(bool value)
        {
            if (value == false) 
            {
                cboAddedOption.Visible = value;
                dtmDateFrom.Visible = value;
                dtmDateTo.Visible = value;
                lblDateFrom.Visible = value;
                lblDateTo.Visible = value;
            }
            else
            {
                cboAddedOption.Visible = value;
                dtmDateFrom.Visible = value;
                dtmDateTo.Visible = value;
                lblDateFrom.Visible = value;
                lblDateTo.Visible = value;
            }
        }

        private void PopulateAddedOptionCombo()
        {
            cboAddedOption.DataSource = new List<string>
            {
                "Ascending",
                "Descending"
            };
        }

        private void ApplyInvoiceFilterAndSorting()
        {
            if (cboFilterInvoice.SelectedItem == null || allInvoices == null)
                return;

            string keyword = txtSearchInvoice.Text.Trim().ToLower();
            var selectedKey = ((dynamic)cboFilterInvoice.SelectedItem).Key.ToString();
            var sortOption = cboAddedOption.SelectedItem?.ToString();

            IEnumerable<SalesDTO> filtered = allInvoices;

            // Filter by selected column
            switch (selectedKey)
            {
                case "strInvoiceNumber":
                    filtered = filtered.Where(i => i.strInvoiceNumber.ToLower().Contains(keyword));
                    break;
                case "Agent":
                    filtered = filtered.Where(i => i.Agent.ToLower().Contains(keyword));
                    break;
                case "DateSold":
                    // Apply date range filter instead of keyword-based match
                    filtered = filtered.Where(i =>
                        i.DateSold.Date >= dtmDateFrom.Value.Date &&
                        i.DateSold.Date <= dtmDateTo.Value.Date);
                    break;
                case "AccountName":
                    filtered = filtered.Where(i => i.AccountName.ToLower().Contains(keyword));
                    break;
                case "PaymentType":
                    filtered = filtered.Where(i => i.PaymentType.ToLower().Contains(keyword));
                    break;
                case "DueDate":
                    filtered = filtered.Where(i =>
                        i.DueDate.Date >= dtmDateFrom.Value.Date &&
                        i.DueDate.Date <= dtmDateTo.Value.Date);
                    break;
                case "Status":
                    filtered = filtered.Where(i => i.Status.ToLower().Contains(keyword));
                    break;
                case "Cluster":
                    filtered = filtered.Where(i => i.Cluster.ToLower().Contains(keyword));
                    break;
            }

            // Apply sorting if date-based column is selected
            if (selectedKey == "DateSold")
            {
                filtered = sortOption == "Descending"
                    ? filtered.OrderByDescending(i => i.DateSold)
                    : filtered.OrderBy(i => i.DateSold);
            }
            else if (selectedKey == "DueDate")
            {
                filtered = sortOption == "Descending"
                    ? filtered.OrderByDescending(i => i.DueDate)
                    : filtered.OrderBy(i => i.DueDate);
            }

            dataGridViewInvoice.DataSource = null;
            dataGridViewInvoice.DataSource = filtered.ToList();



            // Ensure ID is hidden again
            if (dataGridViewInvoice.Columns.Contains("Id") || dataGridViewInvoice.Columns.Contains("accountId") || dataGridViewInvoice.Columns.Contains("locationId"))
            {
                dataGridViewInvoice.Columns["Id"].Visible = false;
                dataGridViewInvoice.Columns["accountId"].Visible = false;
                dataGridViewInvoice.Columns["locationId"].Visible = false;
            }

            // Re-apply visible columns and headers
            ShowColumn("strInvoiceNumber", "Invoice #");
            ShowColumn("DateSold", "Date Sold");
            ShowColumn("Agent", "Agent Name");
            ShowColumn("AccountName", "Customer Name");
            ShowColumn("PaymentType", "Payment Method");
            ShowColumn("TotalSales", "Total Sales");
            ShowColumn("Tax", "Tax Amount");
            ShowColumn("DiscountPeso", "Discount (₱)");
            ShowColumn("Terms", "Terms (Days)");
            ShowColumn("DueDate", "Due Date");
            ShowColumn("RemainingBalance", "Balance Remaining");
            ShowColumn("Status", "Status");
            ShowColumn("Cluster", "Cluster");
        }

        private void cboAddedOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyInvoiceFilterAndSorting();
        }
        void ShowColumn(string columnName, string header)
        {
            if (dataGridViewInvoice.Columns.Contains(columnName))
            {
                var column = dataGridViewInvoice.Columns[columnName];
                column.Visible = true;
                column.HeaderText = header;
            }
        }
        private void dtmDateFrom_ValueChanged(object sender, EventArgs e)
        {
            ApplyInvoiceFilterAndSorting();
        }

        private void dtmDateTo_ValueChanged(object sender, EventArgs e)
        {
            ApplyInvoiceFilterAndSorting();
        }

        #endregion

        #region Maintenance
        private async Task LoadMaintenanceTab()
        {
            var maintenanceForm = new MaintenanceForm();

            tabPageMaintenance.Controls.Clear();              // Safe on UI thread
            tabPageMaintenance.Controls.Add(maintenanceForm); // Add to container
            maintenanceForm.Show();                           // Display the embedded form

            await Task.CompletedTask; // Optional, just to preserve async signature
        }
        #endregion
    }
}
