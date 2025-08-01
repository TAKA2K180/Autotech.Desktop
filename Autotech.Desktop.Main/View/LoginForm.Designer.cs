﻿using Autotech.Desktop.BusinessLayer.Services;
using MetroSet_UI.Controls;
using MetroSet_UI.Forms;

namespace Autotech.Desktop.Main.View
{
    partial class LoginForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        /// 
        private MetroSetLabel lblUsername;
        private MetroSetTextBox txtUsername;
        private MetroSetLabel lblPassword;
        private MetroSetTextBox txtPassword;
        private MetroSetButton btnLogin;
        private void InitializeComponent()
        {
            lblUsername = new MetroSetLabel();
            txtUsername = new MetroSetTextBox();
            lblPassword = new MetroSetLabel();
            txtPassword = new MetroSetTextBox();
            btnLogin = new MetroSetButton();
            metroSetControlBox1 = new MetroSetControlBox();
            SuspendLayout();
            // 
            // lblUsername
            // 
            lblUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblUsername.IsDerivedStyle = true;
            lblUsername.Location = new Point(41, 92);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(100, 23);
            lblUsername.Style = MetroSet_UI.Enums.Style.Light;
            lblUsername.StyleManager = null;
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Username:";
            lblUsername.ThemeAuthor = "Narwin";
            lblUsername.ThemeName = "MetroLite";
            // 
            // txtUsername
            // 
            txtUsername.AutoCompleteCustomSource = null;
            txtUsername.AutoCompleteMode = AutoCompleteMode.None;
            txtUsername.AutoCompleteSource = AutoCompleteSource.None;
            txtUsername.BorderColor = Color.FromArgb(155, 155, 155);
            txtUsername.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtUsername.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtUsername.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtUsername.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtUsername.HoverColor = Color.FromArgb(102, 102, 102);
            txtUsername.Image = null;
            txtUsername.IsDerivedStyle = true;
            txtUsername.Lines = null;
            txtUsername.Location = new Point(141, 92);
            txtUsername.MaxLength = 32767;
            txtUsername.Multiline = false;
            txtUsername.Name = "txtUsername";
            txtUsername.ReadOnly = false;
            txtUsername.Size = new Size(250, 30);
            txtUsername.Style = MetroSet_UI.Enums.Style.Light;
            txtUsername.StyleManager = null;
            txtUsername.TabIndex = 2;
            txtUsername.TextAlign = HorizontalAlignment.Left;
            txtUsername.ThemeAuthor = "Narwin";
            txtUsername.ThemeName = "MetroLite";
            txtUsername.UseSystemPasswordChar = false;
            txtUsername.WatermarkText = "Enter username";
            // 
            // lblPassword
            // 
            lblPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblPassword.IsDerivedStyle = true;
            lblPassword.Location = new Point(41, 152);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(100, 23);
            lblPassword.Style = MetroSet_UI.Enums.Style.Light;
            lblPassword.StyleManager = null;
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Password:";
            lblPassword.ThemeAuthor = "Narwin";
            lblPassword.ThemeName = "MetroLite";
            // 
            // txtPassword
            // 
            txtPassword.AutoCompleteCustomSource = null;
            txtPassword.AutoCompleteMode = AutoCompleteMode.None;
            txtPassword.AutoCompleteSource = AutoCompleteSource.None;
            txtPassword.BorderColor = Color.FromArgb(155, 155, 155);
            txtPassword.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtPassword.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtPassword.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtPassword.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtPassword.HoverColor = Color.FromArgb(102, 102, 102);
            txtPassword.Image = null;
            txtPassword.IsDerivedStyle = true;
            txtPassword.Lines = null;
            txtPassword.Location = new Point(141, 152);
            txtPassword.MaxLength = 32767;
            txtPassword.Multiline = false;
            txtPassword.Name = "txtPassword";
            txtPassword.ReadOnly = false;
            txtPassword.Size = new Size(250, 30);
            txtPassword.Style = MetroSet_UI.Enums.Style.Light;
            txtPassword.StyleManager = null;
            txtPassword.TabIndex = 4;
            txtPassword.TextAlign = HorizontalAlignment.Left;
            txtPassword.ThemeAuthor = "Narwin";
            txtPassword.ThemeName = "MetroLite";
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.WatermarkText = "Enter password";
            txtPassword.KeyPressed += txtPassword_KeyPressed;
            // 
            // btnLogin
            // 
            btnLogin.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnLogin.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnLogin.DisabledForeColor = Color.Gray;
            btnLogin.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnLogin.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnLogin.HoverColor = Color.FromArgb(95, 207, 255);
            btnLogin.HoverTextColor = Color.White;
            btnLogin.IsDerivedStyle = true;
            btnLogin.Location = new Point(157, 235);
            btnLogin.Name = "btnLogin";
            btnLogin.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnLogin.NormalColor = Color.FromArgb(65, 177, 225);
            btnLogin.NormalTextColor = Color.White;
            btnLogin.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnLogin.PressColor = Color.FromArgb(35, 147, 195);
            btnLogin.PressTextColor = Color.White;
            btnLogin.Size = new Size(100, 40);
            btnLogin.Style = MetroSet_UI.Enums.Style.Light;
            btnLogin.StyleManager = null;
            btnLogin.TabIndex = 5;
            btnLogin.Text = "Login";
            btnLogin.ThemeAuthor = "Narwin";
            btnLogin.ThemeName = "MetroLite";
            btnLogin.Click += btnLogin_Click;
            // 
            // metroSetControlBox1
            // 
            metroSetControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            metroSetControlBox1.CloseHoverBackColor = Color.FromArgb(183, 40, 40);
            metroSetControlBox1.CloseHoverForeColor = Color.White;
            metroSetControlBox1.CloseNormalForeColor = Color.Gray;
            metroSetControlBox1.DisabledForeColor = Color.DimGray;
            metroSetControlBox1.IsDerivedStyle = true;
            metroSetControlBox1.Location = new Point(343, 7);
            metroSetControlBox1.MaximizeBox = false;
            metroSetControlBox1.MaximizeHoverBackColor = Color.FromArgb(238, 238, 238);
            metroSetControlBox1.MaximizeHoverForeColor = Color.Gray;
            metroSetControlBox1.MaximizeNormalForeColor = Color.Gray;
            metroSetControlBox1.MinimizeBox = false;
            metroSetControlBox1.MinimizeHoverBackColor = Color.FromArgb(238, 238, 238);
            metroSetControlBox1.MinimizeHoverForeColor = Color.Gray;
            metroSetControlBox1.MinimizeNormalForeColor = Color.Gray;
            metroSetControlBox1.Name = "metroSetControlBox1";
            metroSetControlBox1.Size = new Size(100, 25);
            metroSetControlBox1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetControlBox1.StyleManager = null;
            metroSetControlBox1.TabIndex = 6;
            metroSetControlBox1.Text = "metroSetControlBox1";
            metroSetControlBox1.ThemeAuthor = "Narwin";
            metroSetControlBox1.ThemeName = "MetroLite";
            metroSetControlBox1.Click += metroSetControlBox1_Click;
            // 
            // LoginForm
            // 
            ClientSize = new Size(445, 300);
            Controls.Add(metroSetControlBox1);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            MinimumSize = new Size(400, 300);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ThemeName = "MetroDark";
            ResumeLayout(false);
        }

        public string Username
        {
            get { return txtUsername.Text; }
            set { Username = value; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
            set { Password = value; }
        }

        LoginServices loginServices = new LoginServices();


        private async void btnLogin_Click(object sender, System.EventArgs e)
        {
            string username = Username;
            string password = Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                new ToastMessageForm("Please enter both username and password").Show();
                return;
            }

            var loginService = new LoginServices();

            bool loginSuccessful = await loginService.LoginAsync(username, password);

            ValidateLogin(loginSuccessful);
        }

        private async void ValidateLogin(bool loginSuccessful)
        {
            if (loginSuccessful)
            {
                await Task.Delay(1000);
                new ToastMessageForm("Login successful!").Show();
                // Hide login form
                this.Hide();

                await Task.Delay(1000);

                // Close any existing MainForm
                var existingMainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
                if (existingMainForm != null)
                {
                    existingMainForm.Close();
                }

                // Create a new MainForm
                MainForm mainForm = new MainForm();
                mainForm.FormClosed += (s, e) => this.Show(); // Optional: return to login when main closes
                mainForm.Show();

                
            }
        }
        private MetroSetControlBox metroSetControlBox1;
    }

    #endregion
}
