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
        }
        #endregion

        #region Variables
        private System.Windows.Forms.Timer dateTimer;
        private List<Items> allItems = new List<Items>();
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
                    else if (location == "Upper Pampanga" || location == "Lowe Pampanga")
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

        private async void LoadItemsIntoGrid()
        {
            if(SessionManager.AgentDetails != null)
            {
                try
                {
                    ItemServices itemService = new ItemServices();
                    allItems = await itemService.GetAllItemsAsync();

                    if (allItems != null && allItems.Count > 0)
                    {
                        dataGridViewItemList.DataSource = allItems;
                    }
                    else
                    {
                        MetroSetMessageBox.Show(this, "No items available", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MetroSetMessageBox.Show(this, $"Error fetching items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitializeDataGridView()
        {
            dataGridViewItemList.AutoGenerateColumns = false;

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
                HeaderText = "Date Added",
                DataPropertyName = "DateAdded",
                Name = "dateAddedColumn"
            });

            dataGridViewItemList.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item Details",
                DataPropertyName = "itemDetails.PropertyName",
                Name = "itemDetailsColumn"
            });
        }
        #endregion

        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchItem.Text.ToLower();

            if (!string.IsNullOrEmpty(searchText))
            {
                // Filter the list based on the search text (for ItemCode, ItemName, or ItemDescription)
                var filteredItems = allItems.Where(item =>
                    item.ItemCode.ToLower().Contains(searchText) ||
                    item.ItemName.ToLower().Contains(searchText) ||
                    item.ItemDescription.ToLower().Contains(searchText)).ToList();

                // Update the DataGridView with the filtered list
                dataGridViewItemList.DataSource = filteredItems;
            }
            else
            {
                // If the search text is empty, reset the DataGridView to show all items
                dataGridViewItemList.DataSource = allItems;
            }
        }
    }
}
