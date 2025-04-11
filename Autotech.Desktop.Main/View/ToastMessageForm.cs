using System;
using System.Drawing;
using System.Windows.Forms;

namespace Autotech.Desktop.Main.View
{
    public partial class ToastMessageForm : Form
    {
        private System.Windows.Forms.Timer closeTimer;

        public ToastMessageForm(string message)
        {
            this.InitializeComponent();

            // Style the toast
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.Opacity = 0.95;
            this.Padding = new Padding(10);

            // Create label
            Label lblMessage = new Label()
            {
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15, FontStyle.Regular),
                Text = message,
                MaximumSize = new Size(300, 0)
            };

            this.Controls.Add(lblMessage);
            this.AutoSize = true;

            // Position toast at bottom-right of screen
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(screen.Width / 2 - this.Width / 2, screen.Height / 2 - this.Height / 2);

            // Auto-close after 1 second
            closeTimer = new System.Windows.Forms.Timer();
            closeTimer.Interval = 1000;
            closeTimer.Tick += (s, e) =>
            {
                closeTimer.Stop();
                this.Close();
            };
            closeTimer.Start();
        }

        // You can leave InitializeComponent empty
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // ToastMessageForm
            // 
            ClientSize = new Size(300, 60);
            ControlBox = false;
            MaximizeBox = false;
            Name = "ToastMessageForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
