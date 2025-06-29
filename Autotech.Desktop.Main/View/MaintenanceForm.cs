using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Models;
using Autotech.Desktop.Main.View;
using MetroSet_UI.Forms;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace Autotech.Desktop.Main
{
    public partial class MaintenanceForm : MetroSetForm
    {
        private List<Accounts> _accounts;
        private List<Items> _items;
        private bool isAccountClicked = false;
        private bool isAgentClicked = false;
        private bool isItemClicked = false;
        public MaintenanceForm()
        {
            InitializeComponent();
            this.TopLevel = false;                // Allows it to behave like a control
            this.FormBorderStyle = FormBorderStyle.None; // Removes window borders
            this.Dock = DockStyle.Fill;          // Fills the parent container
            //this.Load += await MaintenanceForm_Load;
            txtSearchAccount.TextChanged += txtSearchAccount_TextChanged;
            txtSearchItems.TextChanged += txtSearchItems_TextChanged;
            _ = LoadAsync();
            btnDelete.Visible = false;
            btnAdd.Visible = false;
        }
        private async Task LoadAsync()
        {
            ToastMessageForm loadingToast = null;
            try
            {
                // ✅ Show loading toast
                loadingToast = new ToastMessageForm("Loading please wait...");
                loadingToast.Show();
                loadingToast.TopMost = true;
                loadingToast.BringToFront();
                await Task.Delay(1000);

                await LoadAgentsAsync();
                await LoadAccountsAsync();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private async void MaintenanceForm_Load(object sender, EventArgs e)
        {
            await LoadAgentsAsync();
            //await LoadAccountsAsync(); // Your async method now runs properly
            //await LoadAccountsAsync();
        }

        #region Events
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new ImportExcelForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                await LoadItemsAsync(); // Refresh table
            }
        }

        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        #region Accounts
        private async Task LoadAccountsAsync()
        {
            ToastMessageForm loadingToast = null;

            try
            {
                loadingToast = new ToastMessageForm("Loading accounts...");
                loadingToast.Show();
                loadingToast.TopMost = true;
                loadingToast.BringToFront();

                var service = new AccountService();

                // 🔹 1. Fetch data in background
                var accounts = await service.GetAllAccountsAsync();
                _accounts = accounts;

                // 🔹 2. Suspend layout & begin UI update
                dataGridViewAccounts.SuspendLayout();
                dataGridViewAccounts.DataSource = null;

                if (dataGridViewAccounts.Columns.Count == 0)
                {
                    dataGridViewAccounts.AutoGenerateColumns = false;

                    dataGridViewAccounts.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Account Name",
                        DataPropertyName = "Name",
                        Name = "Name",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });

                    dataGridViewAccounts.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Contact Person",
                        DataPropertyName = "ContactPerson",
                        Name = "ContactPerson",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });

                    dataGridViewAccounts.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Address",
                        DataPropertyName = "Address",
                        Name = "Address",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }

                // 🔹 3. Bind data
                dataGridViewAccounts.DataSource = accounts;

                // 🔹 4. Final layout optimizations
                dataGridViewAccounts.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridViewAccounts.DefaultCellStyle.Padding = new Padding(5);
                dataGridViewAccounts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                dataGridViewAccounts.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load accounts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (loadingToast != null && !loadingToast.IsDisposed)
                {
                    loadingToast.Close();
                }
            }
        }

        private void txtSearchAccount_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearchAccount.Text.Trim().ToLower();

            var filtered = _accounts
                .Where(a => !string.IsNullOrEmpty(a.Name) && a.Name.ToLower().Contains(keyword))
                .ToList();

            dataGridViewAccounts.DataSource = null;
            dataGridViewAccounts.DataSource = filtered;
        }

        #endregion

        #region Agents
        private async Task LoadAgentsAsync()
        {
            ToastMessageForm loadingToast = null;

            try
            {
                // ✅ Show loading toast
                loadingToast = new ToastMessageForm("Loading agents...");
                loadingToast.Show();
                loadingToast.TopMost = true;
                loadingToast.BringToFront();

                // ✅ Fetch agents from API/service
                var agentService = new AgentsService();
                var agents = await agentService.GetAllAgentsAsync(); // Assumes it returns List<AgentDTO>

                // ✅ Setup DataGridView
                dtgAgents.DataSource = null;
                dtgAgents.Columns.Clear();
                dtgAgents.AutoGenerateColumns = false;

                dtgAgents.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Agent Name",
                    DataPropertyName = "AgentName",
                    Name = "AgentName",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });

                dtgAgents.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Username",
                    DataPropertyName = "Username",
                    Name = "Username"
                });

                dtgAgents.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Email",
                    DataPropertyName = "Email",
                    Name = "Email"
                });

                dtgAgents.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Active",
                    DataPropertyName = "IsActive",
                    Name = "IsActive"
                });

                dtgAgents.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Last Login",
                    DataPropertyName = "DateLastLogin",
                    Name = "DateLastLogin"
                });

                dtgAgents.DataSource = agents;

                // ✅ Optional formatting
                dtgAgents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dtgAgents.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dtgAgents.DefaultCellStyle.Padding = new Padding(5);
                dtgAgents.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load agents: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #endregion

        #region Items
        private async Task LoadItemsAsync()
        {
            ToastMessageForm loadingToast = null;

            try
            {
                loadingToast = new ToastMessageForm("Loading items...");
                loadingToast.Show();
                loadingToast.TopMost = true;
                loadingToast.BringToFront();

                var service = new ItemServices();
                var items = await service.GetAllItemsAsync(); // List<Items>
                _items = items;

                dtgItems.DataSource = null;
                dtgItems.AutoGenerateColumns = false;

                // Flatten item + itemDetails for display
                var viewModel = items.Select(i => new
                {
                    i.Id,
                    i.ItemCode,
                    i.ItemName,
                    i.ItemDescription,
                    i.Quantity,
                    i.itemDetails.ItemId,
                    i.itemDetails.ItemsSold,
                    i.itemDetails.Sales,
                    OnHand = i.itemDetails?.OnHand ?? 0,
                    BataanRetail = i.itemDetails?.BataanRetail ?? 0,
                    BataanWholesale = i.itemDetails?.BataanWholeSale ?? 0,
                    PampangaRetail = i.itemDetails?.PampangaRetail ?? 0,
                    PampangaWholesale = i.itemDetails?.PampangaWholeSale ?? 0,
                    ZambalesRetail = i.itemDetails?.ZambalesRetail ?? 0,
                    ZambalesWholesale = i.itemDetails?.ZambalesWholeSale ?? 0
                }).ToList();

                // Define columns once
                if (dtgItems.Columns.Count == 0)
                {
                    dtgItems.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Code",
                        DataPropertyName = "ItemCode",
                        Width = 120
                    });
                    dtgItems.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Name",
                        DataPropertyName = "ItemName",
                        Width = 200
                    });
                    dtgItems.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Description",
                        DataPropertyName = "ItemDescription",
                        Width = 250
                    });
                    dtgItems.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "On Hand",
                        DataPropertyName = "OnHand",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" },
                        Width = 120
                    });

                    // 📍 Bataan
                    dtgItems.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Bataan Retail",
                        DataPropertyName = "BataanRetail",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" },
                        Width = 120
                    });
                    dtgItems.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Bataan Wholesale",
                        DataPropertyName = "BataanWholesale",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" },
                        Width = 120
                    });

                    // 📍 Pampanga
                    dtgItems.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Pampanga Retail",
                        DataPropertyName = "PampangaRetail",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" },
                        Width = 120
                    });
                    dtgItems.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Pampanga Wholesale",
                        DataPropertyName = "PampangaWholesale",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" },
                        Width = 120
                    });

                    // 📍 Zambales
                    dtgItems.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Zambales Retail",
                        DataPropertyName = "ZambalesRetail",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" },
                        Width = 120
                    });
                    dtgItems.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Zambales Wholesale",
                        DataPropertyName = "ZambalesWholesale",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" },
                        Width = 120
                    });
                }

                dtgItems.DataSource = viewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (loadingToast != null && !loadingToast.IsDisposed)
                {
                    loadingToast.Close();
                }
            }
        }
        private void FilterItems(string keyword)
        {
            var filtered = _items
                .Where(i => i.ItemName != null && i.ItemName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .Select(i => new
                {
                    i.ItemCode,
                    i.ItemName,
                    i.ItemDescription,
                    OnHand = i.itemDetails?.OnHand ?? 0,
                    BataanRetail = i.itemDetails?.BataanRetail ?? 0,
                    BataanWholesale = i.itemDetails?.BataanWholeSale ?? 0,
                    PampangaRetail = i.itemDetails?.PampangaRetail ?? 0,
                    PampangaWholesale = i.itemDetails?.PampangaWholeSale ?? 0,
                    ZambalesRetail = i.itemDetails?.ZambalesRetail ?? 0,
                    ZambalesWholesale = i.itemDetails?.ZambalesWholeSale ?? 0
                })
                .ToList();

            dtgItems.DataSource = filtered;
        }
        private void txtSearchItems_TextChanged(object sender, EventArgs e)
        {
            FilterItems(txtSearchItems.Text.Trim());
        }

        #endregion

        #region Buttons
        private async void metroSetTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnImportToExcel.Visible = false;
            if (metroSetTabControl1.SelectedTab == tabPageAccounts)
            {
                if (!isAccountClicked)
                {
                    await LoadAccountsAsync();
                }
                isAccountClicked = true;
            }
            else if (metroSetTabControl1.SelectedTab == tabPageAgents)
            {
                // do nothing, staging fix
            }
            else if (metroSetTabControl1.SelectedTab == tabPageItems)
            {
                btnImportToExcel.Visible = true;
                if (!isItemClicked)
                {
                    await LoadItemsAsync();
                }
                isItemClicked = true;
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (metroSetTabControl1.SelectedTab == tabPageAccounts)
            {
                if (dataGridViewAccounts.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an account to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dataGridViewAccounts.SelectedRows[0];
                var boundItem = selectedRow.DataBoundItem;


                if (boundItem is Accounts selectedAccount)
                {
                    using (var editForm = new EditAccountForm(selectedAccount))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            await LoadAccountsAsync(); // Refresh after editing
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The selected row is not a valid Account.", "Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (metroSetTabControl1.SelectedTab == tabPageAgents)
            {
                if (dtgAgents.CurrentRow != null && dtgAgents.CurrentRow.DataBoundItem is AgentDTO selectedAgent)
                {
                    using (var editForm = new EditAgentForm(selectedAgent))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            await LoadAgentsAsync(); // Refresh the list after saving
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select an agent to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (metroSetTabControl1.SelectedTab == tabPageItems)
            {
                if (dtgItems.CurrentRow?.DataBoundItem is not null)
                {
                    // The anonymous object includes Id
                    dynamic rowData = dtgItems.CurrentRow.DataBoundItem;

                    Guid itemId = rowData.Id; // ✅ this works even if Id column is not shown

                    var selectedItem = _items.FirstOrDefault(i => i.Id == itemId);
                    if (selectedItem != null)
                    {
                        using (var editForm = new EditItemForm(selectedItem))
                        {
                            if (editForm.ShowDialog() == DialogResult.OK)
                            {
                                await LoadItemsAsync();
                            }
                        }
                    }
                }
            }

        }
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadAccountsAsync();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
        #endregion


        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (metroSetTabControl1.SelectedTab == tabPageAccounts)
            {

            }
            else if (metroSetTabControl1.SelectedTab == tabPageAgents)
            {

            }
            else if (metroSetTabControl1.SelectedTab == tabPageItems)
            {

            }
        }

        private void btnProfitPerMonth_Click(object sender, EventArgs e)
        {
            var showProfitForm = new ProfitPerMonthForm();
            showProfitForm.ShowDialog();
        }
    }
}
