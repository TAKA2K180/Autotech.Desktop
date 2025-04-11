using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
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
            dataGridViewItemList.CellFormatting += DataGridViewItemList_CellFormatting;
        }
        #endregion

        #region Variables
        private System.Windows.Forms.Timer dateTimer;
        private List<Items> allItems = new List<Items>();
        private int currentPage = 1;
        private int pageSize = 20;
        private List<Items> currentPageItems = new();
        private HashSet<Guid> selectedItemIds = new();
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
                if (row.DataBoundItem is Items item && selectedItemIds.Equals(item.Id))
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

        
    }
}
