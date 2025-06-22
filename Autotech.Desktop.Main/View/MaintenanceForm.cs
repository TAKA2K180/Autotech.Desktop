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
        public MaintenanceForm()
        {
            InitializeComponent();
            this.TopLevel = false;                // Allows it to behave like a control
            this.FormBorderStyle = FormBorderStyle.None; // Removes window borders
            this.Dock = DockStyle.Fill;          // Fills the parent container
            this.Load += MaintenanceForm_Load;
            txtSearchAccount.TextChanged += txtSearchAccount_TextChanged;
        }
        private async void MaintenanceForm_Load(object sender, EventArgs e)
        {
            await LoadAccountsAsync(); // Your async method now runs properly
            //await LoadAccountsAsync();
        }

        #region Events
        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        #endregion


        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadAccountsAsync();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        #region Accounts
        private async Task LoadAccountsAsync()
        {
            ToastMessageForm loadingToast = null;

            try
            {
                // ✅ Show loading toast
                loadingToast = new ToastMessageForm("Loading accounts...");
                loadingToast.Show();
                loadingToast.TopMost = true;
                loadingToast.BringToFront();

                var service = new AccountService(); // Your service class to fetch accounts
                var accounts = await service.GetAllAccountsAsync(); // Returns List<AccountDTO>
                _accounts = accounts;

                dataGridViewAccounts.DataSource = null;
                dataGridViewAccounts.Columns.Clear();
                dataGridViewAccounts.AutoGenerateColumns = false;

                // ✅ Add Checkbox Column
                var checkboxColumn = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "",
                    Name = "SelectColumn",
                    Width = 30
                };
                dataGridViewAccounts.Columns.Add(checkboxColumn);
                

                // ✅ Add Data Columns
                dataGridViewAccounts.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Account Name",
                    DataPropertyName = "Name",
                    Name = "Name",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                Task.Delay(1000).Wait();

                dataGridViewAccounts.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Contact Person",
                    DataPropertyName = "ContactPerson",
                    Name = "ContactPerson"
                });

                dataGridViewAccounts.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Address",
                    DataPropertyName = "Address",
                    Name = "Address"
                });
                
                // ✅ Bind data
                dataGridViewAccounts.DataSource = accounts;

                dataGridViewAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridViewAccounts.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridViewAccounts.DefaultCellStyle.Padding = new Padding(5);
                dataGridViewAccounts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load accounts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void metroSetTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroSetTabControl1.SelectedTab == tabPageAccounts)
            {
                //await LoadAccountsAsync();
            }
            //else if (metroSetTabControl1.SelectedTab == tabPageMaintenance)
            //{
            //    LoadMaintenanceTab();
            //}
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
    }
}
