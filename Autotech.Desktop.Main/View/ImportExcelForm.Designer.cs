namespace Autotech.Desktop.Main.View
{
    partial class ImportExcelForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Button btnCreateTemplate;
        private Button btnImportExcel;
        private Button btnSubmit;
        private OpenFileDialog openFileDialog;
        private DataGridView dtgPreview;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnCreateTemplate = new Button();
            btnImportExcel = new Button();
            btnSubmit = new Button();
            dtgPreview = new DataGridView();
            openFileDialog = new OpenFileDialog();
            metroSetLabel1 = new MetroSet_UI.Controls.MetroSetLabel();
            btnClose = new MetroSet_UI.Controls.MetroSetButton();
            ((System.ComponentModel.ISupportInitialize)dtgPreview).BeginInit();
            SuspendLayout();
            // 
            // btnCreateTemplate
            // 
            btnCreateTemplate.Location = new Point(15, 117);
            btnCreateTemplate.Name = "btnCreateTemplate";
            btnCreateTemplate.Size = new Size(227, 37);
            btnCreateTemplate.TabIndex = 0;
            btnCreateTemplate.Text = "Create Excel Template";
            btnCreateTemplate.Click += btnCreateTemplate_Click;
            // 
            // btnImportExcel
            // 
            btnImportExcel.Location = new Point(15, 198);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(157, 34);
            btnImportExcel.TabIndex = 1;
            btnImportExcel.Text = "Import Excel File";
            btnImportExcel.Click += btnImportExcel_Click;
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(15, 502);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(157, 34);
            btnSubmit.TabIndex = 2;
            btnSubmit.Text = "Insert to System";
            btnSubmit.Click += btnSubmit_Click;
            // 
            // dtgPreview
            // 
            dtgPreview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgPreview.Location = new Point(15, 238);
            dtgPreview.Name = "dtgPreview";
            dtgPreview.ReadOnly = true;
            dtgPreview.RowHeadersVisible = false;
            dtgPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgPreview.Size = new Size(740, 258);
            dtgPreview.TabIndex = 3;
            // 
            // metroSetLabel1
            // 
            metroSetLabel1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel1.IsDerivedStyle = true;
            metroSetLabel1.Location = new Point(15, 91);
            metroSetLabel1.Name = "metroSetLabel1";
            metroSetLabel1.Size = new Size(371, 23);
            metroSetLabel1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel1.StyleManager = null;
            metroSetLabel1.TabIndex = 4;
            metroSetLabel1.Text = "Download excel template if you don't have";
            metroSetLabel1.ThemeAuthor = "Narwin";
            metroSetLabel1.ThemeName = "MetroLite";
            // 
            // btnClose
            // 
            btnClose.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnClose.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnClose.DisabledForeColor = Color.Gray;
            btnClose.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnClose.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnClose.HoverColor = Color.FromArgb(95, 207, 255);
            btnClose.HoverTextColor = Color.White;
            btnClose.IsDerivedStyle = true;
            btnClose.Location = new Point(685, 8);
            btnClose.Name = "btnClose";
            btnClose.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnClose.NormalColor = Color.FromArgb(65, 177, 225);
            btnClose.NormalTextColor = Color.White;
            btnClose.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnClose.PressColor = Color.FromArgb(35, 147, 195);
            btnClose.PressTextColor = Color.White;
            btnClose.Size = new Size(70, 40);
            btnClose.Style = MetroSet_UI.Enums.Style.Light;
            btnClose.StyleManager = null;
            btnClose.TabIndex = 5;
            btnClose.Text = "Close";
            btnClose.ThemeAuthor = "Narwin";
            btnClose.ThemeName = "MetroLite";
            btnClose.Click += btnClose_Click;
            // 
            // ImportExcelForm
            // 
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(770, 577);
            Controls.Add(btnClose);
            Controls.Add(metroSetLabel1);
            Controls.Add(btnCreateTemplate);
            Controls.Add(btnImportExcel);
            Controls.Add(btnSubmit);
            Controls.Add(dtgPreview);
            Name = "ImportExcelForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Import/Export Excel";
            ((System.ComponentModel.ISupportInitialize)dtgPreview).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel1;
        private MetroSet_UI.Controls.MetroSetButton btnClose;
    }
}