using MetroSet_UI.Controls;

namespace Autotech.Desktop.Main.View
{
    partial class RegistrationForm
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
        /// 
        private MetroSetLabel lblName;
        private MetroSetTextBox txtName;
        private MetroSetLabel lblContactPerson;
        private MetroSetTextBox txtContactPerson;
        private MetroSetLabel lblEmail;
        private MetroSetTextBox txtEmail;
        private MetroSetLabel lblContactNumber;
        private MetroSetTextBox txtContactNumber;
        private MetroSetLabel lblAddress;
        private MetroSetTextBox txtAddress;
        private MetroSetLabel lblTerms;
        private MetroSetTextBox txtTerms;
        private MetroSetLabel lblDiscountPercent;
        private MetroSetTextBox txtDiscountPercent;
        private MetroSetLabel lblCluster;
        private MetroSetTextBox txtCluster;
        private MetroSetLabel lblLocation;
        private MetroSetComboBox comboLocation;
        private MetroSetCheckBox chkIsActive;
        private MetroSetButton btnRegister;
        private void InitializeComponent()
        {
            lblName = new MetroSetLabel();
            txtName = new MetroSetTextBox();
            lblContactPerson = new MetroSetLabel();
            txtContactPerson = new MetroSetTextBox();
            lblEmail = new MetroSetLabel();
            txtEmail = new MetroSetTextBox();
            lblContactNumber = new MetroSetLabel();
            txtContactNumber = new MetroSetTextBox();
            lblAddress = new MetroSetLabel();
            txtAddress = new MetroSetTextBox();
            lblTerms = new MetroSetLabel();
            txtTerms = new MetroSetTextBox();
            lblDiscountPercent = new MetroSetLabel();
            txtDiscountPercent = new MetroSetTextBox();
            lblCluster = new MetroSetLabel();
            txtCluster = new MetroSetTextBox();
            lblLocation = new MetroSetLabel();
            comboLocation = new MetroSetComboBox();
            chkIsActive = new MetroSetCheckBox();
            btnRegister = new MetroSetButton();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblName.IsDerivedStyle = true;
            lblName.Location = new Point(47, 86);
            lblName.Name = "lblName";
            lblName.Size = new Size(100, 25);
            lblName.Style = MetroSet_UI.Enums.Style.Light;
            lblName.StyleManager = null;
            lblName.TabIndex = 0;
            lblName.Text = "Name:";
            lblName.ThemeAuthor = "Narwin";
            lblName.ThemeName = "MetroLite";
            // 
            // txtName
            // 
            txtName.AutoCompleteCustomSource = null;
            txtName.AutoCompleteMode = AutoCompleteMode.None;
            txtName.AutoCompleteSource = AutoCompleteSource.None;
            txtName.BorderColor = Color.FromArgb(155, 155, 155);
            txtName.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtName.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtName.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtName.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtName.HoverColor = Color.FromArgb(102, 102, 102);
            txtName.Image = null;
            txtName.IsDerivedStyle = true;
            txtName.Lines = null;
            txtName.Location = new Point(197, 86);
            txtName.MaxLength = 32767;
            txtName.Multiline = false;
            txtName.Name = "txtName";
            txtName.ReadOnly = false;
            txtName.Size = new Size(250, 25);
            txtName.Style = MetroSet_UI.Enums.Style.Light;
            txtName.StyleManager = null;
            txtName.TabIndex = 1;
            txtName.TextAlign = HorizontalAlignment.Left;
            txtName.ThemeAuthor = "Narwin";
            txtName.ThemeName = "MetroLite";
            txtName.UseSystemPasswordChar = false;
            txtName.WatermarkText = "Enter name";
            // 
            // lblContactPerson
            // 
            lblContactPerson.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblContactPerson.IsDerivedStyle = true;
            lblContactPerson.Location = new Point(47, 126);
            lblContactPerson.Name = "lblContactPerson";
            lblContactPerson.Size = new Size(120, 25);
            lblContactPerson.Style = MetroSet_UI.Enums.Style.Light;
            lblContactPerson.StyleManager = null;
            lblContactPerson.TabIndex = 2;
            lblContactPerson.Text = "Contact Person:";
            lblContactPerson.ThemeAuthor = "Narwin";
            lblContactPerson.ThemeName = "MetroLite";
            // 
            // txtContactPerson
            // 
            txtContactPerson.AutoCompleteCustomSource = null;
            txtContactPerson.AutoCompleteMode = AutoCompleteMode.None;
            txtContactPerson.AutoCompleteSource = AutoCompleteSource.None;
            txtContactPerson.BorderColor = Color.FromArgb(155, 155, 155);
            txtContactPerson.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtContactPerson.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtContactPerson.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtContactPerson.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtContactPerson.HoverColor = Color.FromArgb(102, 102, 102);
            txtContactPerson.Image = null;
            txtContactPerson.IsDerivedStyle = true;
            txtContactPerson.Lines = null;
            txtContactPerson.Location = new Point(197, 126);
            txtContactPerson.MaxLength = 32767;
            txtContactPerson.Multiline = false;
            txtContactPerson.Name = "txtContactPerson";
            txtContactPerson.ReadOnly = false;
            txtContactPerson.Size = new Size(250, 25);
            txtContactPerson.Style = MetroSet_UI.Enums.Style.Light;
            txtContactPerson.StyleManager = null;
            txtContactPerson.TabIndex = 3;
            txtContactPerson.TextAlign = HorizontalAlignment.Left;
            txtContactPerson.ThemeAuthor = "Narwin";
            txtContactPerson.ThemeName = "MetroLite";
            txtContactPerson.UseSystemPasswordChar = false;
            txtContactPerson.WatermarkText = "Enter contact person";
            // 
            // lblEmail
            // 
            lblEmail.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblEmail.IsDerivedStyle = true;
            lblEmail.Location = new Point(47, 166);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(100, 25);
            lblEmail.Style = MetroSet_UI.Enums.Style.Light;
            lblEmail.StyleManager = null;
            lblEmail.TabIndex = 4;
            lblEmail.Text = "Email:";
            lblEmail.ThemeAuthor = "Narwin";
            lblEmail.ThemeName = "MetroLite";
            // 
            // txtEmail
            // 
            txtEmail.AutoCompleteCustomSource = null;
            txtEmail.AutoCompleteMode = AutoCompleteMode.None;
            txtEmail.AutoCompleteSource = AutoCompleteSource.None;
            txtEmail.BorderColor = Color.FromArgb(155, 155, 155);
            txtEmail.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtEmail.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtEmail.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtEmail.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtEmail.HoverColor = Color.FromArgb(102, 102, 102);
            txtEmail.Image = null;
            txtEmail.IsDerivedStyle = true;
            txtEmail.Lines = null;
            txtEmail.Location = new Point(197, 166);
            txtEmail.MaxLength = 32767;
            txtEmail.Multiline = false;
            txtEmail.Name = "txtEmail";
            txtEmail.ReadOnly = false;
            txtEmail.Size = new Size(250, 25);
            txtEmail.Style = MetroSet_UI.Enums.Style.Light;
            txtEmail.StyleManager = null;
            txtEmail.TabIndex = 5;
            txtEmail.TextAlign = HorizontalAlignment.Left;
            txtEmail.ThemeAuthor = "Narwin";
            txtEmail.ThemeName = "MetroLite";
            txtEmail.UseSystemPasswordChar = false;
            txtEmail.WatermarkText = "Enter email";
            // 
            // lblContactNumber
            // 
            lblContactNumber.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblContactNumber.IsDerivedStyle = true;
            lblContactNumber.Location = new Point(47, 206);
            lblContactNumber.Name = "lblContactNumber";
            lblContactNumber.Size = new Size(120, 25);
            lblContactNumber.Style = MetroSet_UI.Enums.Style.Light;
            lblContactNumber.StyleManager = null;
            lblContactNumber.TabIndex = 6;
            lblContactNumber.Text = "Contact Number:";
            lblContactNumber.ThemeAuthor = "Narwin";
            lblContactNumber.ThemeName = "MetroLite";
            // 
            // txtContactNumber
            // 
            txtContactNumber.AutoCompleteCustomSource = null;
            txtContactNumber.AutoCompleteMode = AutoCompleteMode.None;
            txtContactNumber.AutoCompleteSource = AutoCompleteSource.None;
            txtContactNumber.BorderColor = Color.FromArgb(155, 155, 155);
            txtContactNumber.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtContactNumber.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtContactNumber.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtContactNumber.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtContactNumber.HoverColor = Color.FromArgb(102, 102, 102);
            txtContactNumber.Image = null;
            txtContactNumber.IsDerivedStyle = true;
            txtContactNumber.Lines = null;
            txtContactNumber.Location = new Point(197, 206);
            txtContactNumber.MaxLength = 32767;
            txtContactNumber.Multiline = false;
            txtContactNumber.Name = "txtContactNumber";
            txtContactNumber.ReadOnly = false;
            txtContactNumber.Size = new Size(250, 25);
            txtContactNumber.Style = MetroSet_UI.Enums.Style.Light;
            txtContactNumber.StyleManager = null;
            txtContactNumber.TabIndex = 7;
            txtContactNumber.TextAlign = HorizontalAlignment.Left;
            txtContactNumber.ThemeAuthor = "Narwin";
            txtContactNumber.ThemeName = "MetroLite";
            txtContactNumber.UseSystemPasswordChar = false;
            txtContactNumber.WatermarkText = "Enter contact number";
            // 
            // lblAddress
            // 
            lblAddress.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblAddress.IsDerivedStyle = true;
            lblAddress.Location = new Point(47, 246);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(100, 25);
            lblAddress.Style = MetroSet_UI.Enums.Style.Light;
            lblAddress.StyleManager = null;
            lblAddress.TabIndex = 8;
            lblAddress.Text = "Address:";
            lblAddress.ThemeAuthor = "Narwin";
            lblAddress.ThemeName = "MetroLite";
            // 
            // txtAddress
            // 
            txtAddress.AutoCompleteCustomSource = null;
            txtAddress.AutoCompleteMode = AutoCompleteMode.None;
            txtAddress.AutoCompleteSource = AutoCompleteSource.None;
            txtAddress.BorderColor = Color.FromArgb(155, 155, 155);
            txtAddress.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtAddress.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtAddress.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtAddress.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtAddress.HoverColor = Color.FromArgb(102, 102, 102);
            txtAddress.Image = null;
            txtAddress.IsDerivedStyle = true;
            txtAddress.Lines = null;
            txtAddress.Location = new Point(197, 246);
            txtAddress.MaxLength = 32767;
            txtAddress.Multiline = false;
            txtAddress.Name = "txtAddress";
            txtAddress.ReadOnly = false;
            txtAddress.Size = new Size(250, 25);
            txtAddress.Style = MetroSet_UI.Enums.Style.Light;
            txtAddress.StyleManager = null;
            txtAddress.TabIndex = 9;
            txtAddress.TextAlign = HorizontalAlignment.Left;
            txtAddress.ThemeAuthor = "Narwin";
            txtAddress.ThemeName = "MetroLite";
            txtAddress.UseSystemPasswordChar = false;
            txtAddress.WatermarkText = "Enter address";
            // 
            // lblTerms
            // 
            lblTerms.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTerms.IsDerivedStyle = true;
            lblTerms.Location = new Point(47, 286);
            lblTerms.Name = "lblTerms";
            lblTerms.Size = new Size(100, 25);
            lblTerms.Style = MetroSet_UI.Enums.Style.Light;
            lblTerms.StyleManager = null;
            lblTerms.TabIndex = 10;
            lblTerms.Text = "Terms:";
            lblTerms.ThemeAuthor = "Narwin";
            lblTerms.ThemeName = "MetroLite";
            // 
            // txtTerms
            // 
            txtTerms.AutoCompleteCustomSource = null;
            txtTerms.AutoCompleteMode = AutoCompleteMode.None;
            txtTerms.AutoCompleteSource = AutoCompleteSource.None;
            txtTerms.BorderColor = Color.FromArgb(155, 155, 155);
            txtTerms.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtTerms.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtTerms.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtTerms.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtTerms.HoverColor = Color.FromArgb(102, 102, 102);
            txtTerms.Image = null;
            txtTerms.IsDerivedStyle = true;
            txtTerms.Lines = null;
            txtTerms.Location = new Point(197, 286);
            txtTerms.MaxLength = 32767;
            txtTerms.Multiline = false;
            txtTerms.Name = "txtTerms";
            txtTerms.ReadOnly = false;
            txtTerms.Size = new Size(250, 25);
            txtTerms.Style = MetroSet_UI.Enums.Style.Light;
            txtTerms.StyleManager = null;
            txtTerms.TabIndex = 11;
            txtTerms.TextAlign = HorizontalAlignment.Left;
            txtTerms.ThemeAuthor = "Narwin";
            txtTerms.ThemeName = "MetroLite";
            txtTerms.UseSystemPasswordChar = false;
            txtTerms.WatermarkText = "Enter terms";
            // 
            // lblDiscountPercent
            // 
            lblDiscountPercent.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblDiscountPercent.IsDerivedStyle = true;
            lblDiscountPercent.Location = new Point(47, 326);
            lblDiscountPercent.Name = "lblDiscountPercent";
            lblDiscountPercent.Size = new Size(120, 25);
            lblDiscountPercent.Style = MetroSet_UI.Enums.Style.Light;
            lblDiscountPercent.StyleManager = null;
            lblDiscountPercent.TabIndex = 12;
            lblDiscountPercent.Text = "Discount Percent:";
            lblDiscountPercent.ThemeAuthor = "Narwin";
            lblDiscountPercent.ThemeName = "MetroLite";
            // 
            // txtDiscountPercent
            // 
            txtDiscountPercent.AutoCompleteCustomSource = null;
            txtDiscountPercent.AutoCompleteMode = AutoCompleteMode.None;
            txtDiscountPercent.AutoCompleteSource = AutoCompleteSource.None;
            txtDiscountPercent.BorderColor = Color.FromArgb(155, 155, 155);
            txtDiscountPercent.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtDiscountPercent.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtDiscountPercent.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtDiscountPercent.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtDiscountPercent.HoverColor = Color.FromArgb(102, 102, 102);
            txtDiscountPercent.Image = null;
            txtDiscountPercent.IsDerivedStyle = true;
            txtDiscountPercent.Lines = null;
            txtDiscountPercent.Location = new Point(197, 326);
            txtDiscountPercent.MaxLength = 32767;
            txtDiscountPercent.Multiline = false;
            txtDiscountPercent.Name = "txtDiscountPercent";
            txtDiscountPercent.ReadOnly = false;
            txtDiscountPercent.Size = new Size(250, 25);
            txtDiscountPercent.Style = MetroSet_UI.Enums.Style.Light;
            txtDiscountPercent.StyleManager = null;
            txtDiscountPercent.TabIndex = 13;
            txtDiscountPercent.TextAlign = HorizontalAlignment.Left;
            txtDiscountPercent.ThemeAuthor = "Narwin";
            txtDiscountPercent.ThemeName = "MetroLite";
            txtDiscountPercent.UseSystemPasswordChar = false;
            txtDiscountPercent.WatermarkText = "Enter discount percentage";
            // 
            // lblCluster
            // 
            lblCluster.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblCluster.IsDerivedStyle = true;
            lblCluster.Location = new Point(47, 366);
            lblCluster.Name = "lblCluster";
            lblCluster.Size = new Size(100, 25);
            lblCluster.Style = MetroSet_UI.Enums.Style.Light;
            lblCluster.StyleManager = null;
            lblCluster.TabIndex = 14;
            lblCluster.Text = "Cluster:";
            lblCluster.ThemeAuthor = "Narwin";
            lblCluster.ThemeName = "MetroLite";
            // 
            // txtCluster
            // 
            txtCluster.AutoCompleteCustomSource = null;
            txtCluster.AutoCompleteMode = AutoCompleteMode.None;
            txtCluster.AutoCompleteSource = AutoCompleteSource.None;
            txtCluster.BorderColor = Color.FromArgb(155, 155, 155);
            txtCluster.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtCluster.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtCluster.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtCluster.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtCluster.HoverColor = Color.FromArgb(102, 102, 102);
            txtCluster.Image = null;
            txtCluster.IsDerivedStyle = true;
            txtCluster.Lines = null;
            txtCluster.Location = new Point(197, 366);
            txtCluster.MaxLength = 32767;
            txtCluster.Multiline = false;
            txtCluster.Name = "txtCluster";
            txtCluster.ReadOnly = false;
            txtCluster.Size = new Size(250, 25);
            txtCluster.Style = MetroSet_UI.Enums.Style.Light;
            txtCluster.StyleManager = null;
            txtCluster.TabIndex = 15;
            txtCluster.TextAlign = HorizontalAlignment.Left;
            txtCluster.ThemeAuthor = "Narwin";
            txtCluster.ThemeName = "MetroLite";
            txtCluster.UseSystemPasswordChar = false;
            txtCluster.WatermarkText = "Enter cluster";
            // 
            // lblLocation
            // 
            lblLocation.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblLocation.IsDerivedStyle = true;
            lblLocation.Location = new Point(47, 406);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(100, 25);
            lblLocation.Style = MetroSet_UI.Enums.Style.Light;
            lblLocation.StyleManager = null;
            lblLocation.TabIndex = 16;
            lblLocation.Text = "Location:";
            lblLocation.ThemeAuthor = "Narwin";
            lblLocation.ThemeName = "MetroLite";
            // 
            // comboLocation
            // 
            comboLocation.AllowDrop = true;
            comboLocation.ArrowColor = Color.FromArgb(150, 150, 150);
            comboLocation.BackColor = Color.Transparent;
            comboLocation.BackgroundColor = Color.FromArgb(238, 238, 238);
            comboLocation.BorderColor = Color.FromArgb(150, 150, 150);
            comboLocation.CausesValidation = false;
            comboLocation.DisabledBackColor = Color.FromArgb(204, 204, 204);
            comboLocation.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            comboLocation.DisabledForeColor = Color.FromArgb(136, 136, 136);
            comboLocation.DrawMode = DrawMode.OwnerDrawFixed;
            comboLocation.DropDownStyle = ComboBoxStyle.DropDownList;
            comboLocation.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point);
            comboLocation.IsDerivedStyle = true;
            comboLocation.ItemHeight = 20;
            comboLocation.Items.AddRange(new object[] { "Location 1", "Location 2", "Location 3" });
            comboLocation.Location = new Point(197, 406);
            comboLocation.Name = "comboLocation";
            comboLocation.SelectedItemBackColor = Color.FromArgb(65, 177, 225);
            comboLocation.SelectedItemForeColor = Color.White;
            comboLocation.Size = new Size(250, 26);
            comboLocation.Style = MetroSet_UI.Enums.Style.Light;
            comboLocation.StyleManager = null;
            comboLocation.TabIndex = 17;
            comboLocation.ThemeAuthor = "Narwin";
            comboLocation.ThemeName = "MetroLite";
            // 
            // chkIsActive
            // 
            chkIsActive.BackColor = Color.Transparent;
            chkIsActive.BackgroundColor = Color.White;
            chkIsActive.BorderColor = Color.FromArgb(155, 155, 155);
            chkIsActive.Checked = false;
            chkIsActive.CheckSignColor = Color.FromArgb(65, 177, 225);
            chkIsActive.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            chkIsActive.DisabledBorderColor = Color.FromArgb(205, 205, 205);
            chkIsActive.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            chkIsActive.IsDerivedStyle = true;
            chkIsActive.Location = new Point(47, 446);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.SignStyle = MetroSet_UI.Enums.SignStyle.Sign;
            chkIsActive.Size = new Size(100, 16);
            chkIsActive.Style = MetroSet_UI.Enums.Style.Light;
            chkIsActive.StyleManager = null;
            chkIsActive.TabIndex = 18;
            chkIsActive.Text = "Is Active";
            chkIsActive.ThemeAuthor = "Narwin";
            chkIsActive.ThemeName = "MetroLite";
            // 
            // btnRegister
            // 
            btnRegister.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnRegister.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnRegister.DisabledForeColor = Color.Gray;
            btnRegister.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnRegister.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnRegister.HoverColor = Color.FromArgb(95, 207, 255);
            btnRegister.HoverTextColor = Color.White;
            btnRegister.IsDerivedStyle = true;
            btnRegister.Location = new Point(197, 496);
            btnRegister.Name = "btnRegister";
            btnRegister.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnRegister.NormalColor = Color.FromArgb(65, 177, 225);
            btnRegister.NormalTextColor = Color.White;
            btnRegister.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnRegister.PressColor = Color.FromArgb(35, 147, 195);
            btnRegister.PressTextColor = Color.White;
            btnRegister.Size = new Size(100, 40);
            btnRegister.Style = MetroSet_UI.Enums.Style.Light;
            btnRegister.StyleManager = null;
            btnRegister.TabIndex = 19;
            btnRegister.Text = "Register";
            btnRegister.ThemeAuthor = "Narwin";
            btnRegister.ThemeName = "MetroLite";
            // 
            // RegistrationForm
            // 
            ClientSize = new Size(500, 600);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblContactPerson);
            Controls.Add(txtContactPerson);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblContactNumber);
            Controls.Add(txtContactNumber);
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            Controls.Add(lblTerms);
            Controls.Add(txtTerms);
            Controls.Add(lblDiscountPercent);
            Controls.Add(txtDiscountPercent);
            Controls.Add(lblCluster);
            Controls.Add(txtCluster);
            Controls.Add(lblLocation);
            Controls.Add(comboLocation);
            Controls.Add(chkIsActive);
            Controls.Add(btnRegister);
            Name = "RegistrationForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Account Registration";
            ResumeLayout(false);
        }

        #endregion
    }
}