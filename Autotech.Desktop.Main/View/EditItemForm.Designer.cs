namespace Autotech.Desktop.Main.View
{
    partial class EditItemForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private TextBox txtItemCode;
        private TextBox txtItemName;
        private TextBox txtDescription;
        private TextBox txtOnHand;
        private TextBox txtBataanRetail;
        private TextBox txtBataanWholesale;
        private TextBox txtPampangaRetail;
        private TextBox txtPampangaWholesale;
        private TextBox txtZambalesRetail;
        private TextBox txtZambalesWholesale;
        private Button btnSave;

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
            txtItemCode = new TextBox();
            txtItemName = new TextBox();
            txtDescription = new TextBox();
            txtOnHand = new TextBox();
            txtBataanRetail = new TextBox();
            txtBataanWholesale = new TextBox();
            txtPampangaRetail = new TextBox();
            txtPampangaWholesale = new TextBox();
            txtZambalesRetail = new TextBox();
            txtZambalesWholesale = new TextBox();
            btnSave = new Button();
            lblItemCode = new Label();
            lblItemName = new Label();
            lblDescription = new Label();
            lblOnHand = new Label();
            lblBataanRetail = new Label();
            lblBataanWholesale = new Label();
            lblPampangaRetail = new Label();
            lblPampangaWholesale = new Label();
            lblZambalesRetail = new Label();
            lblZambalesWholesale = new Label();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // txtItemCode
            // 
            txtItemCode.Location = new Point(156, 88);
            txtItemCode.Name = "txtItemCode";
            txtItemCode.Size = new Size(200, 27);
            txtItemCode.TabIndex = 1;
            // 
            // txtItemName
            // 
            txtItemName.Location = new Point(156, 118);
            txtItemName.Name = "txtItemName";
            txtItemName.Size = new Size(200, 27);
            txtItemName.TabIndex = 3;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(156, 148);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(200, 27);
            txtDescription.TabIndex = 5;
            // 
            // txtOnHand
            // 
            txtOnHand.Location = new Point(156, 178);
            txtOnHand.Name = "txtOnHand";
            txtOnHand.Size = new Size(200, 27);
            txtOnHand.TabIndex = 7;
            // 
            // txtBataanRetail
            // 
            txtBataanRetail.Location = new Point(156, 228);
            txtBataanRetail.Name = "txtBataanRetail";
            txtBataanRetail.Size = new Size(200, 27);
            txtBataanRetail.TabIndex = 9;
            // 
            // txtBataanWholesale
            // 
            txtBataanWholesale.Location = new Point(156, 288);
            txtBataanWholesale.Name = "txtBataanWholesale";
            txtBataanWholesale.Size = new Size(200, 27);
            txtBataanWholesale.TabIndex = 11;
            // 
            // txtPampangaRetail
            // 
            txtPampangaRetail.Location = new Point(156, 358);
            txtPampangaRetail.Name = "txtPampangaRetail";
            txtPampangaRetail.Size = new Size(200, 27);
            txtPampangaRetail.TabIndex = 13;
            // 
            // txtPampangaWholesale
            // 
            txtPampangaWholesale.Location = new Point(156, 421);
            txtPampangaWholesale.Name = "txtPampangaWholesale";
            txtPampangaWholesale.Size = new Size(200, 27);
            txtPampangaWholesale.TabIndex = 15;
            // 
            // txtZambalesRetail
            // 
            txtZambalesRetail.Location = new Point(156, 485);
            txtZambalesRetail.Name = "txtZambalesRetail";
            txtZambalesRetail.Size = new Size(200, 27);
            txtZambalesRetail.TabIndex = 17;
            // 
            // txtZambalesWholesale
            // 
            txtZambalesWholesale.Location = new Point(156, 547);
            txtZambalesWholesale.Name = "txtZambalesWholesale";
            txtZambalesWholesale.Size = new Size(200, 27);
            txtZambalesWholesale.TabIndex = 19;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(46, 625);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(109, 46);
            btnSave.TabIndex = 20;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // lblItemCode
            // 
            lblItemCode.Location = new Point(26, 88);
            lblItemCode.Name = "lblItemCode";
            lblItemCode.Size = new Size(120, 20);
            lblItemCode.TabIndex = 0;
            lblItemCode.Text = "Item Code:";
            // 
            // lblItemName
            // 
            lblItemName.Location = new Point(26, 118);
            lblItemName.Name = "lblItemName";
            lblItemName.Size = new Size(120, 20);
            lblItemName.TabIndex = 2;
            lblItemName.Text = "Item Name:";
            // 
            // lblDescription
            // 
            lblDescription.Location = new Point(26, 148);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(120, 20);
            lblDescription.TabIndex = 4;
            lblDescription.Text = "Description:";
            // 
            // lblOnHand
            // 
            lblOnHand.Location = new Point(26, 178);
            lblOnHand.Name = "lblOnHand";
            lblOnHand.Size = new Size(120, 20);
            lblOnHand.TabIndex = 6;
            lblOnHand.Text = "On Hand:";
            // 
            // lblBataanRetail
            // 
            lblBataanRetail.Location = new Point(26, 208);
            lblBataanRetail.Name = "lblBataanRetail";
            lblBataanRetail.Size = new Size(120, 47);
            lblBataanRetail.TabIndex = 8;
            lblBataanRetail.Text = "Bataan Retail:";
            // 
            // lblBataanWholesale
            // 
            lblBataanWholesale.Location = new Point(26, 266);
            lblBataanWholesale.Name = "lblBataanWholesale";
            lblBataanWholesale.Size = new Size(120, 49);
            lblBataanWholesale.TabIndex = 10;
            lblBataanWholesale.Text = "Bataan Wholesale:";
            // 
            // lblPampangaRetail
            // 
            lblPampangaRetail.Location = new Point(26, 328);
            lblPampangaRetail.Name = "lblPampangaRetail";
            lblPampangaRetail.Size = new Size(120, 57);
            lblPampangaRetail.TabIndex = 12;
            lblPampangaRetail.Text = "Pampanga Retail:";
            // 
            // lblPampangaWholesale
            // 
            lblPampangaWholesale.Location = new Point(16, 399);
            lblPampangaWholesale.Name = "lblPampangaWholesale";
            lblPampangaWholesale.Size = new Size(130, 49);
            lblPampangaWholesale.TabIndex = 14;
            lblPampangaWholesale.Text = "Pampanga Wholesale:";
            // 
            // lblZambalesRetail
            // 
            lblZambalesRetail.Location = new Point(26, 462);
            lblZambalesRetail.Name = "lblZambalesRetail";
            lblZambalesRetail.Size = new Size(120, 50);
            lblZambalesRetail.TabIndex = 16;
            lblZambalesRetail.Text = "Zambales Retail:";
            // 
            // lblZambalesWholesale
            // 
            lblZambalesWholesale.Location = new Point(16, 524);
            lblZambalesWholesale.Name = "lblZambalesWholesale";
            lblZambalesWholesale.Size = new Size(130, 50);
            lblZambalesWholesale.TabIndex = 18;
            lblZambalesWholesale.Text = "Zambales Wholesale:";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(199, 625);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(109, 46);
            btnCancel.TabIndex = 21;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // EditItemForm
            // 
            ClientSize = new Size(387, 685);
            Controls.Add(btnCancel);
            Controls.Add(lblItemCode);
            Controls.Add(txtItemCode);
            Controls.Add(lblItemName);
            Controls.Add(txtItemName);
            Controls.Add(lblDescription);
            Controls.Add(txtDescription);
            Controls.Add(lblOnHand);
            Controls.Add(txtOnHand);
            Controls.Add(lblBataanRetail);
            Controls.Add(txtBataanRetail);
            Controls.Add(lblBataanWholesale);
            Controls.Add(txtBataanWholesale);
            Controls.Add(lblPampangaRetail);
            Controls.Add(txtPampangaRetail);
            Controls.Add(lblPampangaWholesale);
            Controls.Add(txtPampangaWholesale);
            Controls.Add(lblZambalesRetail);
            Controls.Add(txtZambalesRetail);
            Controls.Add(lblZambalesWholesale);
            Controls.Add(txtZambalesWholesale);
            Controls.Add(btnSave);
            Name = "EditItemForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Edit Item";
            Load += EditItemForm_Load;
            ResumeLayout(false);
            PerformLayout();
            //this.Load += new EventHandler(this.EditItemForm_Load);
        }



        #endregion

        private Label lblItemCode;
        private Label lblItemName;
        private Label lblDescription;
        private Label lblOnHand;
        private Label lblBataanRetail;
        private Label lblBataanWholesale;
        private Label lblPampangaRetail;
        private Label lblPampangaWholesale;
        private Label lblZambalesRetail;
        private Label lblZambalesWholesale;
        private Button btnCancel;
    }
}
