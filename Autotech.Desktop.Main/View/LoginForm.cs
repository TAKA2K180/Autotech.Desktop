using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Models;
using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autotech.Desktop.Main.View
{
    public partial class LoginForm : MetroSetForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void txtPassword_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // prevent beep sound
                PerformLogin();   // or your custom logic here
            }
        }

        private async void PerformLogin()
        {
            string username = Username;
            string password = Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MetroSetMessageBox.Show(this, "Please enter both username and password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var loginService = new LoginServices();

            bool loginSuccessful = await loginService.LoginAsync(username, password);
            LoginHelper.isLoggedIn = loginSuccessful;
            ValidateLogin(loginSuccessful);
            txtUsername.Text = "";
            txtPassword.Text = "";
            this.Hide();
        }
    }
}
