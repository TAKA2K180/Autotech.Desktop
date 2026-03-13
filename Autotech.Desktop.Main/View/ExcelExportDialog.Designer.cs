namespace Autotech.Desktop.Main.View
{
    partial class ExcelExportDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            btnInvoices = new MetroSet_UI.Controls.MetroSetButton();
            btnPurchasedItems = new MetroSet_UI.Controls.MetroSetButton();
            metroSetLabel1 = new MetroSet_UI.Controls.MetroSetLabel();
            btnClose = new MetroSet_UI.Controls.MetroSetDefaultButton();
            SuspendLayout();
            // 
            // btnInvoices
            // 
            btnInvoices.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnInvoices.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnInvoices.DisabledForeColor = Color.Gray;
            btnInvoices.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnInvoices.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnInvoices.HoverColor = Color.FromArgb(95, 207, 255);
            btnInvoices.HoverTextColor = Color.White;
            btnInvoices.IsDerivedStyle = true;
            btnInvoices.Location = new Point(29, 129);
            btnInvoices.Name = "btnInvoices";
            btnInvoices.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnInvoices.NormalColor = Color.FromArgb(65, 177, 225);
            btnInvoices.NormalTextColor = Color.White;
            btnInvoices.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnInvoices.PressColor = Color.FromArgb(35, 147, 195);
            btnInvoices.PressTextColor = Color.White;
            btnInvoices.Size = new Size(142, 52);
            btnInvoices.Style = MetroSet_UI.Enums.Style.Light;
            btnInvoices.StyleManager = null;
            btnInvoices.TabIndex = 0;
            btnInvoices.Text = "Invoices";
            btnInvoices.ThemeAuthor = "Narwin";
            btnInvoices.ThemeName = "MetroLite";
            // 
            // btnPurchasedItems
            // 
            btnPurchasedItems.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnPurchasedItems.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnPurchasedItems.DisabledForeColor = Color.Gray;
            btnPurchasedItems.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnPurchasedItems.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnPurchasedItems.HoverColor = Color.FromArgb(95, 207, 255);
            btnPurchasedItems.HoverTextColor = Color.White;
            btnPurchasedItems.IsDerivedStyle = true;
            btnPurchasedItems.Location = new Point(200, 129);
            btnPurchasedItems.Name = "btnPurchasedItems";
            btnPurchasedItems.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnPurchasedItems.NormalColor = Color.FromArgb(65, 177, 225);
            btnPurchasedItems.NormalTextColor = Color.White;
            btnPurchasedItems.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnPurchasedItems.PressColor = Color.FromArgb(35, 147, 195);
            btnPurchasedItems.PressTextColor = Color.White;
            btnPurchasedItems.Size = new Size(142, 52);
            btnPurchasedItems.Style = MetroSet_UI.Enums.Style.Light;
            btnPurchasedItems.StyleManager = null;
            btnPurchasedItems.TabIndex = 1;
            btnPurchasedItems.Text = "Purchased items";
            btnPurchasedItems.ThemeAuthor = "Narwin";
            btnPurchasedItems.ThemeName = "MetroLite";
            // 
            // metroSetLabel1
            // 
            metroSetLabel1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel1.IsDerivedStyle = true;
            metroSetLabel1.Location = new Point(29, 88);
            metroSetLabel1.Name = "metroSetLabel1";
            metroSetLabel1.Size = new Size(196, 23);
            metroSetLabel1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel1.StyleManager = null;
            metroSetLabel1.TabIndex = 2;
            metroSetLabel1.Text = "Excel bulk export selection";
            metroSetLabel1.ThemeAuthor = "Narwin";
            metroSetLabel1.ThemeName = "MetroLite";
            // 
            // btnClose
            // 
            btnClose.DisabledBackColor = Color.FromArgb(204, 204, 204);
            btnClose.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            btnClose.DisabledForeColor = Color.FromArgb(136, 136, 136);
            btnClose.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnClose.HoverBorderColor = Color.FromArgb(102, 102, 102);
            btnClose.HoverColor = Color.FromArgb(102, 102, 102);
            btnClose.HoverTextColor = Color.White;
            btnClose.IsDerivedStyle = true;
            btnClose.Location = new Point(349, 6);
            btnClose.Name = "btnClose";
            btnClose.NormalBorderColor = Color.FromArgb(204, 204, 204);
            btnClose.NormalColor = Color.FromArgb(238, 238, 238);
            btnClose.NormalTextColor = Color.Black;
            btnClose.PressBorderColor = Color.FromArgb(51, 51, 51);
            btnClose.PressColor = Color.FromArgb(51, 51, 51);
            btnClose.PressTextColor = Color.White;
            btnClose.Size = new Size(42, 33);
            btnClose.Style = MetroSet_UI.Enums.Style.Light;
            btnClose.StyleManager = null;
            btnClose.TabIndex = 3;
            btnClose.Text = "X";
            btnClose.ThemeAuthor = "Narwin";
            btnClose.ThemeName = "MetroLite";
            btnClose.Click += btnClose_Click;
            // 
            // ExcelExportDialog
            // 
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(396, 228);
            Controls.Add(btnClose);
            Controls.Add(metroSetLabel1);
            Controls.Add(btnPurchasedItems);
            Controls.Add(btnInvoices);
            Name = "ExcelExportDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Excel Export Selection";
            ResumeLayout(false);
        }

        #endregion

        private MetroSet_UI.Controls.MetroSetButton btnInvoices;
        private MetroSet_UI.Controls.MetroSetButton btnPurchasedItems;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel1;
        private MetroSet_UI.Controls.MetroSetDefaultButton btnClose;
    }
}