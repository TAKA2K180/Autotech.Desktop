namespace Autotech.Desktop.Main.View
{
    partial class EditAgentForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            txtAgentName = new TextBox();
            txtContact = new TextBox();
            txtAddress = new TextBox();
            txtRole = new TextBox();
            cboLocation = new ComboBox();
            btnSave = new Button();
            metroSetLabel1 = new MetroSet_UI.Controls.MetroSetLabel();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(20, 92);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Username";
            txtUsername.Size = new Size(250, 27);
            txtUsername.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(20, 127);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Password";
            txtPassword.Size = new Size(250, 27);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // txtAgentName
            // 
            txtAgentName.Location = new Point(20, 162);
            txtAgentName.Name = "txtAgentName";
            txtAgentName.PlaceholderText = "Agent Name";
            txtAgentName.Size = new Size(250, 27);
            txtAgentName.TabIndex = 2;
            // 
            // txtContact
            // 
            txtContact.Location = new Point(20, 197);
            txtContact.Name = "txtContact";
            txtContact.PlaceholderText = "Contact Number";
            txtContact.Size = new Size(250, 27);
            txtContact.TabIndex = 3;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(20, 232);
            txtAddress.Name = "txtAddress";
            txtAddress.PlaceholderText = "Address";
            txtAddress.Size = new Size(250, 27);
            txtAddress.TabIndex = 4;
            // 
            // txtRole
            // 
            txtRole.Location = new Point(20, 267);
            txtRole.Name = "txtRole";
            txtRole.PlaceholderText = "Role";
            txtRole.Size = new Size(250, 27);
            txtRole.TabIndex = 5;
            // 
            // cboLocation
            // 
            cboLocation.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocation.Location = new Point(19, 330);
            cboLocation.Name = "cboLocation";
            cboLocation.Size = new Size(250, 28);
            cboLocation.TabIndex = 8;
            cboLocation.Click += cboLocation_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(24, 393);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(99, 30);
            btnSave.TabIndex = 9;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // metroSetLabel1
            // 
            metroSetLabel1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel1.IsDerivedStyle = true;
            metroSetLabel1.Location = new Point(19, 304);
            metroSetLabel1.Name = "metroSetLabel1";
            metroSetLabel1.Size = new Size(100, 23);
            metroSetLabel1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetLabel1.StyleManager = null;
            metroSetLabel1.TabIndex = 10;
            metroSetLabel1.Text = "Location";
            metroSetLabel1.ThemeAuthor = "Narwin";
            metroSetLabel1.ThemeName = "MetroLite";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(152, 393);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(99, 30);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // EditAgentForm
            // 
            ClientSize = new Size(288, 446);
            Controls.Add(btnCancel);
            Controls.Add(metroSetLabel1);
            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Controls.Add(txtAgentName);
            Controls.Add(txtContact);
            Controls.Add(txtAddress);
            Controls.Add(txtRole);
            Controls.Add(cboLocation);
            Controls.Add(btnSave);
            Name = "EditAgentForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Edit Agent";
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtAgentName;
        private TextBox txtContact;
        private TextBox txtAddress;
        private TextBox txtRole;
        private ComboBox cboLocation;
        private Button btnSave;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel1;
        private Button btnCancel;
    }
}
