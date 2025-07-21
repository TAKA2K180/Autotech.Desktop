using MetroSet_UI.Controls;

namespace Autotech.Desktop.Main
{
    partial class MaintenanceForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            metroSetTabControl1 = new MetroSetTabControl();
            tabPageAgents = new TabPage();
            dtgAgents = new DataGridView();
            txtSearchAgent = new MetroSetTextBox();
            lblSearchAgent = new MetroSetLabel();
            tabPageAccounts = new TabPage();
            panel2 = new Panel();
            dataGridViewAccounts = new DataGridView();
            txtSearchAccount = new MetroSetTextBox();
            lblSearchAccount = new MetroSetLabel();
            tabPageItems = new TabPage();
            dtgItems = new DataGridView();
            txtSearchItems = new MetroSetTextBox();
            lblSearchItems = new MetroSetLabel();
            tabPageReports = new TabPage();
            btnItemSalesReport = new MetroSetButton();
            btnProfitPerMonth = new MetroSetButton();
            btnImportToExcel = new MetroSetButton();
            btnEdit = new MetroSetButton();
            btnDelete = new MetroSetButton();
            btnRefresh = new MetroSetButton();
            panel1 = new Panel();
            btnAdd = new MetroSetButton();
            metroSetTabControl1.SuspendLayout();
            tabPageAgents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtgAgents).BeginInit();
            tabPageAccounts.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAccounts).BeginInit();
            tabPageItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtgItems).BeginInit();
            tabPageReports.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // metroSetTabControl1
            // 
            metroSetTabControl1.AnimateEasingType = MetroSet_UI.Enums.EasingType.CubeOut;
            metroSetTabControl1.AnimateTime = 200;
            metroSetTabControl1.BackgroundColor = Color.White;
            metroSetTabControl1.Controls.Add(tabPageAgents);
            metroSetTabControl1.Controls.Add(tabPageAccounts);
            metroSetTabControl1.Controls.Add(tabPageItems);
            metroSetTabControl1.Controls.Add(tabPageReports);
            metroSetTabControl1.Dock = DockStyle.Top;
            metroSetTabControl1.IsDerivedStyle = true;
            metroSetTabControl1.ItemSize = new Size(100, 38);
            metroSetTabControl1.Location = new Point(12, 70);
            metroSetTabControl1.Name = "metroSetTabControl1";
            metroSetTabControl1.SelectedIndex = 0;
            metroSetTabControl1.SelectedTextColor = Color.White;
            metroSetTabControl1.Size = new Size(776, 543);
            metroSetTabControl1.SizeMode = TabSizeMode.Fixed;
            metroSetTabControl1.Speed = 100;
            metroSetTabControl1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetTabControl1.StyleManager = null;
            metroSetTabControl1.TabIndex = 0;
            metroSetTabControl1.TabStyle = MetroSet_UI.Enums.TabStyle.Style2;
            metroSetTabControl1.ThemeAuthor = "Narwin";
            metroSetTabControl1.ThemeName = "MetroLite";
            metroSetTabControl1.UnselectedTextColor = Color.Gray;
            metroSetTabControl1.UseAnimation = false;
            metroSetTabControl1.SelectedIndexChanged += metroSetTabControl1_SelectedIndexChanged;
            // 
            // tabPageAgents
            // 
            tabPageAgents.BackColor = Color.White;
            tabPageAgents.Controls.Add(dtgAgents);
            tabPageAgents.Controls.Add(txtSearchAgent);
            tabPageAgents.Controls.Add(lblSearchAgent);
            tabPageAgents.Location = new Point(4, 42);
            tabPageAgents.Name = "tabPageAgents";
            tabPageAgents.Padding = new Padding(3);
            tabPageAgents.Size = new Size(768, 497);
            tabPageAgents.TabIndex = 1;
            tabPageAgents.Text = "Agents";
            // 
            // dtgAgents
            // 
            dtgAgents.AllowUserToAddRows = false;
            dtgAgents.AllowUserToDeleteRows = false;
            dtgAgents.AllowUserToResizeColumns = false;
            dtgAgents.AllowUserToResizeRows = false;
            dtgAgents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgAgents.Dock = DockStyle.Top;
            dtgAgents.Location = new Point(3, 56);
            dtgAgents.Name = "dtgAgents";
            dtgAgents.ReadOnly = true;
            dtgAgents.RowHeadersVisible = false;
            dtgAgents.RowTemplate.Height = 25;
            dtgAgents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgAgents.Size = new Size(762, 435);
            dtgAgents.TabIndex = 2;
            // 
            // txtSearchAgent
            // 
            txtSearchAgent.AutoCompleteCustomSource = null;
            txtSearchAgent.AutoCompleteMode = AutoCompleteMode.None;
            txtSearchAgent.AutoCompleteSource = AutoCompleteSource.None;
            txtSearchAgent.BorderColor = Color.FromArgb(155, 155, 155);
            txtSearchAgent.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtSearchAgent.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtSearchAgent.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtSearchAgent.Dock = DockStyle.Top;
            txtSearchAgent.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtSearchAgent.HoverColor = Color.FromArgb(102, 102, 102);
            txtSearchAgent.Image = null;
            txtSearchAgent.IsDerivedStyle = true;
            txtSearchAgent.Lines = null;
            txtSearchAgent.Location = new Point(3, 26);
            txtSearchAgent.MaxLength = 32767;
            txtSearchAgent.Multiline = false;
            txtSearchAgent.Name = "txtSearchAgent";
            txtSearchAgent.ReadOnly = false;
            txtSearchAgent.Size = new Size(762, 30);
            txtSearchAgent.Style = MetroSet_UI.Enums.Style.Light;
            txtSearchAgent.StyleManager = null;
            txtSearchAgent.TabIndex = 1;
            txtSearchAgent.TextAlign = HorizontalAlignment.Left;
            txtSearchAgent.ThemeAuthor = "Narwin";
            txtSearchAgent.ThemeName = "MetroLite";
            txtSearchAgent.UseSystemPasswordChar = false;
            txtSearchAgent.WatermarkText = "";
            // 
            // lblSearchAgent
            // 
            lblSearchAgent.Dock = DockStyle.Top;
            lblSearchAgent.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblSearchAgent.IsDerivedStyle = true;
            lblSearchAgent.Location = new Point(3, 3);
            lblSearchAgent.Name = "lblSearchAgent";
            lblSearchAgent.Size = new Size(762, 23);
            lblSearchAgent.Style = MetroSet_UI.Enums.Style.Light;
            lblSearchAgent.StyleManager = null;
            lblSearchAgent.TabIndex = 0;
            lblSearchAgent.Text = "Search Agents";
            lblSearchAgent.ThemeAuthor = "Narwin";
            lblSearchAgent.ThemeName = "MetroLite";
            // 
            // tabPageAccounts
            // 
            tabPageAccounts.BackColor = Color.White;
            tabPageAccounts.Controls.Add(panel2);
            tabPageAccounts.Location = new Point(4, 42);
            tabPageAccounts.Name = "tabPageAccounts";
            tabPageAccounts.Padding = new Padding(3);
            tabPageAccounts.Size = new Size(768, 497);
            tabPageAccounts.TabIndex = 0;
            tabPageAccounts.Text = "Accounts";
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridViewAccounts);
            panel2.Controls.Add(txtSearchAccount);
            panel2.Controls.Add(lblSearchAccount);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(762, 491);
            panel2.TabIndex = 3;
            // 
            // dataGridViewAccounts
            // 
            dataGridViewAccounts.AllowUserToAddRows = false;
            dataGridViewAccounts.AllowUserToDeleteRows = false;
            dataGridViewAccounts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewAccounts.Dock = DockStyle.Top;
            dataGridViewAccounts.Location = new Point(0, 53);
            dataGridViewAccounts.MultiSelect = false;
            dataGridViewAccounts.Name = "dataGridViewAccounts";
            dataGridViewAccounts.ReadOnly = true;
            dataGridViewAccounts.RowHeadersVisible = false;
            dataGridViewAccounts.RowTemplate.Height = 25;
            dataGridViewAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewAccounts.Size = new Size(762, 382);
            dataGridViewAccounts.TabIndex = 0;
            // 
            // txtSearchAccount
            // 
            txtSearchAccount.AutoCompleteCustomSource = null;
            txtSearchAccount.AutoCompleteMode = AutoCompleteMode.None;
            txtSearchAccount.AutoCompleteSource = AutoCompleteSource.None;
            txtSearchAccount.BorderColor = Color.FromArgb(155, 155, 155);
            txtSearchAccount.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtSearchAccount.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtSearchAccount.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtSearchAccount.Dock = DockStyle.Top;
            txtSearchAccount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtSearchAccount.HoverColor = Color.FromArgb(102, 102, 102);
            txtSearchAccount.Image = null;
            txtSearchAccount.IsDerivedStyle = true;
            txtSearchAccount.Lines = null;
            txtSearchAccount.Location = new Point(0, 23);
            txtSearchAccount.MaxLength = 32767;
            txtSearchAccount.Multiline = false;
            txtSearchAccount.Name = "txtSearchAccount";
            txtSearchAccount.ReadOnly = false;
            txtSearchAccount.Size = new Size(762, 30);
            txtSearchAccount.Style = MetroSet_UI.Enums.Style.Light;
            txtSearchAccount.StyleManager = null;
            txtSearchAccount.TabIndex = 2;
            txtSearchAccount.TextAlign = HorizontalAlignment.Left;
            txtSearchAccount.ThemeAuthor = "Narwin";
            txtSearchAccount.ThemeName = "MetroLite";
            txtSearchAccount.UseSystemPasswordChar = false;
            txtSearchAccount.WatermarkText = "";
            txtSearchAccount.TextChanged += txtSearchAccount_TextChanged;
            // 
            // lblSearchAccount
            // 
            lblSearchAccount.Dock = DockStyle.Top;
            lblSearchAccount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblSearchAccount.IsDerivedStyle = true;
            lblSearchAccount.Location = new Point(0, 0);
            lblSearchAccount.Name = "lblSearchAccount";
            lblSearchAccount.Size = new Size(762, 23);
            lblSearchAccount.Style = MetroSet_UI.Enums.Style.Light;
            lblSearchAccount.StyleManager = null;
            lblSearchAccount.TabIndex = 1;
            lblSearchAccount.Text = "Search Account:";
            lblSearchAccount.ThemeAuthor = "Narwin";
            lblSearchAccount.ThemeName = "MetroLite";
            // 
            // tabPageItems
            // 
            tabPageItems.BackColor = Color.White;
            tabPageItems.Controls.Add(dtgItems);
            tabPageItems.Controls.Add(txtSearchItems);
            tabPageItems.Controls.Add(lblSearchItems);
            tabPageItems.Location = new Point(4, 42);
            tabPageItems.Name = "tabPageItems";
            tabPageItems.Padding = new Padding(3);
            tabPageItems.Size = new Size(768, 497);
            tabPageItems.TabIndex = 2;
            tabPageItems.Text = "Items";
            // 
            // dtgItems
            // 
            dtgItems.AllowUserToAddRows = false;
            dtgItems.AllowUserToDeleteRows = false;
            dtgItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgItems.Dock = DockStyle.Top;
            dtgItems.Location = new Point(3, 56);
            dtgItems.Name = "dtgItems";
            dtgItems.ReadOnly = true;
            dtgItems.RowHeadersVisible = false;
            dtgItems.RowTemplate.Height = 25;
            dtgItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgItems.Size = new Size(762, 413);
            dtgItems.TabIndex = 3;
            // 
            // txtSearchItems
            // 
            txtSearchItems.AutoCompleteCustomSource = null;
            txtSearchItems.AutoCompleteMode = AutoCompleteMode.None;
            txtSearchItems.AutoCompleteSource = AutoCompleteSource.None;
            txtSearchItems.BorderColor = Color.FromArgb(155, 155, 155);
            txtSearchItems.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtSearchItems.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtSearchItems.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtSearchItems.Dock = DockStyle.Top;
            txtSearchItems.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtSearchItems.HoverColor = Color.FromArgb(102, 102, 102);
            txtSearchItems.Image = null;
            txtSearchItems.IsDerivedStyle = true;
            txtSearchItems.Lines = null;
            txtSearchItems.Location = new Point(3, 26);
            txtSearchItems.MaxLength = 32767;
            txtSearchItems.Multiline = false;
            txtSearchItems.Name = "txtSearchItems";
            txtSearchItems.ReadOnly = false;
            txtSearchItems.Size = new Size(762, 30);
            txtSearchItems.Style = MetroSet_UI.Enums.Style.Light;
            txtSearchItems.StyleManager = null;
            txtSearchItems.TabIndex = 2;
            txtSearchItems.TextAlign = HorizontalAlignment.Left;
            txtSearchItems.ThemeAuthor = "Narwin";
            txtSearchItems.ThemeName = "MetroLite";
            txtSearchItems.UseSystemPasswordChar = false;
            txtSearchItems.WatermarkText = "";
            txtSearchItems.TextChanged += txtSearchItems_TextChanged;
            // 
            // lblSearchItems
            // 
            lblSearchItems.Dock = DockStyle.Top;
            lblSearchItems.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblSearchItems.IsDerivedStyle = true;
            lblSearchItems.Location = new Point(3, 3);
            lblSearchItems.Name = "lblSearchItems";
            lblSearchItems.Size = new Size(762, 23);
            lblSearchItems.Style = MetroSet_UI.Enums.Style.Light;
            lblSearchItems.StyleManager = null;
            lblSearchItems.TabIndex = 4;
            lblSearchItems.Text = "Search Items";
            lblSearchItems.ThemeAuthor = "Narwin";
            lblSearchItems.ThemeName = "MetroLite";
            // 
            // tabPageReports
            // 
            tabPageReports.BackColor = Color.White;
            tabPageReports.Controls.Add(btnItemSalesReport);
            tabPageReports.Controls.Add(btnProfitPerMonth);
            tabPageReports.Location = new Point(4, 42);
            tabPageReports.Name = "tabPageReports";
            tabPageReports.Padding = new Padding(3);
            tabPageReports.Size = new Size(768, 497);
            tabPageReports.TabIndex = 5;
            tabPageReports.Text = "Reports";
            // 
            // btnItemSalesReport
            // 
            btnItemSalesReport.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnItemSalesReport.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnItemSalesReport.DisabledForeColor = Color.Gray;
            btnItemSalesReport.Dock = DockStyle.Top;
            btnItemSalesReport.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnItemSalesReport.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnItemSalesReport.HoverColor = Color.FromArgb(95, 207, 255);
            btnItemSalesReport.HoverTextColor = Color.White;
            btnItemSalesReport.IsDerivedStyle = true;
            btnItemSalesReport.Location = new Point(3, 77);
            btnItemSalesReport.Name = "btnItemSalesReport";
            btnItemSalesReport.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnItemSalesReport.NormalColor = Color.FromArgb(65, 177, 225);
            btnItemSalesReport.NormalTextColor = Color.White;
            btnItemSalesReport.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnItemSalesReport.PressColor = Color.FromArgb(35, 147, 195);
            btnItemSalesReport.PressTextColor = Color.White;
            btnItemSalesReport.Size = new Size(762, 74);
            btnItemSalesReport.Style = MetroSet_UI.Enums.Style.Light;
            btnItemSalesReport.StyleManager = null;
            btnItemSalesReport.TabIndex = 1;
            btnItemSalesReport.Text = "Item sales";
            btnItemSalesReport.ThemeAuthor = "Narwin";
            btnItemSalesReport.ThemeName = "MetroLite";
            btnItemSalesReport.Click += btnItemSalesReport_Click;
            // 
            // btnProfitPerMonth
            // 
            btnProfitPerMonth.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnProfitPerMonth.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnProfitPerMonth.DisabledForeColor = Color.Gray;
            btnProfitPerMonth.Dock = DockStyle.Top;
            btnProfitPerMonth.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnProfitPerMonth.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnProfitPerMonth.HoverColor = Color.FromArgb(95, 207, 255);
            btnProfitPerMonth.HoverTextColor = Color.White;
            btnProfitPerMonth.IsDerivedStyle = true;
            btnProfitPerMonth.Location = new Point(3, 3);
            btnProfitPerMonth.Name = "btnProfitPerMonth";
            btnProfitPerMonth.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnProfitPerMonth.NormalColor = Color.FromArgb(65, 177, 225);
            btnProfitPerMonth.NormalTextColor = Color.White;
            btnProfitPerMonth.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnProfitPerMonth.PressColor = Color.FromArgb(35, 147, 195);
            btnProfitPerMonth.PressTextColor = Color.White;
            btnProfitPerMonth.Size = new Size(762, 74);
            btnProfitPerMonth.Style = MetroSet_UI.Enums.Style.Light;
            btnProfitPerMonth.StyleManager = null;
            btnProfitPerMonth.TabIndex = 0;
            btnProfitPerMonth.Text = "Profit Per Month";
            btnProfitPerMonth.ThemeAuthor = "Narwin";
            btnProfitPerMonth.ThemeName = "MetroLite";
            btnProfitPerMonth.Click += btnProfitPerMonth_Click;
            // 
            // btnImportToExcel
            // 
            btnImportToExcel.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnImportToExcel.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnImportToExcel.DisabledForeColor = Color.Gray;
            btnImportToExcel.Dock = DockStyle.Left;
            btnImportToExcel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnImportToExcel.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnImportToExcel.HoverColor = Color.FromArgb(95, 207, 255);
            btnImportToExcel.HoverTextColor = Color.White;
            btnImportToExcel.IsDerivedStyle = true;
            btnImportToExcel.Location = new Point(248, 0);
            btnImportToExcel.Name = "btnImportToExcel";
            btnImportToExcel.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnImportToExcel.NormalColor = Color.FromArgb(65, 177, 225);
            btnImportToExcel.NormalTextColor = Color.White;
            btnImportToExcel.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnImportToExcel.PressColor = Color.FromArgb(35, 147, 195);
            btnImportToExcel.PressTextColor = Color.White;
            btnImportToExcel.Size = new Size(135, 65);
            btnImportToExcel.Style = MetroSet_UI.Enums.Style.Light;
            btnImportToExcel.StyleManager = null;
            btnImportToExcel.TabIndex = 1;
            btnImportToExcel.Text = "Import via excel";
            btnImportToExcel.ThemeAuthor = "Narwin";
            btnImportToExcel.ThemeName = "MetroLite";
            btnImportToExcel.Visible = false;
            btnImportToExcel.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnEdit.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnEdit.DisabledForeColor = Color.Gray;
            btnEdit.Dock = DockStyle.Left;
            btnEdit.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnEdit.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnEdit.HoverColor = Color.FromArgb(95, 207, 255);
            btnEdit.HoverTextColor = Color.White;
            btnEdit.IsDerivedStyle = true;
            btnEdit.Location = new Point(0, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnEdit.NormalColor = Color.FromArgb(65, 177, 225);
            btnEdit.NormalTextColor = Color.White;
            btnEdit.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnEdit.PressColor = Color.FromArgb(35, 147, 195);
            btnEdit.PressTextColor = Color.White;
            btnEdit.Size = new Size(123, 65);
            btnEdit.Style = MetroSet_UI.Enums.Style.Light;
            btnEdit.StyleManager = null;
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Edit";
            btnEdit.ThemeAuthor = "Narwin";
            btnEdit.ThemeName = "MetroLite";
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnDelete.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnDelete.DisabledForeColor = Color.Gray;
            btnDelete.Dock = DockStyle.Left;
            btnDelete.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnDelete.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnDelete.HoverColor = Color.FromArgb(95, 207, 255);
            btnDelete.HoverTextColor = Color.White;
            btnDelete.IsDerivedStyle = true;
            btnDelete.Location = new Point(123, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnDelete.NormalColor = Color.FromArgb(65, 177, 225);
            btnDelete.NormalTextColor = Color.White;
            btnDelete.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnDelete.PressColor = Color.FromArgb(35, 147, 195);
            btnDelete.PressTextColor = Color.White;
            btnDelete.Size = new Size(125, 65);
            btnDelete.Style = MetroSet_UI.Enums.Style.Light;
            btnDelete.StyleManager = null;
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            btnDelete.ThemeAuthor = "Narwin";
            btnDelete.ThemeName = "MetroLite";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnRefresh.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnRefresh.DisabledForeColor = Color.Gray;
            btnRefresh.Dock = DockStyle.Left;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnRefresh.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnRefresh.HoverColor = Color.FromArgb(95, 207, 255);
            btnRefresh.HoverTextColor = Color.White;
            btnRefresh.IsDerivedStyle = true;
            btnRefresh.Location = new Point(383, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnRefresh.NormalColor = Color.FromArgb(65, 177, 225);
            btnRefresh.NormalTextColor = Color.White;
            btnRefresh.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnRefresh.PressColor = Color.FromArgb(35, 147, 195);
            btnRefresh.PressTextColor = Color.White;
            btnRefresh.Size = new Size(151, 65);
            btnRefresh.Style = MetroSet_UI.Enums.Style.Light;
            btnRefresh.StyleManager = null;
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Refresh";
            btnRefresh.ThemeAuthor = "Narwin";
            btnRefresh.ThemeName = "MetroLite";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnAdd);
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(btnImportToExcel);
            panel1.Controls.Add(btnDelete);
            panel1.Controls.Add(btnEdit);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(12, 613);
            panel1.Name = "panel1";
            panel1.Size = new Size(776, 65);
            panel1.TabIndex = 5;
            // 
            // btnAdd
            // 
            btnAdd.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnAdd.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnAdd.DisabledForeColor = Color.Gray;
            btnAdd.Dock = DockStyle.Left;
            btnAdd.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnAdd.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnAdd.HoverColor = Color.FromArgb(95, 207, 255);
            btnAdd.HoverTextColor = Color.White;
            btnAdd.IsDerivedStyle = true;
            btnAdd.Location = new Point(534, 0);
            btnAdd.Name = "btnAdd";
            btnAdd.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnAdd.NormalColor = Color.FromArgb(65, 177, 225);
            btnAdd.NormalTextColor = Color.White;
            btnAdd.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnAdd.PressColor = Color.FromArgb(35, 147, 195);
            btnAdd.PressTextColor = Color.White;
            btnAdd.Size = new Size(135, 65);
            btnAdd.Style = MetroSet_UI.Enums.Style.Light;
            btnAdd.StyleManager = null;
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Add";
            btnAdd.ThemeAuthor = "Narwin";
            btnAdd.ThemeName = "MetroLite";
            btnAdd.Click += btnAdd_Click_1;
            // 
            // MaintenanceForm
            // 
            ClientSize = new Size(800, 699);
            Controls.Add(panel1);
            Controls.Add(metroSetTabControl1);
            Name = "MaintenanceForm";
            Text = "Autotech POS System - Maintenance";
            WindowState = FormWindowState.Maximized;
            metroSetTabControl1.ResumeLayout(false);
            tabPageAgents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dtgAgents).EndInit();
            tabPageAccounts.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewAccounts).EndInit();
            tabPageItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dtgItems).EndInit();
            tabPageReports.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private MetroSetTabControl metroSetTabControl1;
        private TabPage tabPageAccounts;
        private TabPage tabPageAgents;
        private TabPage tabPageItems;
        private TabPage tabPageReports;
        private MetroSetButton btnImportToExcel;
        private MetroSetButton btnEdit;
        private MetroSetButton btnDelete;
        private MetroSetButton btnRefresh;
        private Panel panel1;
        private DataGridView dataGridViewAccounts;
        private MetroSetTextBox txtSearchAccount;
        private MetroSetLabel lblSearchAccount;
        private Panel panel2;
        private DataGridView dtgAgents;
        private MetroSetTextBox txtSearchAgent;
        private MetroSetLabel lblSearchAgent;
        private DataGridView dtgItems;
        private MetroSetTextBox txtSearchItems;
        private MetroSetButton btnAdd;
        private MetroSetLabel lblSearchItems;
        private MetroSetButton btnItemSalesReport;
        private MetroSetButton btnProfitPerMonth;
    }
}

    #endregion

