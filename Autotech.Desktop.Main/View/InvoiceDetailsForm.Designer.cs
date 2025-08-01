﻿namespace Autotech.Desktop.Main.View
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
            btnCloseInvoiceDetails = new MetroSet_UI.Controls.MetroSetButton();
            btnConfirmPayment = new MetroSet_UI.Controls.MetroSetButton();
            dvgPaymentHistory = new DataGridView();
            metroSetLabel1 = new MetroSet_UI.Controls.MetroSetLabel();
            btnAddPayment = new MetroSet_UI.Controls.MetroSetButton();
            lblStatus = new MetroSet_UI.Controls.MetroSetLabel();
            btnCancelInvoice = new MetroSet_UI.Controls.MetroSetButton();
            lblOrigin = new MetroSet_UI.Controls.MetroSetLabel();
            ((System.ComponentModel.ISupportInitialize)dataGridViewInvoiceDetails).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dvgPaymentHistory).BeginInit();
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
            txtSubtotal.Location = new Point(477, 473);
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
            txtTax.Location = new Point(477, 513);
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
            txtDiscount.Location = new Point(477, 553);
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
            txtTotal.Location = new Point(477, 593);
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
            dataGridViewInvoiceDetails.Size = new Size(647, 300);
            dataGridViewInvoiceDetails.TabIndex = 3;
            // 
            // metroSetLabel2
            // 
            metroSetLabel2.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel2.IsDerivedStyle = true;
            metroSetLabel2.Location = new Point(409, 480);
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
            metroSetLabel3.Location = new Point(439, 520);
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
            metroSetLabel4.Location = new Point(402, 553);
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
            metroSetLabel5.Location = new Point(426, 593);
            metroSetLabel5.Name = "metroSetLabel5";
            metroSetLabel5.Size = new Size(45, 23);
            metroSetLabel5.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel5.StyleManager = null;
            metroSetLabel5.TabIndex = 13;
            metroSetLabel5.Text = "Total";
            metroSetLabel5.ThemeAuthor = "Narwin";
            metroSetLabel5.ThemeName = "MetroLite";
            // 
            // btnCloseInvoiceDetails
            // 
            btnCloseInvoiceDetails.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnCloseInvoiceDetails.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnCloseInvoiceDetails.DisabledForeColor = Color.Gray;
            btnCloseInvoiceDetails.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnCloseInvoiceDetails.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnCloseInvoiceDetails.HoverColor = Color.FromArgb(95, 207, 255);
            btnCloseInvoiceDetails.HoverTextColor = Color.White;
            btnCloseInvoiceDetails.IsDerivedStyle = true;
            btnCloseInvoiceDetails.Location = new Point(30, 553);
            btnCloseInvoiceDetails.Name = "btnCloseInvoiceDetails";
            btnCloseInvoiceDetails.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnCloseInvoiceDetails.NormalColor = Color.FromArgb(65, 177, 225);
            btnCloseInvoiceDetails.NormalTextColor = Color.White;
            btnCloseInvoiceDetails.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnCloseInvoiceDetails.PressColor = Color.FromArgb(35, 147, 195);
            btnCloseInvoiceDetails.PressTextColor = Color.White;
            btnCloseInvoiceDetails.Size = new Size(150, 40);
            btnCloseInvoiceDetails.Style = MetroSet_UI.Enums.Style.Light;
            btnCloseInvoiceDetails.StyleManager = null;
            btnCloseInvoiceDetails.TabIndex = 14;
            btnCloseInvoiceDetails.Text = "Close";
            btnCloseInvoiceDetails.ThemeAuthor = "Narwin";
            btnCloseInvoiceDetails.ThemeName = "MetroLite";
            btnCloseInvoiceDetails.Click += btnCloseInvoiceDetail_Click;
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
            btnConfirmPayment.Location = new Point(700, 553);
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
            // dvgPaymentHistory
            // 
            dvgPaymentHistory.AllowUserToAddRows = false;
            dvgPaymentHistory.AllowUserToDeleteRows = false;
            dvgPaymentHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dvgPaymentHistory.Location = new Point(700, 158);
            dvgPaymentHistory.Name = "dvgPaymentHistory";
            dvgPaymentHistory.RowHeadersVisible = false;
            dvgPaymentHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvgPaymentHistory.Size = new Size(647, 300);
            dvgPaymentHistory.TabIndex = 16;
            // 
            // metroSetLabel1
            // 
            metroSetLabel1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel1.IsDerivedStyle = true;
            metroSetLabel1.Location = new Point(700, 130);
            metroSetLabel1.Name = "metroSetLabel1";
            metroSetLabel1.Size = new Size(400, 25);
            metroSetLabel1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel1.StyleManager = null;
            metroSetLabel1.TabIndex = 17;
            metroSetLabel1.Text = "Payment history";
            metroSetLabel1.ThemeAuthor = "Narwin";
            metroSetLabel1.ThemeName = "MetroLite";
            // 
            // btnAddPayment
            // 
            btnAddPayment.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnAddPayment.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnAddPayment.DisabledForeColor = Color.Gray;
            btnAddPayment.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnAddPayment.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnAddPayment.HoverColor = Color.FromArgb(95, 207, 255);
            btnAddPayment.HoverTextColor = Color.White;
            btnAddPayment.IsDerivedStyle = true;
            btnAddPayment.Location = new Point(700, 464);
            btnAddPayment.Name = "btnAddPayment";
            btnAddPayment.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnAddPayment.NormalColor = Color.FromArgb(65, 177, 225);
            btnAddPayment.NormalTextColor = Color.White;
            btnAddPayment.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnAddPayment.PressColor = Color.FromArgb(35, 147, 195);
            btnAddPayment.PressTextColor = Color.White;
            btnAddPayment.Size = new Size(150, 40);
            btnAddPayment.Style = MetroSet_UI.Enums.Style.Light;
            btnAddPayment.StyleManager = null;
            btnAddPayment.TabIndex = 18;
            btnAddPayment.Text = "Add Payment";
            btnAddPayment.ThemeAuthor = "Narwin";
            btnAddPayment.ThemeName = "MetroLite";
            btnAddPayment.Click += btnAddPayment_Click;
            // 
            // lblStatus
            // 
            lblStatus.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatus.IsDerivedStyle = true;
            lblStatus.Location = new Point(477, 130);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(200, 25);
            lblStatus.Style = MetroSet_UI.Enums.Style.Light;
            lblStatus.StyleManager = null;
            lblStatus.TabIndex = 19;
            lblStatus.Text = "Status: ";
            lblStatus.ThemeAuthor = "Narwin";
            lblStatus.ThemeName = "MetroLite";
            // 
            // btnCancelInvoice
            // 
            btnCancelInvoice.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnCancelInvoice.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnCancelInvoice.DisabledForeColor = Color.Gray;
            btnCancelInvoice.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancelInvoice.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnCancelInvoice.HoverColor = Color.FromArgb(95, 207, 255);
            btnCancelInvoice.HoverTextColor = Color.White;
            btnCancelInvoice.IsDerivedStyle = true;
            btnCancelInvoice.Location = new Point(1197, 553);
            btnCancelInvoice.Name = "btnCancelInvoice";
            btnCancelInvoice.NormalBorderColor = Color.FromArgb(64, 0, 0);
            btnCancelInvoice.NormalColor = Color.Maroon;
            btnCancelInvoice.NormalTextColor = Color.White;
            btnCancelInvoice.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnCancelInvoice.PressColor = Color.FromArgb(35, 147, 195);
            btnCancelInvoice.PressTextColor = Color.White;
            btnCancelInvoice.Size = new Size(150, 40);
            btnCancelInvoice.Style = MetroSet_UI.Enums.Style.Light;
            btnCancelInvoice.StyleManager = null;
            btnCancelInvoice.TabIndex = 20;
            btnCancelInvoice.Text = "Cancel Invoice";
            btnCancelInvoice.ThemeAuthor = "Narwin";
            btnCancelInvoice.ThemeName = "MetroLite";
            btnCancelInvoice.Click += btnCancelInvoice_Click;
            // 
            // lblOrigin
            // 
            lblOrigin.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblOrigin.IsDerivedStyle = true;
            lblOrigin.Location = new Point(477, 105);
            lblOrigin.Name = "lblOrigin";
            lblOrigin.Size = new Size(200, 25);
            lblOrigin.Style = MetroSet_UI.Enums.Style.Light;
            lblOrigin.StyleManager = null;
            lblOrigin.TabIndex = 21;
            lblOrigin.Text = "Origin: ";
            lblOrigin.ThemeAuthor = "Narwin";
            lblOrigin.ThemeName = "MetroLite";
            // 
            // InvoiceDetailsForm
            // 
            ClientSize = new Size(1370, 653);
            Controls.Add(lblOrigin);
            Controls.Add(btnCancelInvoice);
            Controls.Add(lblStatus);
            Controls.Add(btnAddPayment);
            Controls.Add(metroSetLabel1);
            Controls.Add(dvgPaymentHistory);
            Controls.Add(btnConfirmPayment);
            Controls.Add(btnCloseInvoiceDetails);
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
            ((System.ComponentModel.ISupportInitialize)dvgPaymentHistory).EndInit();
            ResumeLayout(false);
        }
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel2;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel3;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel4;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel5;
        private MetroSet_UI.Controls.MetroSetButton btnCloseInvoiceDetails;
        private MetroSet_UI.Controls.MetroSetButton btnConfirmPayment;
        private DataGridView dvgPaymentHistory;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel1;
        private MetroSet_UI.Controls.MetroSetButton btnAddPayment;
        private MetroSet_UI.Controls.MetroSetLabel lblStatus;
        private MetroSet_UI.Controls.MetroSetButton btnCancelInvoice;
        private MetroSet_UI.Controls.MetroSetLabel lblOrigin;
    }
}
