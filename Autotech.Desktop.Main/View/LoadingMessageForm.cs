using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Autotech.Desktop.Main.View
{
    public partial class LoadingMessageForm : Form
    {
        private System.Windows.Forms.Timer closeTimer;

        public LoadingMessageForm(string message)
        {
            this.InitializeComponent();

            // Style the form
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.Opacity = 0.95;
            this.Padding = new Padding(10);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;


            // Create message label
            Label lblMessage = new Label()
            {
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15, FontStyle.Regular),
                Text = message,
                MaximumSize = new Size(300, 0),
                Dock = DockStyle.Fill
            };

            // Layout with horizontal flow
            FlowLayoutPanel layout = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(10),
                BackColor = Color.Transparent
            };

            this.Controls.Add(layout);

            // Center form manually
            this.Load += (s, e) =>
            {
                var screen = Screen.PrimaryScreen.WorkingArea;
                this.Location = new Point(
                    screen.Width / 2 - this.Width / 2,
                    screen.Height / 2 - this.Height / 2);
            };

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

        // Minimal initializer
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingMessageForm));
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.WhiteSmoke;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(2, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(130, 133);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // LoadingMessageForm
            // 
            BackColor = Color.White;
            ClientSize = new Size(134, 134);
            ControlBox = false;
            Controls.Add(pictureBox1);
            MaximizeBox = false;
            Name = "LoadingMessageForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Load += LoadingMessageForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        private void LoadingMessageForm_Load(object sender, EventArgs e)
        {

        }
    }
}
