using MetroSet_UI.Controls;

namespace Autotech.Desktop.Main.View
{
    partial class SplashForm
    {
        private MetroSetLabel lblWelcome;
        private PictureBox logoPictureBox;
        private System.Windows.Forms.Timer splashTimer;
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
            // Set up the form
            this.Text = "Initializing...";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None; // Remove the border to give a splash screen look.
            this.Style = MetroSet_UI.Enums.Style.Light;
            this.ThemeName = "MetroLite";

            // Set up the logo picture box (optional)
            logoPictureBox = new PictureBox();
            logoPictureBox.Size = new Size(100, 100);
            logoPictureBox.Location = new Point(150, 50);
            logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            logoPictureBox.Image = Image.FromFile("logo.jpg"); // Change to your logo path
            this.Controls.Add(logoPictureBox);

            // Set up the Welcome Label
            lblWelcome = new MetroSetLabel();
            lblWelcome.Text = "Welcome to Autotech!";
            lblWelcome.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            lblWelcome.Location = new Point(50, 170);
            lblWelcome.Size = new Size(300, 50);
            lblWelcome.Style = MetroSet_UI.Enums.Style.Light;
            lblWelcome.ThemeName = "MetroLite";
            this.Controls.Add(lblWelcome);

            // Set up the Timer for splash screen duration
            splashTimer = new System.Windows.Forms.Timer();
            splashTimer.Interval = 3000; // 3 seconds (3000 milliseconds)
            splashTimer.Tick += SplashTimer_Tick;
            splashTimer.Start();
        }

        private void SplashTimer_Tick(object sender, EventArgs e)
        {
            splashTimer.Stop();
            this.Hide(); // Hide the splash screen

            // Open the main form after the splash screen
            MainForm mainForm = new MainForm();
            mainForm.Show();

            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        // Optional: To stop closing when ESC is pressed
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                return true; // Ignore Escape key
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    #endregion
}