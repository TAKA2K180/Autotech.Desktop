namespace Autotech.Desktop.Main.View
{
    partial class InvoiceDetailsForm
    {
        private MetroSet_UI.Controls.MetroSetLabel lblInvoiceNumber;
        private MetroSet_UI.Controls.MetroSetLabel lblCustomer;
        private MetroSet_UI.Controls.MetroSetLabel lblDate;
        private MetroSet_UI.Controls.MetroSetTextBox txtSubtotal;
        private MetroSet_UI.Controls.MetroSetTextBox txtTax;
        private MetroSet_UI.Controls.MetroSetTextBox txtDiscount;
        private MetroSet_UI.Controls.MetroSetTextBox txtTotal;
        private MetroSet_UI.Controls.MetroSetButton btnPrint;
        private System.Windows.Forms.DataGridView dataGridViewInvoiceDetails;

        private void InitializeComponent()
        {
            lblInvoiceNumber = new MetroSet_UI.Controls.MetroSetLabel();
            lblCustomer = new MetroSet_UI.Controls.MetroSetLabel();
            lblDate = new MetroSet_UI.Controls.MetroSetLabel();
            txtSubtotal = new MetroSet_UI.Controls.MetroSetTextBox();
            txtTax = new MetroSet_UI.Controls.MetroSetTextBox();
            txtDiscount = new MetroSet_UI.Controls.MetroSetTextBox();
            txtTotal = new MetroSet_UI.Controls.MetroSetTextBox();
            btnPrint = new MetroSet_UI.Controls.MetroSetButton();
            dataGridViewInvoiceDetails = new DataGridView();
            metroSetLabel2 = new MetroSet_UI.Controls.MetroSetLabel();
            metroSetLabel3 = new MetroSet_UI.Controls.MetroSetLabel();
            metroSetLabel4 = new MetroSet_UI.Controls.MetroSetLabel();
            metroSetLabel5 = new MetroSet_UI.Controls.MetroSetLabel();
            metroSetButton1 = new MetroSet_UI.Controls.MetroSetButton();
            btnConfirmPayment = new MetroSet_UI.Controls.MetroSetButton();
            ((System.ComponentModel.ISupportInitialize)dataGridViewInvoiceDetails).BeginInit();
            SuspendLayout();
            // 
            // lblInvoiceNumber
            // 
            lblInvoiceNumber.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblInvoiceNumber.IsDerivedStyle = true;
            lblInvoiceNumber.Location = new Point(30, 130);
            lblInvoiceNumber.Name = "lblInvoiceNumber";
            lblInvoiceNumber.Size = new Size(400, 25);
            lblInvoiceNumber.Style = MetroSet_UI.Enums.Style.Light;
            lblInvoiceNumber.StyleManager = null;
            lblInvoiceNumber.TabIndex = 0;
            lblInvoiceNumber.Text = "Invoice #:";
            lblInvoiceNumber.ThemeAuthor = "Narwin";
            lblInvoiceNumber.ThemeName = "MetroLite";
            // 
            // lblCustomer
            // 
            lblCustomer.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblCustomer.IsDerivedStyle = true;
            lblCustomer.Location = new Point(30, 105);
            lblCustomer.Name = "lblCustomer";
            lblCustomer.Size = new Size(400, 25);
            lblCustomer.Style = MetroSet_UI.Enums.Style.Light;
            lblCustomer.StyleManager = null;
            lblCustomer.TabIndex = 1;
            lblCustomer.Text = "Customer:";
            lblCustomer.ThemeAuthor = "Narwin";
            lblCustomer.ThemeName = "MetroLite";
            // 
            // lblDate
            // 
            lblDate.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblDate.IsDerivedStyle = true;
            lblDate.Location = new Point(30, 80);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(400, 25);
            lblDate.Style = MetroSet_UI.Enums.Style.Light;
            lblDate.StyleManager = null;
            lblDate.TabIndex = 2;
            lblDate.Text = "Date:";
            lblDate.ThemeAuthor = "Narwin";
            lblDate.ThemeName = "MetroLite";
            // 
            // txtSubtotal
            // 
            txtSubtotal.AutoCompleteCustomSource = null;
            txtSubtotal.AutoCompleteMode = AutoCompleteMode.None;
            txtSubtotal.AutoCompleteSource = AutoCompleteSource.None;
            txtSubtotal.BorderColor = Color.FromArgb(155, 155, 155);
            txtSubtotal.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtSubtotal.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtSubtotal.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtSubtotal.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtSubtotal.HoverColor = Color.FromArgb(102, 102, 102);
            txtSubtotal.Image = null;
            txtSubtotal.IsDerivedStyle = true;
            txtSubtotal.Lines = null;
            txtSubtotal.Location = new Point(885, 473);
            txtSubtotal.MaxLength = 32767;
            txtSubtotal.Multiline = false;
            txtSubtotal.Name = "txtSubtotal";
            txtSubtotal.ReadOnly = true;
            txtSubtotal.Size = new Size(200, 30);
            txtSubtotal.Style = MetroSet_UI.Enums.Style.Light;
            txtSubtotal.StyleManager = null;
            txtSubtotal.TabIndex = 4;
            txtSubtotal.TextAlign = HorizontalAlignment.Left;
            txtSubtotal.ThemeAuthor = "Narwin";
            txtSubtotal.ThemeName = "MetroLite";
            txtSubtotal.UseSystemPasswordChar = false;
            txtSubtotal.WatermarkText = "Subtotal";
            // 
            // txtTax
            // 
            txtTax.AutoCompleteCustomSource = null;
            txtTax.AutoCompleteMode = AutoCompleteMode.None;
            txtTax.AutoCompleteSource = AutoCompleteSource.None;
            txtTax.BorderColor = Color.FromArgb(155, 155, 155);
            txtTax.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtTax.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtTax.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtTax.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtTax.HoverColor = Color.FromArgb(102, 102, 102);
            txtTax.Image = null;
            txtTax.IsDerivedStyle = true;
            txtTax.Lines = null;
            txtTax.Location = new Point(885, 513);
            txtTax.MaxLength = 32767;
            txtTax.Multiline = false;
            txtTax.Name = "txtTax";
            txtTax.ReadOnly = false;
            txtTax.Size = new Size(200, 30);
            txtTax.Style = MetroSet_UI.Enums.Style.Light;
            txtTax.StyleManager = null;
            txtTax.TabIndex = 5;
            txtTax.TextAlign = HorizontalAlignment.Left;
            txtTax.ThemeAuthor = "Narwin";
            txtTax.ThemeName = "MetroLite";
            txtTax.UseSystemPasswordChar = false;
            txtTax.WatermarkText = "Tax";
            // 
            // txtDiscount
            // 
            txtDiscount.AutoCompleteCustomSource = null;
            txtDiscount.AutoCompleteMode = AutoCompleteMode.None;
            txtDiscount.AutoCompleteSource = AutoCompleteSource.None;
            txtDiscount.BorderColor = Color.FromArgb(155, 155, 155);
            txtDiscount.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtDiscount.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtDiscount.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtDiscount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtDiscount.HoverColor = Color.FromArgb(102, 102, 102);
            txtDiscount.Image = null;
            txtDiscount.IsDerivedStyle = true;
            txtDiscount.Lines = null;
            txtDiscount.Location = new Point(885, 553);
            txtDiscount.MaxLength = 32767;
            txtDiscount.Multiline = false;
            txtDiscount.Name = "txtDiscount";
            txtDiscount.ReadOnly = false;
            txtDiscount.Size = new Size(200, 30);
            txtDiscount.Style = MetroSet_UI.Enums.Style.Light;
            txtDiscount.StyleManager = null;
            txtDiscount.TabIndex = 6;
            txtDiscount.TextAlign = HorizontalAlignment.Left;
            txtDiscount.ThemeAuthor = "Narwin";
            txtDiscount.ThemeName = "MetroLite";
            txtDiscount.UseSystemPasswordChar = false;
            txtDiscount.WatermarkText = "Discount";
            // 
            // txtTotal
            // 
            txtTotal.AutoCompleteCustomSource = null;
            txtTotal.AutoCompleteMode = AutoCompleteMode.None;
            txtTotal.AutoCompleteSource = AutoCompleteSource.None;
            txtTotal.BorderColor = Color.FromArgb(155, 155, 155);
            txtTotal.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtTotal.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtTotal.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtTotal.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtTotal.HoverColor = Color.FromArgb(102, 102, 102);
            txtTotal.Image = null;
            txtTotal.IsDerivedStyle = true;
            txtTotal.Lines = null;
            txtTotal.Location = new Point(885, 593);
            txtTotal.MaxLength = 32767;
            txtTotal.Multiline = false;
            txtTotal.Name = "txtTotal";
            txtTotal.ReadOnly = true;
            txtTotal.Size = new Size(200, 30);
            txtTotal.Style = MetroSet_UI.Enums.Style.Light;
            txtTotal.StyleManager = null;
            txtTotal.TabIndex = 7;
            txtTotal.TextAlign = HorizontalAlignment.Left;
            txtTotal.ThemeAuthor = "Narwin";
            txtTotal.ThemeName = "MetroLite";
            txtTotal.UseSystemPasswordChar = false;
            txtTotal.WatermarkText = "Total";
            // 
            // btnPrint
            // 
            btnPrint.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnPrint.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnPrint.DisabledForeColor = Color.Gray;
            btnPrint.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnPrint.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnPrint.HoverColor = Color.FromArgb(95, 207, 255);
            btnPrint.HoverTextColor = Color.White;
            btnPrint.IsDerivedStyle = true;
            btnPrint.Location = new Point(30, 463);
            btnPrint.Name = "btnPrint";
            btnPrint.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnPrint.NormalColor = Color.FromArgb(65, 177, 225);
            btnPrint.NormalTextColor = Color.White;
            btnPrint.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnPrint.PressColor = Color.FromArgb(35, 147, 195);
            btnPrint.PressTextColor = Color.White;
            btnPrint.Size = new Size(150, 40);
            btnPrint.Style = MetroSet_UI.Enums.Style.Light;
            btnPrint.StyleManager = null;
            btnPrint.TabIndex = 8;
            btnPrint.Text = "Print Invoice";
            btnPrint.ThemeAuthor = "Narwin";
            btnPrint.ThemeName = "MetroLite";
            btnPrint.Click += btnPrint_Click;
            // 
            // dataGridViewInvoiceDetails
            // 
            dataGridViewInvoiceDetails.AllowUserToAddRows = false;
            dataGridViewInvoiceDetails.AllowUserToDeleteRows = false;
            dataGridViewInvoiceDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewInvoiceDetails.Location = new Point(30, 158);
            dataGridViewInvoiceDetails.Name = "dataGridViewInvoiceDetails";
            dataGridViewInvoiceDetails.Size = new Size(1055, 300);
            dataGridViewInvoiceDetails.TabIndex = 3;
            // 
            // metroSetLabel2
            // 
            metroSetLabel2.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel2.IsDerivedStyle = true;
            metroSetLabel2.Location = new Point(817, 480);
            metroSetLabel2.Name = "metroSetLabel2";
            metroSetLabel2.Size = new Size(62, 23);
            metroSetLabel2.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel2.StyleManager = null;
            metroSetLabel2.TabIndex = 10;
            metroSetLabel2.Text = "Subtotal";
            metroSetLabel2.ThemeAuthor = "Narwin";
            metroSetLabel2.ThemeName = "MetroLite";
            // 
            // metroSetLabel3
            // 
            metroSetLabel3.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel3.IsDerivedStyle = true;
            metroSetLabel3.Location = new Point(847, 520);
            metroSetLabel3.Name = "metroSetLabel3";
            metroSetLabel3.Size = new Size(32, 23);
            metroSetLabel3.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel3.StyleManager = null;
            metroSetLabel3.TabIndex = 11;
            metroSetLabel3.Text = "Tax";
            metroSetLabel3.ThemeAuthor = "Narwin";
            metroSetLabel3.ThemeName = "MetroLite";
            // 
            // metroSetLabel4
            // 
            metroSetLabel4.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel4.IsDerivedStyle = true;
            metroSetLabel4.Location = new Point(810, 553);
            metroSetLabel4.Name = "metroSetLabel4";
            metroSetLabel4.Size = new Size(69, 23);
            metroSetLabel4.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel4.StyleManager = null;
            metroSetLabel4.TabIndex = 12;
            metroSetLabel4.Text = "Discount";
            metroSetLabel4.ThemeAuthor = "Narwin";
            metroSetLabel4.ThemeName = "MetroLite";
            // 
            // metroSetLabel5
            // 
            metroSetLabel5.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel5.IsDerivedStyle = true;
            metroSetLabel5.Location = new Point(834, 593);
            metroSetLabel5.Name = "metroSetLabel5";
            metroSetLabel5.Size = new Size(45, 23);
            metroSetLabel5.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel5.StyleManager = null;
            metroSetLabel5.TabIndex = 13;
            metroSetLabel5.Text = "Total";
            metroSetLabel5.ThemeAuthor = "Narwin";
            metroSetLabel5.ThemeName = "MetroLite";
            // 
            // metroSetButton1
            // 
            metroSetButton1.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            metroSetButton1.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            metroSetButton1.DisabledForeColor = Color.Gray;
            metroSetButton1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetButton1.HoverBorderColor = Color.FromArgb(95, 207, 255);
            metroSetButton1.HoverColor = Color.FromArgb(95, 207, 255);
            metroSetButton1.HoverTextColor = Color.White;
            metroSetButton1.IsDerivedStyle = true;
            metroSetButton1.Location = new Point(30, 553);
            metroSetButton1.Name = "metroSetButton1";
            metroSetButton1.NormalBorderColor = Color.FromArgb(65, 177, 225);
            metroSetButton1.NormalColor = Color.FromArgb(65, 177, 225);
            metroSetButton1.NormalTextColor = Color.White;
            metroSetButton1.PressBorderColor = Color.FromArgb(35, 147, 195);
            metroSetButton1.PressColor = Color.FromArgb(35, 147, 195);
            metroSetButton1.PressTextColor = Color.White;
            metroSetButton1.Size = new Size(150, 40);
            metroSetButton1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetButton1.StyleManager = null;
            metroSetButton1.TabIndex = 14;
            metroSetButton1.Text = "Close";
            metroSetButton1.ThemeAuthor = "Narwin";
            metroSetButton1.ThemeName = "MetroLite";
            metroSetButton1.Click += metroSetButton1_Click;
            // 
            // btnConfirmPayment
            // 
            btnConfirmPayment.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnConfirmPayment.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnConfirmPayment.DisabledForeColor = Color.Gray;
            btnConfirmPayment.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnConfirmPayment.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnConfirmPayment.HoverColor = Color.FromArgb(95, 207, 255);
            btnConfirmPayment.HoverTextColor = Color.White;
            btnConfirmPayment.IsDerivedStyle = true;
            btnConfirmPayment.Location = new Point(200, 464);
            btnConfirmPayment.Name = "btnConfirmPayment";
            btnConfirmPayment.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnConfirmPayment.NormalColor = Color.FromArgb(65, 177, 225);
            btnConfirmPayment.NormalTextColor = Color.White;
            btnConfirmPayment.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnConfirmPayment.PressColor = Color.FromArgb(35, 147, 195);
            btnConfirmPayment.PressTextColor = Color.White;
            btnConfirmPayment.Size = new Size(150, 40);
            btnConfirmPayment.Style = MetroSet_UI.Enums.Style.Light;
            btnConfirmPayment.StyleManager = null;
            btnConfirmPayment.TabIndex = 15;
            btnConfirmPayment.Text = "Confirm Payment";
            btnConfirmPayment.ThemeAuthor = "Narwin";
            btnConfirmPayment.ThemeName = "MetroLite";
            btnConfirmPayment.Click += btnConfirmPayment_Click;
            // 
            // InvoiceDetailsForm
            // 
            ClientSize = new Size(1109, 653);
            Controls.Add(btnConfirmPayment);
            Controls.Add(metroSetButton1);
            Controls.Add(metroSetLabel5);
            Controls.Add(metroSetLabel4);
            Controls.Add(metroSetLabel3);
            Controls.Add(metroSetLabel2);
            Controls.Add(lblInvoiceNumber);
            Controls.Add(lblCustomer);
            Controls.Add(lblDate);
            Controls.Add(dataGridViewInvoiceDetails);
            Controls.Add(txtSubtotal);
            Controls.Add(txtTax);
            Controls.Add(txtDiscount);
            Controls.Add(txtTotal);
            Controls.Add(btnPrint);
            Name = "InvoiceDetailsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Invoice Details";
            ((System.ComponentModel.ISupportInitialize)dataGridViewInvoiceDetails).EndInit();
            ResumeLayout(false);
        }
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel2;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel3;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel4;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel5;
        private MetroSet_UI.Controls.MetroSetButton metroSetButton1;
        private MetroSet_UI.Controls.MetroSetButton btnConfirmPayment;
    }
}
