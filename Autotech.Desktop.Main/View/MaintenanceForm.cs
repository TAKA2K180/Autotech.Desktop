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
        private bool isAccountClicked = false;
        private bool isAgentClicked = false;
        private bool isItemClicked = false;
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
            await LoadAgentsAsync();
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

        private async void metroSetTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                //if (!isAgentClicked)
                //{
                //    await LoadAgentsAsync();
                //}
                //isAgentClicked = true;
            }
            //else if (metroSetTabControl1.SelectedTab == tabPageMaintenance)
            //{
            //    LoadMaintenanceTab();
            //}
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

        }
    }
}
