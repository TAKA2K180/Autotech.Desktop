namespace Autotech.Desktop.Main.View
{
    partial class AddPaymentForm
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
            metroSetLabel1 = new MetroSet_UI.Controls.MetroSetLabel();
            txtAmount = new MetroSet_UI.Controls.MetroSetTextBox();
            metroSetLabel2 = new MetroSet_UI.Controls.MetroSetLabel();
            comboPaymentMethod = new MetroSet_UI.Controls.MetroSetComboBox();
            btnOK = new MetroSet_UI.Controls.MetroSetButton();
            metroSetControlBox1 = new MetroSet_UI.Controls.MetroSetControlBox();
            SuspendLayout();
            // 
            // metroSetLabel1
            // 
            metroSetLabel1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel1.IsDerivedStyle = true;
            metroSetLabel1.Location = new Point(15, 92);
            metroSetLabel1.Name = "metroSetLabel1";
            metroSetLabel1.Size = new Size(100, 23);
            metroSetLabel1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel1.StyleManager = null;
            metroSetLabel1.TabIndex = 0;
            metroSetLabel1.Text = "Enter Amount:";
            metroSetLabel1.ThemeAuthor = "Narwin";
            metroSetLabel1.ThemeName = "MetroLite";
            // 
            // txtAmount
            // 
            txtAmount.AutoCompleteCustomSource = null;
            txtAmount.AutoCompleteMode = AutoCompleteMode.None;
            txtAmount.AutoCompleteSource = AutoCompleteSource.None;
            txtAmount.BorderColor = Color.FromArgb(155, 155, 155);
            txtAmount.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtAmount.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtAmount.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtAmount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtAmount.HoverColor = Color.FromArgb(102, 102, 102);
            txtAmount.Image = null;
            txtAmount.IsDerivedStyle = true;
            txtAmount.Lines = null;
            txtAmount.Location = new Point(143, 85);
            txtAmount.MaxLength = 32767;
            txtAmount.Multiline = false;
            txtAmount.Name = "txtAmount";
            txtAmount.ReadOnly = false;
            txtAmount.Size = new Size(159, 30);
            txtAmount.Style = MetroSet_UI.Enums.Style.Light;
            txtAmount.StyleManager = null;
            txtAmount.TabIndex = 1;
            txtAmount.TextAlign = HorizontalAlignment.Left;
            txtAmount.ThemeAuthor = "Narwin";
            txtAmount.ThemeName = "MetroLite";
            txtAmount.UseSystemPasswordChar = false;
            txtAmount.WatermarkText = "";
            // 
            // metroSetLabel2
            // 
            metroSetLabel2.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel2.IsDerivedStyle = true;
            metroSetLabel2.Location = new Point(15, 133);
            metroSetLabel2.Name = "metroSetLabel2";
            metroSetLabel2.Size = new Size(114, 23);
            metroSetLabel2.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel2.StyleManager = null;
            metroSetLabel2.TabIndex = 2;
            metroSetLabel2.Text = "Payment Method";
            metroSetLabel2.ThemeAuthor = "Narwin";
            metroSetLabel2.ThemeName = "MetroLite";
            // 
            // comboPaymentMethod
            // 
            comboPaymentMethod.AllowDrop = true;
            comboPaymentMethod.ArrowColor = Color.FromArgb(150, 150, 150);
            comboPaymentMethod.BackColor = Color.Transparent;
            comboPaymentMethod.BackgroundColor = Color.FromArgb(238, 238, 238);
            comboPaymentMethod.BorderColor = Color.FromArgb(150, 150, 150);
            comboPaymentMethod.CausesValidation = false;
            comboPaymentMethod.DisabledBackColor = Color.FromArgb(204, 204, 204);
            comboPaymentMethod.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            comboPaymentMethod.DisabledForeColor = Color.FromArgb(136, 136, 136);
            comboPaymentMethod.DrawMode = DrawMode.OwnerDrawFixed;
            comboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            comboPaymentMethod.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point);
            comboPaymentMethod.FormattingEnabled = true;
            comboPaymentMethod.IsDerivedStyle = true;
            comboPaymentMethod.ItemHeight = 20;
            comboPaymentMethod.Location = new Point(143, 133);
            comboPaymentMethod.Name = "comboPaymentMethod";
            comboPaymentMethod.SelectedItemBackColor = Color.FromArgb(65, 177, 225);
            comboPaymentMethod.SelectedItemForeColor = Color.White;
            comboPaymentMethod.Size = new Size(159, 26);
            comboPaymentMethod.Style = MetroSet_UI.Enums.Style.Light;
            comboPaymentMethod.StyleManager = null;
            comboPaymentMethod.TabIndex = 3;
            comboPaymentMethod.ThemeAuthor = "Narwin";
            comboPaymentMethod.ThemeName = "MetroLite";
            // 
            // btnOK
            // 
            btnOK.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnOK.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnOK.DisabledForeColor = Color.Gray;
            btnOK.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnOK.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnOK.HoverColor = Color.FromArgb(95, 207, 255);
            btnOK.HoverTextColor = Color.White;
            btnOK.IsDerivedStyle = true;
            btnOK.Location = new Point(112, 194);
            btnOK.Name = "btnOK";
            btnOK.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnOK.NormalColor = Color.FromArgb(65, 177, 225);
            btnOK.NormalTextColor = Color.White;
            btnOK.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnOK.PressColor = Color.FromArgb(35, 147, 195);
            btnOK.PressTextColor = Color.White;
            btnOK.Size = new Size(109, 38);
            btnOK.Style = MetroSet_UI.Enums.Style.Light;
            btnOK.StyleManager = null;
            btnOK.TabIndex = 4;
            btnOK.Text = "SAVE";
            btnOK.ThemeAuthor = "Narwin";
            btnOK.ThemeName = "MetroLite";
            btnOK.Click += btnOK_Click;
            // 
            // metroSetControlBox1
            // 
            metroSetControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            metroSetControlBox1.CloseHoverBackColor = Color.FromArgb(183, 40, 40);
            metroSetControlBox1.CloseHoverForeColor = Color.White;
            metroSetControlBox1.CloseNormalForeColor = Color.Gray;
            metroSetControlBox1.DisabledForeColor = Color.DimGray;
            metroSetControlBox1.IsDerivedStyle = true;
            metroSetControlBox1.Location = new Point(250, 8);
            metroSetControlBox1.MaximizeBox = true;
            metroSetControlBox1.MaximizeHoverBackColor = Color.FromArgb(238, 238, 238);
            metroSetControlBox1.MaximizeHoverForeColor = Color.Gray;
            metroSetControlBox1.MaximizeNormalForeColor = Color.Gray;
            metroSetControlBox1.MinimizeBox = true;
            metroSetControlBox1.MinimizeHoverBackColor = Color.FromArgb(238, 238, 238);
            metroSetControlBox1.MinimizeHoverForeColor = Color.Gray;
            metroSetControlBox1.MinimizeNormalForeColor = Color.Gray;
            metroSetControlBox1.Name = "metroSetControlBox1";
            metroSetControlBox1.Size = new Size(100, 25);
            metroSetControlBox1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetControlBox1.StyleManager = null;
            metroSetControlBox1.TabIndex = 5;
            metroSetControlBox1.Text = "metroSetControlBox1";
            metroSetControlBox1.ThemeAuthor = "Narwin";
            metroSetControlBox1.ThemeName = "MetroLite";
            // 
            // AddPaymentForm
            // 
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(355, 254);
            Controls.Add(metroSetControlBox1);
            Controls.Add(btnOK);
            Controls.Add(comboPaymentMethod);
            Controls.Add(metroSetLabel2);
            Controls.Add(txtAmount);
            Controls.Add(metroSetLabel1);
            Name = "AddPaymentForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add Payment";
            ResumeLayout(false);
        }

        #endregion

        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel1;
        private MetroSet_UI.Controls.MetroSetTextBox txtAmount;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel2;
        private MetroSet_UI.Controls.MetroSetComboBox comboPaymentMethod;
        private MetroSet_UI.Controls.MetroSetButton btnOK;
        private MetroSet_UI.Controls.MetroSetControlBox metroSetControlBox1;
    }
}