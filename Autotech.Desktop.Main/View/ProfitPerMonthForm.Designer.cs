namespace Autotech.Desktop.Main.View
{
    partial class ProfitPerMonthForm
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
            label1 = new Label();
            cmbMonth = new ComboBox();
            label2 = new Label();
            cmbYear = new ComboBox();
            btnCalculate = new Button();
            btnExport = new Button();
            button2 = new Button();
            dtgResult = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dtgResult).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(26, 90);
            label1.Name = "label1";
            label1.Size = new Size(127, 27);
            label1.TabIndex = 0;
            label1.Text = "Select Month:";
            // 
            // cmbMonth
            // 
            cmbMonth.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMonth.Items.AddRange(new object[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" });
            cmbMonth.Location = new Point(159, 90);
            cmbMonth.Name = "cmbMonth";
            cmbMonth.Size = new Size(186, 28);
            cmbMonth.TabIndex = 1;
            // 
            // label2
            // 
            label2.Location = new Point(26, 151);
            label2.Name = "label2";
            label2.Size = new Size(127, 27);
            label2.TabIndex = 2;
            label2.Text = "Select Year:";
            // 
            // cmbYear
            // 
            cmbYear.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbYear.Location = new Point(159, 150);
            cmbYear.Name = "cmbYear";
            cmbYear.Size = new Size(186, 28);
            cmbYear.TabIndex = 3;
            // 
            // btnCalculate
            // 
            btnCalculate.Location = new Point(26, 204);
            btnCalculate.Name = "btnCalculate";
            btnCalculate.Size = new Size(153, 44);
            btnCalculate.TabIndex = 4;
            btnCalculate.Text = "Calculate Profit";
            btnCalculate.Click += btnCalculate_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(26, 565);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(153, 44);
            btnExport.TabIndex = 6;
            btnExport.Text = "Export to excel";
            btnExport.Click += btnExport_Click;
            // 
            // button2
            // 
            button2.Location = new Point(533, 10);
            button2.Name = "button2";
            button2.Size = new Size(75, 41);
            button2.TabIndex = 7;
            button2.Text = "Close";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // dtgResult
            // 
            dtgResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgResult.Location = new Point(26, 268);
            dtgResult.Name = "dtgResult";
            dtgResult.ReadOnly = true;
            dtgResult.RowHeadersVisible = false;
            dtgResult.RowTemplate.Height = 25;
            dtgResult.Size = new Size(572, 276);
            dtgResult.TabIndex = 8;
            // 
            // ProfitPerMonthForm
            // 
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderColor = Color.Black;
            BorderThickness = 5F;
            ClientSize = new Size(623, 621);
            Controls.Add(dtgResult);
            Controls.Add(button2);
            Controls.Add(btnExport);
            Controls.Add(label1);
            Controls.Add(cmbMonth);
            Controls.Add(label2);
            Controls.Add(cmbYear);
            Controls.Add(btnCalculate);
            Name = "ProfitPerMonthForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Profit Per Month";
            Load += ProfitPerMonthForm_Load;
            ((System.ComponentModel.ISupportInitialize)dtgResult).EndInit();
            ResumeLayout(false);
        }
        private Label label1;
        private ComboBox cmbMonth;
        private Label label2;
        private ComboBox cmbYear;
        private Button btnCalculate;
        private TextBox txtResult;

        #endregion

        private Button btnExport;
        private Button button2;
        private DataGridView dtgResult;
    }
}