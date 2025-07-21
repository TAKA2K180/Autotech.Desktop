namespace Autotech.Desktop.Main.View
{
    partial class ItemSalesReportForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridView dtgItemSales;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnFilter = new Button();
            btnExport = new Button();
            dtgItemSales = new DataGridView();
            lblFrom = new Label();
            lblTo = new Label();
            btnClose = new Button();
            dtmFrom = new DateTimePicker();
            dtmTo = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)dtgItemSales).BeginInit();
            SuspendLayout();
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(372, 97);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(157, 35);
            btnFilter.TabIndex = 4;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = true;
            btnFilter.Click += btnFilter_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(535, 99);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(184, 33);
            btnExport.TabIndex = 5;
            btnExport.Text = "Export to Excel";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // dtgItemSales
            // 
            dtgItemSales.AllowUserToAddRows = false;
            dtgItemSales.AllowUserToDeleteRows = false;
            dtgItemSales.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dtgItemSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgItemSales.Location = new Point(20, 138);
            dtgItemSales.Name = "dtgItemSales";
            dtgItemSales.ReadOnly = true;
            dtgItemSales.RowHeadersVisible = false;
            dtgItemSales.Size = new Size(740, 418);
            dtgItemSales.TabIndex = 5;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Location = new Point(20, 102);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(56, 22);
            lblFrom.TabIndex = 0;
            lblFrom.Text = "From:";
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Location = new Point(202, 102);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(37, 22);
            lblTo.TabIndex = 1;
            lblTo.Text = "To:";
            // 
            // btnClose
            // 
            btnClose.Location = new Point(669, 6);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 33);
            btnClose.TabIndex = 6;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // dtmFrom
            // 
            dtmFrom.Format = DateTimePickerFormat.Short;
            dtmFrom.Location = new Point(82, 102);
            dtmFrom.Name = "dtmFrom";
            dtmFrom.Size = new Size(106, 27);
            dtmFrom.TabIndex = 7;
            // 
            // dtmTo
            // 
            dtmTo.Format = DateTimePickerFormat.Short;
            dtmTo.Location = new Point(245, 102);
            dtmTo.Name = "dtmTo";
            dtmTo.Size = new Size(106, 27);
            dtmTo.TabIndex = 8;
            // 
            // ItemSalesReportForm
            // 
            ClientSize = new Size(784, 577);
            Controls.Add(dtmTo);
            Controls.Add(dtmFrom);
            Controls.Add(btnClose);
            Controls.Add(lblFrom);
            Controls.Add(lblTo);
            Controls.Add(btnFilter);
            Controls.Add(btnExport);
            Controls.Add(dtgItemSales);
            Name = "ItemSalesReportForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Item Sales Report";
            ((System.ComponentModel.ISupportInitialize)dtgItemSales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private Button btnClose;
        private DateTimePicker dtmFrom;
        private DateTimePicker dtmTo;
    }
}
