using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Enums;
using Autotech.Desktop.Core.Models;
using MetroSet_UI.Controls;
using MetroSet_UI.Forms;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO;
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
            this.Shown += MainForm_Shown;

        }
        private async Task PerformInitializationAsync()
        {
            if (LoginHelper.isLoggedIn == true)
            {
                LogHelper.Log($"Logged in user {SessionManager.AgentDetails.Username}, at {DateTime.Now}");
                this.Enabled = true;

                if (SessionManager.AgentDetails.AgentRole != "Admin")
                {
                    metroSetTabControl1.TabPages.Remove(tabPageMaintenance);
                }

                InitializeTimer();
                GetUser();
                SetLocation();
                InitializeDataGridView();
                InitializeOrderCartGrid();


                InitializePaymentMethods();
                PopulateFilterCombo();

                // Load version and accounts
                LoadVersion();
                await InitializeAccountsAsync();

                // Load paginated items
                await LoadItemsIntoGrid();

                // Start loading ALL items in the background so searches can use cached data without blocking UI
                if (allItemsLoadTask == null)
                {
                    allItemsLoadTask = Task.Run(async () =>
                    {
                        try
                        {
                            var svc = new ItemServices();
                            var items = await svc.GetAllItemsAsync();
                            if (items != null && items.Count > 0)
                                allItems = items;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Log("Error preloading all items: ", ex);
                        }
                    });
                }


                dataGridViewItemList.CellFormatting += DataGridViewItemList_CellFormatting;
                txtPaidAmount.TextChanged += txtPaidAmount_TextChanged;
                comboAccount.SelectedIndexChanged += comboAccount_SelectedIndexChanged;
                radioRetail.Checked = true;
                // Final UI tweaks (optional)
            }
            else
            {
                LogHelper.Log("Error logging in");
                var toastMessage = new ToastMessageForm("Login failed");
            }
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            this.Shown -= MainForm_Shown; // run only once

            try
            {
                LoadingMessageForm loadingForm = null;

                if (LoginHelper.isLoggedIn)
                {
                    loadingForm = new LoadingMessageForm("Loading, please wait...");
                    loadingForm.StartPosition = FormStartPosition.CenterScreen;
                    loadingForm.TopMost = true;
                    loadingForm.Show();
                    bool isShown = false;

                    // Ensure the loading form is drawn
                    Application.DoEvents();

                    //
                    CancellationTokenSource cts = new CancellationTokenSource();
                    while (!cts.Token.IsCancellationRequested)
                    {
                        await Task.Delay(2000);
                        if (isShown == false)
                        {
                            await PerformInitializationAsync(); // Assume it returns bool
                            isShown = true;
                        }
                    }

                    if (loadingForm != null && !loadingForm.IsDisposed)
                    {
                        loadingForm.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message);
                MessageBox.Show("An error occurred during initialization.");
                this.Close();
            }
        }

        #endregion

        #region Variables
        private System.Windows.Forms.Timer dateTimer;
        private List<Items> allItems = new List<Items>();
        private Task? allItemsLoadTask; // background loader task for all items
        private int currentPage = 1;
        private int pageSize = 20;
        private List<Items> currentPageItems = new();
        private HashSet<Guid> selectedItemIds = new();
        private List<Items> orderCartItems = new();
        private List<SalesDTO> allInvoices = new();
        private bool isFirstLoad = true;
        private bool suppressSelectionChanged = false; // when true, ignore SelectionChanged events triggered by programmatic updates
        private System.Windows.Forms.Timer invoiceSearchDebounceTimer; // debounce timer for invoice search
        private System.Windows.Forms.Timer datePickerDebounceTimer; // debounce timer for date picker changes
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
            catch (Exception ex)
            {
                LogHelper.Log("Error: ", ex);
                throw;
            }
        }
        #endregion

        #region Paging
        private async Task LoadItemsIntoGrid(int page = 1)
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

                // Update grid programmatically - suppress selection changed handling during this update
                suppressSelectionChanged = true;
                dataGridViewItemList.DataSource = null;
                dataGridViewItemList.DataSource = currentPageItems;

                foreach (DataGridViewRow row in dataGridViewItemList.Rows)
                {
                    if (row.DataBoundItem is Items item && selectedItemIds.Contains(item.Id))
                    {
                        row.Cells["selectColumn"].Value = true;
                    }
                }
                dataGridViewItemList.ClearSelection(); // Prevent auto-select first row after loading
                suppressSelectionChanged = false;
            }
            catch (Exception ex)
            {
                LogHelper.Log("Error: ", ex);
                MessageBox.Show($"Error loading items: {ex.Message}", "Error");
            }
        }

        private async Task<List<Items>> GetAllItemsAsync()
        {
            // Return cached if available
            if (allItems != null && allItems.Count > 0)
                return allItems;

            // If background load is running, await it
            if (allItemsLoadTask != null)
            {
                try
                {
                    await allItemsLoadTask;
                }
                catch
                {
                    // swallow - fallback to direct fetch
                }
                if (allItems != null && allItems.Count > 0)
                    return allItems;
            }

            // As a last resort fetch synchronously (async) from service
            try
            {
                var itemService = new ItemServices();
                var items = await itemService.GetAllItemsAsync();
                if (items != null && items.Count > 0)
                    allItems = items;
                return allItems;
            }
            catch (Exception ex)
            {
                LogHelper.Log("Error fetching all items: ", ex);
                return new List<Items>();
            }
        }

        private async void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await LoadItemsIntoGrid(currentPage);
                lblPage.Text = currentPage.ToString();
                await RestoreCheckboxStates();
            }
        }
        private async void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            await LoadItemsIntoGrid(currentPage);
            lblPage.Text = currentPage.ToString();
            await RestoreCheckboxStates();
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
                DataPropertyName = "itemDetails.QuantityPerBox",
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
            dataGridViewItemList.ClearSelection(); // Prevent auto-select first row
            dataGridViewItemList.SelectionChanged += dataGridViewItemList_SelectionChanged;
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
            else if (dataGridViewItemList.Columns[e.ColumnIndex].Name == "itemQuantityColumn" && item != null)
            {
                if (item.itemDetails != null)
                {
                    e.Value = item.itemDetails.QuantityPerBox;
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
        private async Task RestoreCheckboxStates()
        {
            // allow asynchronous yielding so callers can await and keep UI responsive
            await Task.Yield();

            foreach (DataGridViewRow row in dataGridViewItemList.Rows)
            {
                if (row.DataBoundItem is Items item && selectedItemIds.Contains(item.Id))
                {
                    row.Cells["selectColumn"].Value = true;
                }
            }
        }

        private void dataGridViewItemList_SelectionChanged(object sender, EventArgs e)
        {
            if (suppressSelectionChanged)
                return;

            if (!isFirstLoad)
            {
                foreach (DataGridViewRow row in dataGridViewItemList.SelectedRows)
                {
                    if (row.Cells["selectColumn"] != null)
                    {
                        row.Cells["selectColumn"].Value = true;
                    }
                }
            }

            isFirstLoad = false;
        }

        #endregion

        #region SearchItems
        private async void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchItem.Text.Trim().ToLower();

            // Task-based UI invoker helper
            Task InvokeUiAsync(Action action)
            {
                var tcs = new TaskCompletionSource<bool>();
                try
                {
                    if (dataGridViewItemList.InvokeRequired)
                    {
                        dataGridViewItemList.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                action();
                                tcs.SetResult(true);
                            }
                            catch (Exception ex)
                            {
                                tcs.SetException(ex);
                            }
                        }));
                    }
                    else
                    {
                        action();
                        tcs.SetResult(true);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }

                return tcs.Task;
            }

            // Snapshot checked IDs from the backing set (fast, no UI traversal)
            var checkedIds = new HashSet<Guid>(selectedItemIds);

            // Choose source for search
            List<Items> sourceForSearch;
            if (allItems != null && allItems.Count > 0)
            {
                sourceForSearch = allItems;
            }
            else
            {
                // Ensure background preload is started
                if (allItemsLoadTask == null)
                {
                    allItemsLoadTask = Task.Run(async () =>
                    {
                        try
                        {
                            var svc = new ItemServices();
                            var items = await svc.GetAllItemsAsync();
                            if (items != null && items.Count > 0)
                                allItems = items;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Log("Error preloading all items (on-demand): ", ex);
                        }
                    });
                }

                sourceForSearch = currentPageItems;
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                // Run filtering on threadpool
                var filtered = await Task.Run(() =>
                {
                    return sourceForSearch.Where(item =>
                        (item.ItemCode ?? string.Empty).ToLower().Contains(searchText) ||
                        (item.ItemName ?? string.Empty).ToLower().Contains(searchText) ||
                        (item.ItemDescription ?? string.Empty).ToLower().Contains(searchText))
                    .ToList();
                });

                // Update UI (suppress SelectionChanged while we update binding)
                await InvokeUiAsync(() =>
                {
                    suppressSelectionChanged = true;
                    dataGridViewItemList.DataSource = null;
                    dataGridViewItemList.DataSource = filtered;

                    // Restore checkboxes for visible rows
                    foreach (DataGridViewRow row in dataGridViewItemList.Rows)
                    {
                        if (row.DataBoundItem is Items item && checkedIds.Contains(item.Id))
                            row.Cells["selectColumn"].Value = true;
                    }
                    dataGridViewItemList.ClearSelection();
                    suppressSelectionChanged = false;
                });
            }
            else
            {
                // Reset to paginated view asynchronously and restore checkboxes
                await InvokeUiAsync(() =>
                {
                    suppressSelectionChanged = true;
                    dataGridViewItemList.DataSource = null;
                    dataGridViewItemList.DataSource = currentPageItems;
                    lblPage.Text = currentPage.ToString();
                    dataGridViewItemList.ClearSelection();
                    suppressSelectionChanged = false;
                });

                await RestoreCheckboxStates();
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

            txtSubtotal.Text = subtotal.ToString("₱#,##0.00");
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
                txtTotal.Text = total.ToString("₱#,##0.00");
                UpdateRemainingBalance();
            }
            catch (Exception ex)
            {
                LogHelper.Log("Error: ", ex);
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

            var codesToRemove = new List<string>();

            foreach (DataGridViewRow row in dataGridViewOrderCart.Rows)
            {
                var cell = row.Cells["selectCartItem"];
                if (cell != null && cell.Value is bool isChecked && isChecked)
                {
                    var codeCell = row.Cells["cartItemCode"];
                    if (codeCell != null && codeCell.Value != null)
                    {
                        codesToRemove.Add(codeCell.Value.ToString());
                    }
                }
            }

            if (codesToRemove.Count == 0)
            {
                MessageBox.Show("No items selected.");
                return;
            }

            // Remove from cart list by ItemCode
            orderCartItems = orderCartItems
                .Where(item => !codesToRemove.Contains(item.ItemCode))
                .ToList();

            // Remove rows from DataGridView
            for (int i = dataGridViewOrderCart.Rows.Count - 1; i >= 0; i--)
            {
                var row = dataGridViewOrderCart.Rows[i];
                var codeCell = row.Cells["cartItemCode"];
                if (codeCell != null && codeCell.Value != null && codesToRemove.Contains(codeCell.Value.ToString()))
                {
                    dataGridViewOrderCart.Rows.RemoveAt(i);
                }
            }

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

                // Clear the grid binding and rows
                dataGridViewOrderCart.DataSource = null;
                dataGridViewOrderCart.Rows.Clear();

                // Reset subtotal and recalculate totals
                txtSubtotal.Text = "₱0.00";
                CalculateTotal();

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
                txtChange.Text = change.ToString("₱#,##0.00"); // format as currency
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
            decimal priceBeforeDiscount = total + discount - tax;

            decimal discountPercent = priceBeforeDiscount == 0
                ? 0
                : discount / priceBeforeDiscount * 100;

            try
            {
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
                        double.TryParse(row.Cells["cartDiscount"].Value?.ToString(), out double discountAmount);

                        // Discount is a fixed value, round to 2 decimal places
                        double totalDiscount = Math.Round(discountAmount, 2);

                        return new InvoiceItemDTO
                        {
                            ItemId = item.Id,
                            Quantity = quantity,
                            ItemPrice = price,
                            TotalPrice = subtotal,
                            ItemName = "",
                            Discount = totalDiscount,
                            AgentId = SessionManager.AgentDetails.Id
                        };
                    })
                    .Where(i => i != null)
                    .ToList()
                };

                // 4. Call backend
                var service = new SalesService();
                var (invoiceId, invoiceNumber) = await service.CreateInvoiceAsync(invoiceDto);

                new ToastMessageForm("Invoice created successfully!").Show();

                // ✅ Fetch the created invoice for printing
                var createdInvoice = await service.GetInvoiceByIdAsync(invoiceId);
                var accountService = new AccountService();
                var accounts = await accountService.GetAccountByIdAsync(selectedAccount.Id);

                // ✅ Print receipt automatically
                await PrintReceiptAsync(createdInvoice, accounts);

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
                LogHelper.Log("Error: ", ex);
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
                txtChange.Text = (paidAmount > total) ? (paidAmount - total).ToString("₱#,##0.00") : "₱0.00";
                txtRemaining.Text = (remaining > 0 ? remaining : 0).ToString("₱#,##0.00");
            }
            catch
            {
                txtRemaining.Text = "₱0.00";
            }
        }

        private async Task PrintReceiptAsync(InvoiceDetailsDTO invoice, Accounts accounts)
        {
            try
            {
                // Create sanitized file name
                string invoiceNumber = SanitizeFileName(invoice.strInvoiceNumber);
                string customerName = SanitizeFileName(invoice.AccountName);
                string date = invoice.DateSold.ToString("yyyy-MM-dd");
                string fileName = $"{invoiceNumber}_{customerName}_{date}.pdf";

                // Get Reports folder under app root
                string appRoot = AppDomain.CurrentDomain.BaseDirectory;
                string reportsFolder = Path.Combine(appRoot, "Reports");
                if (!Directory.Exists(reportsFolder))
                    Directory.CreateDirectory(reportsFolder);

                string savePath = Path.Combine(reportsFolder, fileName);

                // Configure page settings
                var settings = new PageSettings
                {
                    Margins = new Margins(10, 10, 10, 10),
                    PaperSize = new PaperSize("A4", 827, 1169)
                };

                // Create print document
                var printDoc = new PrintDocument();
                printDoc.DefaultPageSettings = settings;
                printDoc.PrintPage += (sender, e) => PrintDoc_PrintPage(sender, e, invoice, accounts);

                // Show print preview
                var previewDialog = new PrintPreviewDialog
                {
                    Document = printDoc
                };
                previewDialog.ShowDialog();

                // Auto-save to PDF
                var pdfDoc = new PrintDocument
                {
                    DefaultPageSettings = settings,
                    PrinterSettings = new PrinterSettings
                    {
                        PrinterName = "Microsoft Print to PDF",
                        PrintToFile = true,
                        PrintFileName = savePath
                    }
                };

                pdfDoc.PrintPage += (sender, e) => PrintDoc_PrintPage(sender, e, invoice, accounts);
                pdfDoc.Print();

                MessageBox.Show($"Receipt printed and saved to:\n{savePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogHelper.Log("Print receipt error: ", ex);
                MessageBox.Show("Failed to print receipt:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string SanitizeFileName(string input)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                input = input.Replace(c, '_');
            }
            return input.Trim();
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e, InvoiceDetailsDTO invoice, Accounts accounts)
        {
            Graphics g = e.Graphics;

            int itemCount = invoice.PurchasedItems.Count;
            float estimatedHeight = 300 + (itemCount * 20);
            float halfA4Height = 792 / 2;

            if (estimatedHeight < halfA4Height)
                e.PageSettings.PaperSize = new PaperSize("HalfA4", 827, (int)estimatedHeight);

            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 10);
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float right = e.MarginBounds.Right;
            float usableWidth = right - x;
            float lineHeight = bodyFont.GetHeight(g) + 2;

            // Header
            g.DrawString("AUTOTECH CAR CARE CENTER", headerFont, Brushes.Black, x + usableWidth / 4, y); y += lineHeight;
            g.DrawString("Wawa, Abucay, Bataan", bodyFont, Brushes.Black, x + usableWidth / 3, y); y += lineHeight;
            g.DrawString("TRUST RECEIPT", headerFont, Brushes.Black, x + usableWidth / 3, y); y += lineHeight;
            g.DrawString("*THIS IS NOT YOUR OFFICIAL RECEIPT*", bodyFont, Brushes.Black, x + usableWidth / 4, y); y += lineHeight;

            // Info
            g.DrawString($"Receipt #: {invoice.strInvoiceNumber}", bodyFont, Brushes.Black, x, y);
            g.DrawString($"Date: {DateTime.Now:g}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y); y += lineHeight;
            g.DrawString($"Terms: {invoice.Terms} day(s)", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, x + usableWidth * 0.55f, y);
            g.DrawString($"Owner's Name: {accounts.ContactPerson}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y + 22);
            g.DrawString($"Prepared by: {SessionManager.AgentDetails?.AgentName ?? "N/A"}", bodyFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Customer: {invoice.AccountName}", bodyFont, Brushes.Black, x, y); y += lineHeight;

            string addressLabel = "Customer address:";
            string fullAddress = $"{addressLabel} {accounts.Address}";
            float ownerColumnX = x + usableWidth * 0.55f;
            float fullWidth = g.MeasureString(fullAddress, bodyFont).Width;

            if (x + fullWidth > ownerColumnX)
            {
                g.DrawString(addressLabel, bodyFont, Brushes.Black, x, y);
                y += lineHeight;
                g.DrawString(accounts.Address, bodyFont, Brushes.Black, x, y);
            }
            else
            {
                g.DrawString(fullAddress, bodyFont, Brushes.Black, x, y);
            }
            y += lineHeight;

            // Table columns
            float totalWidth = 110f;
            float discWidth = 90f;
            float unitWidth = 110f;
            float qtyWidth = 50f;

            float colTotalPos = right - totalWidth;
            float colDiscPos = colTotalPos - discWidth;
            float colUnitPos = colDiscPos - unitWidth;
            float colQtyPos = colUnitPos - qtyWidth;
            float colDescription = x;

            var numAlign = new StringFormat() { Alignment = StringAlignment.Near };
            g.DrawString("Description", bodyFont, Brushes.Black, new RectangleF(colDescription, y, colQtyPos - colDescription, lineHeight));
            g.DrawString("Qty", bodyFont, Brushes.Black, new RectangleF(colQtyPos, y, qtyWidth, lineHeight), numAlign);
            g.DrawString("Unit", bodyFont, Brushes.Black, new RectangleF(colUnitPos, y, unitWidth, lineHeight), numAlign);
            g.DrawString("Disc", bodyFont, Brushes.Black, new RectangleF(colDiscPos, y, discWidth, lineHeight), numAlign);
            g.DrawString("Total", bodyFont, Brushes.Black, new RectangleF(colTotalPos, y, totalWidth, lineHeight), numAlign);
            y += lineHeight;

            g.DrawLine(Pens.Black, x, y, right, y); y += 4;

            // Items
            foreach (var item in invoice.PurchasedItems)
            {
                var descRect = new RectangleF(colDescription, y, colQtyPos - colDescription, lineHeight);
                g.DrawString(item.ItemName, bodyFont, Brushes.Black, descRect);

                g.DrawString(item.Quantity.ToString(), bodyFont, Brushes.Black, new RectangleF(colQtyPos, y, qtyWidth, lineHeight), numAlign);

                var unitText = item.ItemPrice.HasValue ? ($"₱{item.ItemPrice.Value:N2}") : "₱0.00";
                g.DrawString(unitText, bodyFont, Brushes.Black, new RectangleF(colUnitPos, y, unitWidth, lineHeight), numAlign);

                var discText = item.Discount.HasValue ? ($"₱{item.Discount.Value:N2}") : "₱0.00";
                g.DrawString(discText, bodyFont, Brushes.Black, new RectangleF(colDiscPos, y, discWidth, lineHeight), numAlign);

                g.DrawString($"₱{item.TotalPrice:N2}", bodyFont, Brushes.Black, new RectangleF(colTotalPos, y, totalWidth, lineHeight), numAlign);

                y += lineHeight;
            }

            y += 6;
            g.DrawLine(Pens.Black, x, y, right, y); y += 2;

            // Totals
            float colSplit = x + usableWidth * 0.65f;
            float totalsLabelCol = colSplit;
            float totalsValueCol = colSplit + 85f;

            double subtotal = invoice.PurchasedItems.Sum(i => i.TotalPrice);
            double tax = invoice.Tax;
            double discount = invoice.DiscountPeso;
            double total = invoice.TotalSales;

            var rightAlignFormat = new StringFormat() { Alignment = StringAlignment.Far };

            g.DrawString("Subtotal:", bodyFont, Brushes.Black, totalsLabelCol, y);
            g.DrawString($"₱{subtotal:N2}", bodyFont, Brushes.Black, new RectangleF(totalsValueCol, y, 100, lineHeight), rightAlignFormat);
            y += lineHeight;

            g.DrawString("Tax:", bodyFont, Brushes.Black, totalsLabelCol, y);
            g.DrawString($"₱{tax:N2}", bodyFont, Brushes.Black, new RectangleF(totalsValueCol, y, 100, lineHeight), rightAlignFormat);
            y += lineHeight;

            g.DrawString("Discount:", bodyFont, Brushes.Black, totalsLabelCol, y);
            g.DrawString($"₱{discount:N2}", bodyFont, Brushes.Black, new RectangleF(totalsValueCol, y, 100, lineHeight), rightAlignFormat);
            y += lineHeight;

            g.DrawString("Total:", headerFont, Brushes.Black, totalsLabelCol, y);
            g.DrawString($"₱{total:N2}", headerFont, Brushes.Black, new RectangleF(totalsValueCol, y, 100, lineHeight), rightAlignFormat);
            y += lineHeight * 2;

            // Terms
            string termsText = "Terms: Payable in cash otherwise stated. An interest of 3% per month will be charged on all overdue accounts. In case of non-payment of overdue accounts, the courts of Balanga City, Bataan will have jurisdictions and the customer hereby agree to pay the attorney's fees and court cost resulting therefrom.";
            RectangleF termsRect = new RectangleF(x, y, usableWidth * 0.65f, lineHeight * 5);
            g.DrawString(termsText, bodyFont, Brushes.Black, termsRect);
            y += lineHeight * 4;

            // Acknowledgment
            g.DrawString("ALL CHECKS MUST BE PAYABLE TO: AUTOTECH CAR CARE CENTER", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, x, y);
            y += lineHeight;

            string ackText = "Received the items in good order, condition and accepted under the terms and conditions stipulated herein and at the back thereof.";
            RectangleF ackRect = new RectangleF(x, y, usableWidth, lineHeight * 3);
            g.DrawString(ackText, bodyFont, Brushes.Black, ackRect);
            y += lineHeight * 2;

            // Signature
            g.DrawString("Received by:", bodyFont, Brushes.Black, x, y);
            y += lineHeight;
            g.DrawString("______________________________", bodyFont, Brushes.Black, x + 80, y);
            y += lineHeight;
            g.DrawString("SIGNATURE OVER PRINTED NAME", bodyFont, Brushes.Black, x + 80, y);
        }
        #endregion

        #region Version
        private void LoadVersion()
        {
            try
            {
                // Try to read version from version.txt file in the application directory
                string versionFilePath = Path.Combine(Application.StartupPath, "version.txt");

                if (File.Exists(versionFilePath))
                {
                    string version = File.ReadAllText(versionFilePath).Trim();
                    lblVersion.Text = $"v{version}";
                }
                else
                {
                    lblVersion.Text = "v1.0.0.0";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("Error loading version: ", ex);
                lblVersion.Text = "v1.0.0.0";
            }
        }
        #endregion

        #region Accounts

        private async Task InitializeAccountsAsync()
        {
            try
            {
                var service = new AccountService();
                var id = SessionManager.AgentDetails.LocationId;
                var accounts = new List<Accounts>();
                if (SessionManager.AgentDetails.AgentRole == "Admin")
                {
                    accounts = await service.GetAllAccountsAsync();
                }
                else
                {
                    accounts = await service.GetAccountsByLocationIdAsync(id);
                }

                // Sort accounts alphabetically by Name (case-insensitive) in-place to avoid extra allocations
                if (accounts != null && accounts.Count > 1)
                {
                    accounts.Sort((a, b) => string.Compare(a?.Name ?? string.Empty, b?.Name ?? string.Empty, StringComparison.CurrentCultureIgnoreCase));
                }

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
                LogHelper.Log("Error: ", ex);
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
                        // Prevent the grid from auto-selecting the first row after binding
                        dataGridViewInvoice.ClearSelection();
                        if (dataGridViewInvoice.Rows.Count > 0)
                        {
                            try { dataGridViewInvoice.CurrentCell = null; } catch { /* ignore if not supported */ }
                        }

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
                LogHelper.Log("Error: ", ex);
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

            await ApplyInvoiceFilterAndSortingAsync();
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

        private void dataGridViewInvoice_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridViewInvoice.Rows.Count > 0)
            {
                dataGridViewInvoice.ClearSelection();
                dataGridViewInvoice.CurrentCell = null;
            }
        }

        private void dataGridViewInvoice_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var dgv = sender as DataGridView;
            var row = dgv.Rows[e.RowIndex];

            if (row.Cells["Status"].Value?.ToString().ToLower() == "fully paid")
            {
                row.DefaultCellStyle.BackColor = Color.LightGreen;
            }
            else if (row.Cells["Status"].Value?.ToString() == "Incomplete")
            {
                row.DefaultCellStyle.BackColor = Color.DarkRed;
            }
            else if (row.Cells["Status"].Value?.ToString().ToLower() == "for approval")
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

        private async void txtSearchInvoice_TextChanged(object sender, EventArgs e)
        {
            // Clear existing timer to reset the delay
            if (invoiceSearchDebounceTimer != null)
            {
                invoiceSearchDebounceTimer.Stop();
                invoiceSearchDebounceTimer.Dispose();
            }

            // Create new timer with 500ms delay
            invoiceSearchDebounceTimer = new System.Windows.Forms.Timer();
            invoiceSearchDebounceTimer.Interval = 500;
            invoiceSearchDebounceTimer.Tick += async (s, args) =>
            {
                invoiceSearchDebounceTimer.Stop();
                await ApplyInvoiceFilterAndSortingAsync();
            };
            invoiceSearchDebounceTimer.Start();
        }

        private async Task ApplyInvoiceFilterAndSortingAsync()
        {
            // Return early if prerequisites aren't met (non-blocking check)
            if (cboFilterInvoice.SelectedItem == null || allInvoices == null || allInvoices.Count == 0)
                return;

            // Capture UI values on UI thread before going async
            string keyword = txtSearchInvoice.Text.Trim().ToLower();
            var selectedKey = ((dynamic)cboFilterInvoice.SelectedItem).Key.ToString();
            var sortOption = cboAddedOption.SelectedItem?.ToString() ?? "";
            ToastMessageForm loadingToast = null;

            try
            {

                // ✅ Show loading toast
                loadingToast = new ToastMessageForm("Searching invoices...");
                loadingToast.Show();
                loadingToast.TopMost = true;
                loadingToast.BringToFront();

                Task.Delay(500).Wait(); // Small delay to ensure toast is visible
                // Run entire filtering and sorting logic on thread pool to avoid UI blocking
                var filtered = await Task.Run(() =>
                {
                    IEnumerable<SalesDTO> result = allInvoices;

                    // Apply filtering
                    switch (selectedKey)
                    {
                        case "strInvoiceNumber":
                            result = result.Where(i => i.strInvoiceNumber != null && i.strInvoiceNumber.ToLower().Contains(keyword));
                            break;
                        case "Agent":
                            result = result.Where(i => i.Agent != null && i.Agent.ToLower().Contains(keyword));
                            break;
                        case "DateSold":
                            result = result.Where(i =>
                                i.DateSold.Date >= dtmDateFrom.Value.Date &&
                                i.DateSold.Date <= dtmDateTo.Value.Date);
                            break;
                        case "AccountName":
                            result = result.Where(i => i.AccountName != null && i.AccountName.ToLower().Contains(keyword));
                            break;
                        case "PaymentType":
                            result = result.Where(i => i.PaymentType != null && i.PaymentType.ToLower().Contains(keyword));
                            break;
                        case "DueDate":
                            result = result.Where(i =>
                                i.DueDate.Date >= dtmDateFrom.Value.Date &&
                                i.DueDate.Date <= dtmDateTo.Value.Date);
                            break;
                        case "Status":
                            result = result.Where(i => i.Status != null && i.Status.ToLower().Contains(keyword));
                            break;
                        case "Cluster":
                            result = result.Where(i => i.Cluster != null && i.Cluster.ToLower().Contains(keyword));
                            break;
                    }

                    // Apply sorting
                    if (selectedKey == "DateSold")
                        result = sortOption == "Descending"
                            ? result.OrderByDescending(i => i.DateSold)
                            : result.OrderBy(i => i.DateSold);
                    else if (selectedKey == "DueDate")
                        result = sortOption == "Descending"
                            ? result.OrderByDescending(i => i.DueDate)
                            : result.OrderBy(i => i.DueDate);

                    return result.ToList();
                });

                // Marshal results back to UI thread asynchronously
                await Task.Run(() =>
                {
                    Invoke(new Action(() =>
                    {
                        dataGridViewInvoice.DataSource = null;
                        dataGridViewInvoice.DataSource = filtered;

                        // Hide ID columns
                        if (dataGridViewInvoice.Columns.Contains("Id"))
                            dataGridViewInvoice.Columns["Id"].Visible = false;
                        if (dataGridViewInvoice.Columns.Contains("accountId"))
                            dataGridViewInvoice.Columns["accountId"].Visible = false;
                        if (dataGridViewInvoice.Columns.Contains("locationId"))
                            dataGridViewInvoice.Columns["locationId"].Visible = false;

                        // Show and configure visible columns
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
                    }));
                });
            }
            catch (Exception ex)
            {
                LogHelper.Log("Error in ApplyInvoiceFilterAndSortingAsync: ", ex);
            }
            finally
            {
                // ✅ Close loading toast
                if (loadingToast != null && !loadingToast.IsDisposed)
                {
                    loadingToast.Close();
                }
            }
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
                    var accountsService = new AccountService();
                    var invoice = await salesService.GetInvoiceByIdAsync(invoiceId);
                    var accounts = await accountsService.GetAccountByIdAsync(invoice.AccountId);

                    var detailsForm = new InvoiceDetailsForm(invoice, invoiceId, accounts, this);
                    detailsForm.ShowDialog(); // or .Show() if you prefer
                }
                catch (Exception ex)
                {
                    LogHelper.Log("Error: ", ex);
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
                LogHelper.Log("Error: ", ex);
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

        private void cboAddedOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Call async method from event handler
            _ = ApplyInvoiceFilterAndSortingAsync();
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
            // Clear existing timer to reset the delay
            if (datePickerDebounceTimer != null)
            {
                datePickerDebounceTimer.Stop();
                datePickerDebounceTimer.Dispose();
            }

            // Create new timer with 500ms delay
            datePickerDebounceTimer = new System.Windows.Forms.Timer();
            datePickerDebounceTimer.Interval = 1500;
            datePickerDebounceTimer.Tick += async (s, args) =>
            {
                datePickerDebounceTimer.Stop();
                await ApplyInvoiceFilterAndSortingAsync();
            };
            datePickerDebounceTimer.Start();
        }

        private void dtmDateTo_ValueChanged(object sender, EventArgs e)
        {
            // Clear existing timer to reset the delay
            if (datePickerDebounceTimer != null)
            {
                datePickerDebounceTimer.Stop();
                datePickerDebounceTimer.Dispose();
            }

            // Create new timer with 500ms delay
            datePickerDebounceTimer = new System.Windows.Forms.Timer();
            datePickerDebounceTimer.Interval = 1500;
            datePickerDebounceTimer.Tick += async (s, args) =>
            {
                datePickerDebounceTimer.Stop();
                await ApplyInvoiceFilterAndSortingAsync();
            };
            datePickerDebounceTimer.Start();
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

        private void metroSetControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void btnStartSearch_Click(object sender, EventArgs e)
        {
            await ApplyInvoiceFilterAndSortingAsync();
        }

        private async void txtSearchInvoice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // prevent beep sound
                await ApplyInvoiceFilterAndSortingAsync();
            }
        }
    }
}
