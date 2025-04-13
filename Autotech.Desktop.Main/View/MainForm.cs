using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Enums;
using Autotech.Desktop.Core.Models;
using MetroSet_UI.Controls;
using MetroSet_UI.Forms;
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
                HeaderText = "Item Code",
                DataPropertyName = "ItemCode",
                Name = "itemCodeColumn"
            });

            dataGridViewItemList.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item Name",
                DataPropertyName = "ItemName",
                Name = "itemNameColumn"
            });

            dataGridViewItemList.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item Description",
                DataPropertyName = "ItemDescription",
                Name = "itemDescriptionColumn"
            });

            dataGridViewItemList.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Onhand",
                DataPropertyName = "itemDetails.PropertyName",
                Name = "itemDetailsColumn"
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

            var checkboxColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "",
                Name = "selectCartItem",
                Width = 30,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dataGridViewOrderCart.Columns.Add(checkboxColumn);

            dataGridViewOrderCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item Code",
                DataPropertyName = "ItemCode",
                Name = "cartItemCode"
            });

            dataGridViewOrderCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item Name",
                DataPropertyName = "ItemName",
                Name = "cartItemName"
            });

            dataGridViewOrderCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Description",
                DataPropertyName = "ItemDescription",
                Name = "cartItemDescription"
            });

            dataGridViewOrderCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                Name = "cartPrice"
            });

            dataGridViewOrderCart.CellFormatting += dataGridViewOrderCart_CellFormatting;
            dataGridViewOrderCart.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dataGridViewOrderCart.IsCurrentCellDirty)
                {
                    dataGridViewOrderCart.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };
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
                    }
                }
            }

            // Bind cart to grid
            dataGridViewOrderCart.DataSource = null;
            dataGridViewOrderCart.DataSource = orderCartItems;

            // Optional: auto-fit columns
            //dataGridViewOrderCart.AutoResizeColumns();
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
            double subtotal = 0;

            foreach (DataGridViewRow row in dataGridViewOrderCart.Rows)
            {
                if (row.DataBoundItem is Items item)
                {
                    subtotal += GetPriceBasedOnSelection(item);
                }
            }

            txtSubtotal.Text = subtotal.ToString("C"); // formats as currency
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

        #endregion

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

        private void btnPay_Click(object sender, EventArgs e)
        {
            // 1. Validate selection
            if (comboAccount.SelectedItem is not Accounts selectedAccount)
            {
                MessageBox.Show("Please select a customer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboPaymentMethod.SelectedItem is null)
            {
                MessageBox.Show("Please select a payment method.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dataGridViewOrderCart.Rows.Count == 0)
            {
                MessageBox.Show("Order cart is empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Get paid amount and total
            if (!decimal.TryParse(txtPaidAmount.Text, out decimal paidAmount))
            {
                MessageBox.Show("Invalid paid amount.");
                return;
            }

            if (!decimal.TryParse(txtTotal.Text, out decimal totalAmount))
            {
                MessageBox.Show("Invalid total.");
                return;
            }

            if (paidAmount < totalAmount)
            {
                MessageBox.Show("Insufficient payment.");
                return;
            }

            // 3. Get payment method
            var selectedPayment = (KeyValuePair<PaymentMethod, string>)comboPaymentMethod.SelectedItem;
            PaymentMethod paymentMethod = selectedPayment.Key;

            // 4. Build invoice object (simplified)
            //var invoice = new InvoiceDTO
            //{
            //    CustomerId = selectedAccount.Id,
            //    PaymentMethod = paymentMethod,
            //    AmountPaid = paidAmount,
            //    TotalAmount = totalAmount,
            //    Items = GetCartItems(),
            //    Change = paidAmount - totalAmount
            //};

            // 5. Process (mock or send to backend)
            MessageBox.Show("Payment processed. Invoice will be generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // TODO: Open invoice/print form here

            // 6. Clear UI
            //ClearCartAndFields();
        }


        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            CalculateChange();
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



    }
}
