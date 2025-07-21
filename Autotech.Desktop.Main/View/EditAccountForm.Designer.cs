using System.Xml.Linq;

namespace Autotech.Desktop.Main.View
{
    partial class EditAccountForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private TextBox txtName;
        private TextBox txtContactPerson;
        private TextBox txtEmail;
        private TextBox txtContactNumber;
        private TextBox txtAddress;
        private NumericUpDown txtTerms;
        private CheckBox chkIsActive;
        private Button btnSave;
        private Label lblTitle;

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
            txtName = new TextBox();
            txtContactPerson = new TextBox();
            txtEmail = new TextBox();
            txtContactNumber = new TextBox();
            txtAddress = new TextBox();
            txtTerms = new NumericUpDown();
            chkIsActive = new CheckBox();
            btnSave = new Button();
            lblTitle = new Label();
            btnCancel = new Button();
            dtmDateRegistered = new DateTimePicker();
            metroSetLabel1 = new MetroSet_UI.Controls.MetroSetLabel();
            metroSetLabel2 = new MetroSet_UI.Controls.MetroSetLabel();
            metroSetLabel3 = new MetroSet_UI.Controls.MetroSetLabel();
            cboLocation = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)txtTerms).BeginInit();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Location = new Point(15, 87);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "Account Name";
            txtName.Size = new Size(300, 27);
            txtName.TabIndex = 1;
            // 
            // txtContactPerson
            // 
            txtContactPerson.Location = new Point(15, 122);
            txtContactPerson.Name = "txtContactPerson";
            txtContactPerson.PlaceholderText = "Contact Person";
            txtContactPerson.Size = new Size(300, 27);
            txtContactPerson.TabIndex = 2;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(15, 157);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Email";
            txtEmail.Size = new Size(300, 27);
            txtEmail.TabIndex = 3;
            // 
            // txtContactNumber
            // 
            txtContactNumber.Location = new Point(15, 192);
            txtContactNumber.Name = "txtContactNumber";
            txtContactNumber.PlaceholderText = "Contact Number";
            txtContactNumber.Size = new Size(300, 27);
            txtContactNumber.TabIndex = 4;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(15, 227);
            txtAddress.Name = "txtAddress";
            txtAddress.PlaceholderText = "Address";
            txtAddress.Size = new Size(300, 27);
            txtAddress.TabIndex = 5;
            // 
            // txtTerms
            // 
            txtTerms.Location = new Point(84, 295);
            txtTerms.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            txtTerms.Name = "txtTerms";
            txtTerms.Size = new Size(100, 27);
            txtTerms.TabIndex = 7;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Location = new Point(15, 379);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(96, 26);
            chkIsActive.TabIndex = 9;
            chkIsActive.Text = "Is Active";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(15, 413);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 32);
            btnSave.TabIndex = 10;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.Location = new Point(15, 47);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(125, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Edit Account";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(210, 413);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 32);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // dtmDateRegistered
            // 
            dtmDateRegistered.Location = new Point(84, 337);
            dtmDateRegistered.Name = "dtmDateRegistered";
            dtmDateRegistered.Size = new Size(200, 27);
            dtmDateRegistered.TabIndex = 12;
            // 
            // metroSetLabel1
            // 
            metroSetLabel1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel1.IsDerivedStyle = true;
            metroSetLabel1.Location = new Point(15, 299);
            metroSetLabel1.Name = "metroSetLabel1";
            metroSetLabel1.Size = new Size(54, 23);
            metroSetLabel1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel1.StyleManager = null;
            metroSetLabel1.TabIndex = 13;
            metroSetLabel1.Text = "Terms";
            metroSetLabel1.ThemeAuthor = "Narwin";
            metroSetLabel1.ThemeName = "MetroLite";
            // 
            // metroSetLabel2
            // 
            metroSetLabel2.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel2.IsDerivedStyle = true;
            metroSetLabel2.Location = new Point(15, 325);
            metroSetLabel2.Name = "metroSetLabel2";
            metroSetLabel2.Size = new Size(63, 53);
            metroSetLabel2.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel2.StyleManager = null;
            metroSetLabel2.TabIndex = 14;
            metroSetLabel2.Text = "Register date";
            metroSetLabel2.TextAlign = ContentAlignment.MiddleLeft;
            metroSetLabel2.ThemeAuthor = "Narwin";
            metroSetLabel2.ThemeName = "MetroLite";
            // 
            // metroSetLabel3
            // 
            metroSetLabel3.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel3.IsDerivedStyle = true;
            metroSetLabel3.Location = new Point(15, 264);
            metroSetLabel3.Name = "metroSetLabel3";
            metroSetLabel3.Size = new Size(63, 23);
            metroSetLabel3.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel3.StyleManager = null;
            metroSetLabel3.TabIndex = 16;
            metroSetLabel3.Text = "Cluster";
            metroSetLabel3.ThemeAuthor = "Narwin";
            metroSetLabel3.ThemeName = "MetroLite";
            // 
            // cboLocation
            // 
            cboLocation.FormattingEnabled = true;
            cboLocation.Location = new Point(84, 261);
            cboLocation.Name = "cboLocation";
            cboLocation.Size = new Size(200, 28);
            cboLocation.TabIndex = 17;
            cboLocation.Click += cboLocation_Click;
            // 
            // EditAccountForm
            // 
            ClientSize = new Size(325, 465);
            Controls.Add(cboLocation);
            Controls.Add(metroSetLabel3);
            Controls.Add(metroSetLabel2);
            Controls.Add(metroSetLabel1);
            Controls.Add(dtmDateRegistered);
            Controls.Add(btnCancel);
            Controls.Add(lblTitle);
            Controls.Add(txtName);
            Controls.Add(txtContactPerson);
            Controls.Add(txtEmail);
            Controls.Add(txtContactNumber);
            Controls.Add(txtAddress);
            Controls.Add(txtTerms);
            Controls.Add(chkIsActive);
            Controls.Add(btnSave);
            Name = "EditAccountForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Edit Account";
            Load += EditAccountForm_Load;
            ((System.ComponentModel.ISupportInitialize)txtTerms).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private Button btnCancel;
        private DateTimePicker dtmDateRegistered;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel1;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel2;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel3;
        private ComboBox cboLocation;
    }
}