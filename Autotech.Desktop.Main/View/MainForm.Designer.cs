﻿using MetroSet_UI.Controls;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using MetroSet_UI.Enums;

namespace Autotech.Desktop.Main.View
{
    partial class MainForm
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
        // Declare the controls at the class level
        private MetroSetTabControl metroSetTabControl1;
        private TabPage tabPageSales;
        private MetroSetLabel lblSalesInfo;
        private PictureBox logoPictureBox;
        private MetroSetLabel lblCashier;
        private MetroSetTextBox txtCashier;
        private MetroSetLabel lblAccount;
        private MetroSetComboBox comboAccount;
        private MetroSetTextBox txtContactNumber;
        private MetroSetLabel lblContactNumber;
        private MetroSetLabel lblTerms;
        private MetroSetTextBox txtTerms;
        private MetroSetButton btnAddToCart;
        private MetroSetButton btnRemoveItem;
        private MetroSetButton btnEmptyCart;
        private DataGridView dataGridViewItemList;
        private DataGridView dataGridViewOrderCart;
        private MetroSetPanel panelPayment;
        private MetroSetTextBox txtSearchItem;
        private MetroSetLabel lblOrderCart;
        private MetroSetLabel lblPaymentMethod;
        private MetroSetComboBox comboPaymentMethod;
        private MetroSetLabel lblPaidAmount;
        private MetroSetTextBox txtPaidAmount;
        private MetroSetLabel lblChange;
        private MetroSetTextBox txtChange;
        private MetroSetButton btnNext;
        private MetroSetButton btnPrint;
        private MetroSetButton btnPay;
        private MetroSetLabel lblSubtotal;
        private MetroSetTextBox txtSubtotal;
        private MetroSetLabel lblTax;
        private MetroSetTextBox txtTax;
        private MetroSetLabel lblDiscount;
        private MetroSetTextBox txtDiscount;
        private MetroSetLabel lblTotal;
        private MetroSetTextBox txtTotal;
        private MetroSetRadioButton radioBataan;
        private MetroSetRadioButton radioPampanga;
        private MetroSetRadioButton radioZambales;
        private MetroSetRadioButton radioRetail;
        private MetroSetRadioButton radioWholesale;
        private void InitializeComponent()
        {
            metroSetTabControl1 = new MetroSetTabControl();
            tabPageSales = new TabPage();
            logoPictureBox = new PictureBox();
            lblCashier = new MetroSetLabel();
            txtCashier = new MetroSetTextBox();
            lblAccount = new MetroSetLabel();
            comboAccount = new MetroSetComboBox();
            lblContactNumber = new MetroSetLabel();
            txtContactNumber = new MetroSetTextBox();
            lblTerms = new MetroSetLabel();
            txtTerms = new MetroSetTextBox();
            btnAddToCart = new MetroSetButton();
            btnRemoveItem = new MetroSetButton();
            btnEmptyCart = new MetroSetButton();
            dataGridViewItemList = new DataGridView();
            dataGridViewOrderCart = new DataGridView();
            lblOrderCart = new MetroSetLabel();
            txtSearchItem = new MetroSetTextBox();
            radioBataan = new MetroSetRadioButton();
            radioPampanga = new MetroSetRadioButton();
            radioZambales = new MetroSetRadioButton();
            radioRetail = new MetroSetRadioButton();
            radioWholesale = new MetroSetRadioButton();
            panelPayment = new MetroSetPanel();
            lblPaymentMethod = new MetroSetLabel();
            comboPaymentMethod = new MetroSetComboBox();
            lblPaidAmount = new MetroSetLabel();
            txtPaidAmount = new MetroSetTextBox();
            lblChange = new MetroSetLabel();
            txtChange = new MetroSetTextBox();
            btnNext = new MetroSetButton();
            btnPrint = new MetroSetButton();
            btnPay = new MetroSetButton();
            lblSubtotal = new MetroSetLabel();
            txtSubtotal = new MetroSetTextBox();
            lblTax = new MetroSetLabel();
            txtTax = new MetroSetTextBox();
            lblDiscount = new MetroSetLabel();
            txtDiscount = new MetroSetTextBox();
            lblTotal = new MetroSetLabel();
            txtTotal = new MetroSetTextBox();
            lblSalesInfo = new MetroSetLabel();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            metroSetControlBox1 = new MetroSetControlBox();
            btnLogout = new MetroSetButton();
            metroSetTabControl1.SuspendLayout();
            tabPageSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewItemList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewOrderCart).BeginInit();
            panelPayment.SuspendLayout();
            SuspendLayout();
            // 
            // metroSetTabControl1
            // 
            metroSetTabControl1.AnimateEasingType = EasingType.CubeOut;
            metroSetTabControl1.AnimateTime = 200;
            metroSetTabControl1.BackgroundColor = Color.White;
            metroSetTabControl1.Controls.Add(tabPageSales);
            metroSetTabControl1.Controls.Add(tabPage1);
            metroSetTabControl1.Controls.Add(tabPage2);
            metroSetTabControl1.Controls.Add(tabPage3);
            metroSetTabControl1.Dock = DockStyle.Top;
            metroSetTabControl1.IsDerivedStyle = true;
            metroSetTabControl1.ItemSize = new Size(100, 38);
            metroSetTabControl1.Location = new Point(12, 70);
            metroSetTabControl1.Name = "metroSetTabControl1";
            metroSetTabControl1.SelectedIndex = 0;
            metroSetTabControl1.SelectedTextColor = Color.White;
            metroSetTabControl1.Size = new Size(1312, 767);
            metroSetTabControl1.SizeMode = TabSizeMode.Fixed;
            metroSetTabControl1.Speed = 100;
            metroSetTabControl1.Style = Style.Light;
            metroSetTabControl1.StyleManager = null;
            metroSetTabControl1.TabIndex = 0;
            metroSetTabControl1.ThemeAuthor = "Narwin";
            metroSetTabControl1.ThemeName = "MetroLite";
            metroSetTabControl1.UnselectedTextColor = Color.Gray;
            metroSetTabControl1.UseAnimation = false;
            // 
            // tabPageSales
            // 
            tabPageSales.Controls.Add(logoPictureBox);
            tabPageSales.Controls.Add(lblCashier);
            tabPageSales.Controls.Add(txtCashier);
            tabPageSales.Controls.Add(lblAccount);
            tabPageSales.Controls.Add(comboAccount);
            tabPageSales.Controls.Add(lblContactNumber);
            tabPageSales.Controls.Add(txtContactNumber);
            tabPageSales.Controls.Add(lblTerms);
            tabPageSales.Controls.Add(txtTerms);
            tabPageSales.Controls.Add(btnAddToCart);
            tabPageSales.Controls.Add(btnRemoveItem);
            tabPageSales.Controls.Add(btnEmptyCart);
            tabPageSales.Controls.Add(dataGridViewItemList);
            tabPageSales.Controls.Add(dataGridViewOrderCart);
            tabPageSales.Controls.Add(lblOrderCart);
            tabPageSales.Controls.Add(txtSearchItem);
            tabPageSales.Controls.Add(radioBataan);
            tabPageSales.Controls.Add(radioPampanga);
            tabPageSales.Controls.Add(radioZambales);
            tabPageSales.Controls.Add(radioRetail);
            tabPageSales.Controls.Add(radioWholesale);
            tabPageSales.Controls.Add(panelPayment);
            tabPageSales.Controls.Add(lblSalesInfo);
            tabPageSales.ForeColor = Color.WhiteSmoke;
            tabPageSales.Location = new Point(4, 42);
            tabPageSales.Name = "tabPageSales";
            tabPageSales.Padding = new Padding(10);
            tabPageSales.Size = new Size(1304, 721);
            tabPageSales.TabIndex = 0;
            tabPageSales.Text = "Sales";
            tabPageSales.UseVisualStyleBackColor = true;
            // 
            // logoPictureBox
            // 
            logoPictureBox.Location = new Point(10, 10);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(80, 80);
            logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            logoPictureBox.TabIndex = 1;
            logoPictureBox.TabStop = false;
            logoPictureBox.Image = System.Drawing.Image.FromFile("logo.jpg");
            // 
            // lblCashier
            // 
            lblCashier.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblCashier.IsDerivedStyle = true;
            lblCashier.Location = new Point(320, 10);
            lblCashier.Name = "lblCashier";
            lblCashier.Size = new Size(60, 25);
            lblCashier.Style = Style.Light;
            lblCashier.StyleManager = null;
            lblCashier.TabIndex = 2;
            lblCashier.Text = "Cashier:";
            lblCashier.ThemeAuthor = "Narwin";
            lblCashier.ThemeName = "MetroLite";
            // 
            // txtCashier
            // 
            txtCashier.AutoCompleteCustomSource = null;
            txtCashier.AutoCompleteMode = AutoCompleteMode.None;
            txtCashier.AutoCompleteSource = AutoCompleteSource.None;
            txtCashier.BorderColor = Color.FromArgb(155, 155, 155);
            txtCashier.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtCashier.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtCashier.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtCashier.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtCashier.HoverColor = Color.FromArgb(102, 102, 102);
            txtCashier.Image = null;
            txtCashier.IsDerivedStyle = true;
            txtCashier.Lines = null;
            txtCashier.Location = new Point(380, 10);
            txtCashier.MaxLength = 32767;
            txtCashier.Multiline = false;
            txtCashier.Name = "txtCashier";
            txtCashier.ReadOnly = false;
            txtCashier.Size = new Size(150, 25);
            txtCashier.Style = Style.Light;
            txtCashier.StyleManager = null;
            txtCashier.TabIndex = 3;
            txtCashier.TextAlign = HorizontalAlignment.Left;
            txtCashier.ThemeAuthor = "Narwin";
            txtCashier.ThemeName = "MetroLite";
            txtCashier.UseSystemPasswordChar = false;
            txtCashier.WatermarkText = "";
            // 
            // lblAccount
            // 
            lblAccount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblAccount.IsDerivedStyle = true;
            lblAccount.Location = new Point(889, 22);
            lblAccount.Name = "lblAccount";
            lblAccount.Size = new Size(65, 25);
            lblAccount.Style = Style.Light;
            lblAccount.StyleManager = null;
            lblAccount.TabIndex = 4;
            lblAccount.Text = "Account:";
            lblAccount.ThemeAuthor = "Narwin";
            lblAccount.ThemeName = "MetroLite";
            // 
            // comboAccount
            // 
            comboAccount.AllowDrop = true;
            comboAccount.ArrowColor = Color.FromArgb(150, 150, 150);
            comboAccount.BackColor = Color.Transparent;
            comboAccount.BackgroundColor = Color.FromArgb(238, 238, 238);
            comboAccount.BorderColor = Color.FromArgb(150, 150, 150);
            comboAccount.CausesValidation = false;
            comboAccount.DisabledBackColor = Color.FromArgb(204, 204, 204);
            comboAccount.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            comboAccount.DisabledForeColor = Color.FromArgb(136, 136, 136);
            comboAccount.DrawMode = DrawMode.OwnerDrawFixed;
            comboAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            comboAccount.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point);
            comboAccount.IsDerivedStyle = true;
            comboAccount.ItemHeight = 20;
            comboAccount.Location = new Point(1010, 10);
            comboAccount.Name = "comboAccount";
            comboAccount.SelectedItemBackColor = Color.FromArgb(65, 177, 225);
            comboAccount.SelectedItemForeColor = Color.White;
            comboAccount.Size = new Size(200, 26);
            comboAccount.Style = Style.Light;
            comboAccount.StyleManager = null;
            comboAccount.TabIndex = 5;
            comboAccount.ThemeAuthor = "Narwin";
            comboAccount.ThemeName = "MetroLite";
            // 
            // lblContactNumber
            // 
            lblContactNumber.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblContactNumber.IsDerivedStyle = true;
            lblContactNumber.Location = new Point(889, 47);
            lblContactNumber.Name = "lblContactNumber";
            lblContactNumber.Size = new Size(120, 25);
            lblContactNumber.Style = Style.Light;
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
            txtContactNumber.Location = new Point(1010, 41);
            txtContactNumber.MaxLength = 32767;
            txtContactNumber.Multiline = false;
            txtContactNumber.Name = "txtContactNumber";
            txtContactNumber.ReadOnly = false;
            txtContactNumber.Size = new Size(150, 25);
            txtContactNumber.Style = Style.Light;
            txtContactNumber.StyleManager = null;
            txtContactNumber.TabIndex = 7;
            txtContactNumber.TextAlign = HorizontalAlignment.Left;
            txtContactNumber.ThemeAuthor = "Narwin";
            txtContactNumber.ThemeName = "MetroLite";
            txtContactNumber.UseSystemPasswordChar = false;
            txtContactNumber.WatermarkText = "";
            // 
            // lblTerms
            // 
            lblTerms.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTerms.IsDerivedStyle = true;
            lblTerms.Location = new Point(1180, 49);
            lblTerms.Name = "lblTerms";
            lblTerms.Size = new Size(53, 23);
            lblTerms.Style = Style.Light;
            lblTerms.StyleManager = null;
            lblTerms.TabIndex = 8;
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
            txtTerms.Location = new Point(1239, 43);
            txtTerms.MaxLength = 32767;
            txtTerms.Multiline = false;
            txtTerms.Name = "txtTerms";
            txtTerms.ReadOnly = false;
            txtTerms.Size = new Size(50, 25);
            txtTerms.Style = Style.Light;
            txtTerms.StyleManager = null;
            txtTerms.TabIndex = 9;
            txtTerms.Text = "30";
            txtTerms.TextAlign = HorizontalAlignment.Left;
            txtTerms.ThemeAuthor = "Narwin";
            txtTerms.ThemeName = "MetroLite";
            txtTerms.UseSystemPasswordChar = false;
            txtTerms.WatermarkText = "";
            // 
            // btnAddToCart
            // 
            btnAddToCart.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnAddToCart.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnAddToCart.DisabledForeColor = Color.Gray;
            btnAddToCart.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnAddToCart.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnAddToCart.HoverColor = Color.FromArgb(95, 207, 255);
            btnAddToCart.HoverTextColor = Color.White;
            btnAddToCart.IsDerivedStyle = true;
            btnAddToCart.Location = new Point(827, 175);
            btnAddToCart.Name = "btnAddToCart";
            btnAddToCart.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnAddToCart.NormalColor = Color.FromArgb(65, 177, 225);
            btnAddToCart.NormalTextColor = Color.White;
            btnAddToCart.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnAddToCart.PressColor = Color.FromArgb(35, 147, 195);
            btnAddToCart.PressTextColor = Color.White;
            btnAddToCart.Size = new Size(50, 50);
            btnAddToCart.Style = Style.Light;
            btnAddToCart.StyleManager = null;
            btnAddToCart.TabIndex = 10;
            btnAddToCart.Text = "+";
            btnAddToCart.ThemeAuthor = "Narwin";
            btnAddToCart.ThemeName = "MetroLite";
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnRemoveItem.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnRemoveItem.DisabledForeColor = Color.Gray;
            btnRemoveItem.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnRemoveItem.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnRemoveItem.HoverColor = Color.FromArgb(95, 207, 255);
            btnRemoveItem.HoverTextColor = Color.White;
            btnRemoveItem.IsDerivedStyle = true;
            btnRemoveItem.Location = new Point(827, 235);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnRemoveItem.NormalColor = Color.FromArgb(65, 177, 225);
            btnRemoveItem.NormalTextColor = Color.White;
            btnRemoveItem.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnRemoveItem.PressColor = Color.FromArgb(35, 147, 195);
            btnRemoveItem.PressTextColor = Color.White;
            btnRemoveItem.Size = new Size(50, 50);
            btnRemoveItem.Style = Style.Light;
            btnRemoveItem.StyleManager = null;
            btnRemoveItem.TabIndex = 11;
            btnRemoveItem.Text = "-";
            btnRemoveItem.ThemeAuthor = "Narwin";
            btnRemoveItem.ThemeName = "MetroLite";
            // 
            // btnEmptyCart
            // 
            btnEmptyCart.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnEmptyCart.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnEmptyCart.DisabledForeColor = Color.Gray;
            btnEmptyCart.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnEmptyCart.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnEmptyCart.HoverColor = Color.FromArgb(95, 207, 255);
            btnEmptyCart.HoverTextColor = Color.White;
            btnEmptyCart.IsDerivedStyle = true;
            btnEmptyCart.Location = new Point(827, 295);
            btnEmptyCart.Name = "btnEmptyCart";
            btnEmptyCart.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnEmptyCart.NormalColor = Color.FromArgb(65, 177, 225);
            btnEmptyCart.NormalTextColor = Color.White;
            btnEmptyCart.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnEmptyCart.PressColor = Color.FromArgb(35, 147, 195);
            btnEmptyCart.PressTextColor = Color.White;
            btnEmptyCart.Size = new Size(50, 50);
            btnEmptyCart.Style = Style.Light;
            btnEmptyCart.StyleManager = null;
            btnEmptyCart.TabIndex = 12;
            btnEmptyCart.Text = "Clear";
            btnEmptyCart.ThemeAuthor = "Narwin";
            btnEmptyCart.ThemeName = "MetroLite";
            // 
            // dataGridViewItemList
            // 
            dataGridViewItemList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewItemList.BackgroundColor = SystemColors.InactiveCaption;
            dataGridViewItemList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewItemList.Location = new Point(10, 100);
            dataGridViewItemList.Name = "dataGridViewItemList";
            dataGridViewItemList.Size = new Size(800, 400);
            dataGridViewItemList.TabIndex = 13;
            // 
            // dataGridViewOrderCart
            // 
            dataGridViewOrderCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewOrderCart.BackgroundColor = SystemColors.InactiveCaption;
            dataGridViewOrderCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewOrderCart.Location = new Point(889, 98);
            dataGridViewOrderCart.Name = "dataGridViewOrderCart";
            dataGridViewOrderCart.Size = new Size(400, 400);
            dataGridViewOrderCart.TabIndex = 14;
            // 
            // lblOrderCart
            // 
            lblOrderCart.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblOrderCart.IsDerivedStyle = true;
            lblOrderCart.Location = new Point(889, 72);
            lblOrderCart.Name = "lblOrderCart";
            lblOrderCart.Size = new Size(100, 25);
            lblOrderCart.Style = Style.Light;
            lblOrderCart.StyleManager = null;
            lblOrderCart.TabIndex = 15;
            lblOrderCart.Text = "Order Cart";
            lblOrderCart.ThemeAuthor = "Narwin";
            lblOrderCart.ThemeName = "MetroLite";
            // 
            // txtSearchItem
            // 
            txtSearchItem.AutoCompleteCustomSource = null;
            txtSearchItem.AutoCompleteMode = AutoCompleteMode.None;
            txtSearchItem.AutoCompleteSource = AutoCompleteSource.None;
            txtSearchItem.BorderColor = Color.FromArgb(155, 155, 155);
            txtSearchItem.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtSearchItem.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtSearchItem.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtSearchItem.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtSearchItem.HoverColor = Color.FromArgb(102, 102, 102);
            txtSearchItem.Image = null;
            txtSearchItem.IsDerivedStyle = true;
            txtSearchItem.Lines = null;
            txtSearchItem.Location = new Point(100, 65);
            txtSearchItem.MaxLength = 32767;
            txtSearchItem.Multiline = false;
            txtSearchItem.Name = "txtSearchItem";
            txtSearchItem.ReadOnly = false;
            txtSearchItem.Size = new Size(263, 25);
            txtSearchItem.Style = Style.Light;
            txtSearchItem.StyleManager = null;
            txtSearchItem.TabIndex = 17;
            txtSearchItem.TextAlign = HorizontalAlignment.Left;
            txtSearchItem.ThemeAuthor = "Narwin";
            txtSearchItem.ThemeName = "MetroLite";
            txtSearchItem.UseSystemPasswordChar = false;
            txtSearchItem.WatermarkText = "Search item...";
            // 
            // radioBataan
            // 
            radioBataan.BackgroundColor = Color.White;
            radioBataan.BorderColor = Color.FromArgb(155, 155, 155);
            radioBataan.Checked = false;
            radioBataan.CheckSignColor = Color.FromArgb(65, 177, 225);
            radioBataan.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            radioBataan.DisabledBorderColor = Color.FromArgb(205, 205, 205);
            radioBataan.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            radioBataan.Group = 0;
            radioBataan.IsDerivedStyle = true;
            radioBataan.Location = new Point(378, 70);
            radioBataan.Name = "radioBataan";
            radioBataan.Size = new Size(100, 17);
            radioBataan.Style = Style.Light;
            radioBataan.StyleManager = null;
            radioBataan.TabIndex = 18;
            radioBataan.Text = "Bataan";
            radioBataan.ThemeAuthor = "Narwin";
            radioBataan.ThemeName = "MetroLite";
            // 
            // radioPampanga
            // 
            radioPampanga.BackgroundColor = Color.White;
            radioPampanga.BorderColor = Color.FromArgb(155, 155, 155);
            radioPampanga.Checked = false;
            radioPampanga.CheckSignColor = Color.FromArgb(65, 177, 225);
            radioPampanga.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            radioPampanga.DisabledBorderColor = Color.FromArgb(205, 205, 205);
            radioPampanga.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            radioPampanga.Group = 0;
            radioPampanga.IsDerivedStyle = true;
            radioPampanga.Location = new Point(478, 70);
            radioPampanga.Name = "radioPampanga";
            radioPampanga.Size = new Size(100, 17);
            radioPampanga.Style = Style.Light;
            radioPampanga.StyleManager = null;
            radioPampanga.TabIndex = 19;
            radioPampanga.Text = "Pampanga";
            radioPampanga.ThemeAuthor = "Narwin";
            radioPampanga.ThemeName = "MetroLite";
            // 
            // radioZambales
            // 
            radioZambales.BackgroundColor = Color.White;
            radioZambales.BorderColor = Color.FromArgb(155, 155, 155);
            radioZambales.Checked = false;
            radioZambales.CheckSignColor = Color.FromArgb(65, 177, 225);
            radioZambales.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            radioZambales.DisabledBorderColor = Color.FromArgb(205, 205, 205);
            radioZambales.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            radioZambales.Group = 0;
            radioZambales.IsDerivedStyle = true;
            radioZambales.Location = new Point(596, 70);
            radioZambales.Name = "radioZambales";
            radioZambales.Size = new Size(100, 17);
            radioZambales.Style = Style.Light;
            radioZambales.StyleManager = null;
            radioZambales.TabIndex = 20;
            radioZambales.Text = "Zambales";
            radioZambales.ThemeAuthor = "Narwin";
            radioZambales.ThemeName = "MetroLite";
            // 
            // radioRetail
            // 
            radioRetail.BackgroundColor = Color.White;
            radioRetail.BorderColor = Color.FromArgb(155, 155, 155);
            radioRetail.Checked = false;
            radioRetail.CheckSignColor = Color.FromArgb(65, 177, 225);
            radioRetail.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            radioRetail.DisabledBorderColor = Color.FromArgb(205, 205, 205);
            radioRetail.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            radioRetail.Group = 0;
            radioRetail.IsDerivedStyle = true;
            radioRetail.Location = new Point(596, 37);
            radioRetail.Name = "radioRetail";
            radioRetail.Size = new Size(80, 17);
            radioRetail.Style = Style.Light;
            radioRetail.StyleManager = null;
            radioRetail.TabIndex = 21;
            radioRetail.Text = "Retail";
            radioRetail.ThemeAuthor = "Narwin";
            radioRetail.ThemeName = "MetroLite";
            // 
            // radioWholesale
            // 
            radioWholesale.BackgroundColor = Color.White;
            radioWholesale.BorderColor = Color.FromArgb(155, 155, 155);
            radioWholesale.Checked = false;
            radioWholesale.CheckSignColor = Color.FromArgb(65, 177, 225);
            radioWholesale.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            radioWholesale.DisabledBorderColor = Color.FromArgb(205, 205, 205);
            radioWholesale.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            radioWholesale.Group = 0;
            radioWholesale.IsDerivedStyle = true;
            radioWholesale.Location = new Point(596, 13);
            radioWholesale.Name = "radioWholesale";
            radioWholesale.Size = new Size(100, 17);
            radioWholesale.Style = Style.Light;
            radioWholesale.StyleManager = null;
            radioWholesale.TabIndex = 22;
            radioWholesale.Text = "Wholesale";
            radioWholesale.ThemeAuthor = "Narwin";
            radioWholesale.ThemeName = "MetroLite";
            // 
            // panelPayment
            // 
            panelPayment.BackgroundColor = Color.White;
            panelPayment.BorderColor = Color.FromArgb(150, 150, 150);
            panelPayment.BorderThickness = 1;
            panelPayment.Controls.Add(lblPaymentMethod);
            panelPayment.Controls.Add(comboPaymentMethod);
            panelPayment.Controls.Add(lblPaidAmount);
            panelPayment.Controls.Add(txtPaidAmount);
            panelPayment.Controls.Add(lblChange);
            panelPayment.Controls.Add(txtChange);
            panelPayment.Controls.Add(btnNext);
            panelPayment.Controls.Add(btnPrint);
            panelPayment.Controls.Add(btnPay);
            panelPayment.Controls.Add(lblSubtotal);
            panelPayment.Controls.Add(txtSubtotal);
            panelPayment.Controls.Add(lblTax);
            panelPayment.Controls.Add(txtTax);
            panelPayment.Controls.Add(lblDiscount);
            panelPayment.Controls.Add(txtDiscount);
            panelPayment.Controls.Add(lblTotal);
            panelPayment.Controls.Add(txtTotal);
            panelPayment.IsDerivedStyle = true;
            panelPayment.Location = new Point(10, 515);
            panelPayment.Name = "panelPayment";
            panelPayment.Size = new Size(1279, 200);
            panelPayment.Style = Style.Light;
            panelPayment.StyleManager = null;
            panelPayment.TabIndex = 23;
            panelPayment.ThemeAuthor = "Narwin";
            panelPayment.ThemeName = "MetroLite";
            // 
            // lblPaymentMethod
            // 
            lblPaymentMethod.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblPaymentMethod.IsDerivedStyle = true;
            lblPaymentMethod.Location = new Point(10, 10);
            lblPaymentMethod.Name = "lblPaymentMethod";
            lblPaymentMethod.Size = new Size(100, 23);
            lblPaymentMethod.Style = Style.Light;
            lblPaymentMethod.StyleManager = null;
            lblPaymentMethod.TabIndex = 0;
            lblPaymentMethod.Text = "Method of Payment:";
            lblPaymentMethod.ThemeAuthor = "Narwin";
            lblPaymentMethod.ThemeName = "MetroLite";
            // 
            // comboPaymentMethod
            // 
            comboPaymentMethod.AllowDrop = true;
            comboPaymentMethod.ArrowColor = Color.FromArgb(150, 150, 150);
            comboPaymentMethod.BackColor = Color.Transparent;
            comboPaymentMethod.BackgroundColor = Color.FromArgb(238, 238, 238);
            comboPaymentMethod.BorderColor = Color.FromArgb(150, 150, 150);
            comboPaymentMethod.CausesValidation = false;
            comboPaymentMethod.DisabledBackColor = Color.FromArgb(204, 204, 204);
            comboPaymentMethod.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            comboPaymentMethod.DisabledForeColor = Color.FromArgb(136, 136, 136);
            comboPaymentMethod.DrawMode = DrawMode.OwnerDrawFixed;
            comboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            comboPaymentMethod.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point);
            comboPaymentMethod.IsDerivedStyle = true;
            comboPaymentMethod.ItemHeight = 20;
            comboPaymentMethod.Location = new Point(160, 10);
            comboPaymentMethod.Name = "comboPaymentMethod";
            comboPaymentMethod.SelectedItemBackColor = Color.FromArgb(65, 177, 225);
            comboPaymentMethod.SelectedItemForeColor = Color.White;
            comboPaymentMethod.Size = new Size(200, 26);
            comboPaymentMethod.Style = Style.Light;
            comboPaymentMethod.StyleManager = null;
            comboPaymentMethod.TabIndex = 1;
            comboPaymentMethod.ThemeAuthor = "Narwin";
            comboPaymentMethod.ThemeName = "MetroLite";
            // 
            // lblPaidAmount
            // 
            lblPaidAmount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblPaidAmount.IsDerivedStyle = true;
            lblPaidAmount.Location = new Point(10, 50);
            lblPaidAmount.Name = "lblPaidAmount";
            lblPaidAmount.Size = new Size(100, 23);
            lblPaidAmount.Style = Style.Light;
            lblPaidAmount.StyleManager = null;
            lblPaidAmount.TabIndex = 2;
            lblPaidAmount.Text = "Paid Amount:";
            lblPaidAmount.ThemeAuthor = "Narwin";
            lblPaidAmount.ThemeName = "MetroLite";
            // 
            // txtPaidAmount
            // 
            txtPaidAmount.AutoCompleteCustomSource = null;
            txtPaidAmount.AutoCompleteMode = AutoCompleteMode.None;
            txtPaidAmount.AutoCompleteSource = AutoCompleteSource.None;
            txtPaidAmount.BorderColor = Color.FromArgb(155, 155, 155);
            txtPaidAmount.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtPaidAmount.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtPaidAmount.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtPaidAmount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtPaidAmount.HoverColor = Color.FromArgb(102, 102, 102);
            txtPaidAmount.Image = null;
            txtPaidAmount.IsDerivedStyle = true;
            txtPaidAmount.Lines = null;
            txtPaidAmount.Location = new Point(160, 50);
            txtPaidAmount.MaxLength = 32767;
            txtPaidAmount.Multiline = false;
            txtPaidAmount.Name = "txtPaidAmount";
            txtPaidAmount.ReadOnly = false;
            txtPaidAmount.Size = new Size(200, 25);
            txtPaidAmount.Style = Style.Light;
            txtPaidAmount.StyleManager = null;
            txtPaidAmount.TabIndex = 3;
            txtPaidAmount.TextAlign = HorizontalAlignment.Left;
            txtPaidAmount.ThemeAuthor = "Narwin";
            txtPaidAmount.ThemeName = "MetroLite";
            txtPaidAmount.UseSystemPasswordChar = false;
            txtPaidAmount.WatermarkText = "";
            // 
            // lblChange
            // 
            lblChange.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblChange.IsDerivedStyle = true;
            lblChange.Location = new Point(10, 90);
            lblChange.Name = "lblChange";
            lblChange.Size = new Size(100, 23);
            lblChange.Style = Style.Light;
            lblChange.StyleManager = null;
            lblChange.TabIndex = 4;
            lblChange.Text = "Change:";
            lblChange.ThemeAuthor = "Narwin";
            lblChange.ThemeName = "MetroLite";
            // 
            // txtChange
            // 
            txtChange.AutoCompleteCustomSource = null;
            txtChange.AutoCompleteMode = AutoCompleteMode.None;
            txtChange.AutoCompleteSource = AutoCompleteSource.None;
            txtChange.BorderColor = Color.FromArgb(155, 155, 155);
            txtChange.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtChange.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtChange.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtChange.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtChange.HoverColor = Color.FromArgb(102, 102, 102);
            txtChange.Image = null;
            txtChange.IsDerivedStyle = true;
            txtChange.Lines = null;
            txtChange.Location = new Point(160, 90);
            txtChange.MaxLength = 32767;
            txtChange.Multiline = false;
            txtChange.Name = "txtChange";
            txtChange.ReadOnly = false;
            txtChange.Size = new Size(200, 25);
            txtChange.Style = Style.Light;
            txtChange.StyleManager = null;
            txtChange.TabIndex = 5;
            txtChange.TextAlign = HorizontalAlignment.Left;
            txtChange.ThemeAuthor = "Narwin";
            txtChange.ThemeName = "MetroLite";
            txtChange.UseSystemPasswordChar = false;
            txtChange.WatermarkText = "";
            // 
            // btnNext
            // 
            btnNext.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnNext.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnNext.DisabledForeColor = Color.Gray;
            btnNext.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnNext.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnNext.HoverColor = Color.FromArgb(95, 207, 255);
            btnNext.HoverTextColor = Color.White;
            btnNext.IsDerivedStyle = true;
            btnNext.Location = new Point(1050, 60);
            btnNext.Name = "btnNext";
            btnNext.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnNext.NormalColor = Color.FromArgb(65, 177, 225);
            btnNext.NormalTextColor = Color.White;
            btnNext.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnNext.PressColor = Color.FromArgb(35, 147, 195);
            btnNext.PressTextColor = Color.White;
            btnNext.Size = new Size(100, 40);
            btnNext.Style = Style.Light;
            btnNext.StyleManager = null;
            btnNext.TabIndex = 6;
            btnNext.Text = "Next";
            btnNext.ThemeAuthor = "Narwin";
            btnNext.ThemeName = "MetroLite";
            // 
            // btnPrint
            // 
            btnPrint.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnPrint.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnPrint.DisabledForeColor = Color.Gray;
            btnPrint.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnPrint.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnPrint.HoverColor = Color.FromArgb(95, 207, 255);
            btnPrint.HoverTextColor = Color.White;
            btnPrint.IsDerivedStyle = true;
            btnPrint.Location = new Point(1050, 110);
            btnPrint.Name = "btnPrint";
            btnPrint.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnPrint.NormalColor = Color.FromArgb(65, 177, 225);
            btnPrint.NormalTextColor = Color.White;
            btnPrint.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnPrint.PressColor = Color.FromArgb(35, 147, 195);
            btnPrint.PressTextColor = Color.White;
            btnPrint.Size = new Size(100, 40);
            btnPrint.Style = Style.Light;
            btnPrint.StyleManager = null;
            btnPrint.TabIndex = 7;
            btnPrint.Text = "Print";
            btnPrint.ThemeAuthor = "Narwin";
            btnPrint.ThemeName = "MetroLite";
            // 
            // btnPay
            // 
            btnPay.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnPay.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnPay.DisabledForeColor = Color.Gray;
            btnPay.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnPay.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnPay.HoverColor = Color.FromArgb(95, 207, 255);
            btnPay.HoverTextColor = Color.White;
            btnPay.IsDerivedStyle = true;
            btnPay.Location = new Point(1050, 10);
            btnPay.Name = "btnPay";
            btnPay.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnPay.NormalColor = Color.FromArgb(65, 177, 225);
            btnPay.NormalTextColor = Color.White;
            btnPay.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnPay.PressColor = Color.FromArgb(35, 147, 195);
            btnPay.PressTextColor = Color.White;
            btnPay.Size = new Size(100, 40);
            btnPay.Style = Style.Light;
            btnPay.StyleManager = null;
            btnPay.TabIndex = 8;
            btnPay.Text = "Pay";
            btnPay.ThemeAuthor = "Narwin";
            btnPay.ThemeName = "MetroLite";
            // 
            // lblSubtotal
            // 
            lblSubtotal.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubtotal.IsDerivedStyle = true;
            lblSubtotal.Location = new Point(400, 10);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(100, 23);
            lblSubtotal.Style = Style.Light;
            lblSubtotal.StyleManager = null;
            lblSubtotal.TabIndex = 9;
            lblSubtotal.Text = "Subtotal:";
            lblSubtotal.ThemeAuthor = "Narwin";
            lblSubtotal.ThemeName = "MetroLite";
            // 
            // txtSubtotal
            // 
            txtSubtotal.AutoCompleteCustomSource = null;
            txtSubtotal.AutoCompleteMode = AutoCompleteMode.None;
            txtSubtotal.AutoCompleteSource = AutoCompleteSource.None;
            txtSubtotal.BorderColor = Color.FromArgb(155, 155, 155);
            txtSubtotal.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtSubtotal.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtSubtotal.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtSubtotal.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtSubtotal.HoverColor = Color.FromArgb(102, 102, 102);
            txtSubtotal.Image = null;
            txtSubtotal.IsDerivedStyle = true;
            txtSubtotal.Lines = null;
            txtSubtotal.Location = new Point(500, 10);
            txtSubtotal.MaxLength = 32767;
            txtSubtotal.Multiline = false;
            txtSubtotal.Name = "txtSubtotal";
            txtSubtotal.ReadOnly = false;
            txtSubtotal.Size = new Size(150, 25);
            txtSubtotal.Style = Style.Light;
            txtSubtotal.StyleManager = null;
            txtSubtotal.TabIndex = 10;
            txtSubtotal.TextAlign = HorizontalAlignment.Left;
            txtSubtotal.ThemeAuthor = "Narwin";
            txtSubtotal.ThemeName = "MetroLite";
            txtSubtotal.UseSystemPasswordChar = false;
            txtSubtotal.WatermarkText = "";
            // 
            // lblTax
            // 
            lblTax.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTax.IsDerivedStyle = true;
            lblTax.Location = new Point(400, 50);
            lblTax.Name = "lblTax";
            lblTax.Size = new Size(100, 23);
            lblTax.Style = Style.Light;
            lblTax.StyleManager = null;
            lblTax.TabIndex = 11;
            lblTax.Text = "Tax:";
            lblTax.ThemeAuthor = "Narwin";
            lblTax.ThemeName = "MetroLite";
            // 
            // txtTax
            // 
            txtTax.AutoCompleteCustomSource = null;
            txtTax.AutoCompleteMode = AutoCompleteMode.None;
            txtTax.AutoCompleteSource = AutoCompleteSource.None;
            txtTax.BorderColor = Color.FromArgb(155, 155, 155);
            txtTax.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtTax.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtTax.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtTax.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtTax.HoverColor = Color.FromArgb(102, 102, 102);
            txtTax.Image = null;
            txtTax.IsDerivedStyle = true;
            txtTax.Lines = null;
            txtTax.Location = new Point(500, 50);
            txtTax.MaxLength = 32767;
            txtTax.Multiline = false;
            txtTax.Name = "txtTax";
            txtTax.ReadOnly = false;
            txtTax.Size = new Size(150, 25);
            txtTax.Style = Style.Light;
            txtTax.StyleManager = null;
            txtTax.TabIndex = 12;
            txtTax.TextAlign = HorizontalAlignment.Left;
            txtTax.ThemeAuthor = "Narwin";
            txtTax.ThemeName = "MetroLite";
            txtTax.UseSystemPasswordChar = false;
            txtTax.WatermarkText = "";
            // 
            // lblDiscount
            // 
            lblDiscount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblDiscount.IsDerivedStyle = true;
            lblDiscount.Location = new Point(400, 90);
            lblDiscount.Name = "lblDiscount";
            lblDiscount.Size = new Size(100, 23);
            lblDiscount.Style = Style.Light;
            lblDiscount.StyleManager = null;
            lblDiscount.TabIndex = 13;
            lblDiscount.Text = "Discount:";
            lblDiscount.ThemeAuthor = "Narwin";
            lblDiscount.ThemeName = "MetroLite";
            // 
            // txtDiscount
            // 
            txtDiscount.AutoCompleteCustomSource = null;
            txtDiscount.AutoCompleteMode = AutoCompleteMode.None;
            txtDiscount.AutoCompleteSource = AutoCompleteSource.None;
            txtDiscount.BorderColor = Color.FromArgb(155, 155, 155);
            txtDiscount.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtDiscount.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtDiscount.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtDiscount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtDiscount.HoverColor = Color.FromArgb(102, 102, 102);
            txtDiscount.Image = null;
            txtDiscount.IsDerivedStyle = true;
            txtDiscount.Lines = null;
            txtDiscount.Location = new Point(500, 90);
            txtDiscount.MaxLength = 32767;
            txtDiscount.Multiline = false;
            txtDiscount.Name = "txtDiscount";
            txtDiscount.ReadOnly = false;
            txtDiscount.Size = new Size(150, 25);
            txtDiscount.Style = Style.Light;
            txtDiscount.StyleManager = null;
            txtDiscount.TabIndex = 14;
            txtDiscount.TextAlign = HorizontalAlignment.Left;
            txtDiscount.ThemeAuthor = "Narwin";
            txtDiscount.ThemeName = "MetroLite";
            txtDiscount.UseSystemPasswordChar = false;
            txtDiscount.WatermarkText = "";
            // 
            // lblTotal
            // 
            lblTotal.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTotal.IsDerivedStyle = true;
            lblTotal.Location = new Point(700, 10);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(100, 23);
            lblTotal.Style = Style.Light;
            lblTotal.StyleManager = null;
            lblTotal.TabIndex = 15;
            lblTotal.Text = "Total:";
            lblTotal.ThemeAuthor = "Narwin";
            lblTotal.ThemeName = "MetroLite";
            // 
            // txtTotal
            // 
            txtTotal.AutoCompleteCustomSource = null;
            txtTotal.AutoCompleteMode = AutoCompleteMode.None;
            txtTotal.AutoCompleteSource = AutoCompleteSource.None;
            txtTotal.BorderColor = Color.FromArgb(155, 155, 155);
            txtTotal.DisabledBackColor = Color.FromArgb(204, 204, 204);
            txtTotal.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            txtTotal.DisabledForeColor = Color.FromArgb(136, 136, 136);
            txtTotal.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtTotal.HoverColor = Color.FromArgb(102, 102, 102);
            txtTotal.Image = null;
            txtTotal.IsDerivedStyle = true;
            txtTotal.Lines = null;
            txtTotal.Location = new Point(800, 10);
            txtTotal.MaxLength = 32767;
            txtTotal.Multiline = false;
            txtTotal.Name = "txtTotal";
            txtTotal.ReadOnly = false;
            txtTotal.Size = new Size(150, 25);
            txtTotal.Style = Style.Light;
            txtTotal.StyleManager = null;
            txtTotal.TabIndex = 16;
            txtTotal.TextAlign = HorizontalAlignment.Left;
            txtTotal.ThemeAuthor = "Narwin";
            txtTotal.ThemeName = "MetroLite";
            txtTotal.UseSystemPasswordChar = false;
            txtTotal.WatermarkText = "";
            // 
            // lblSalesInfo
            // 
            lblSalesInfo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblSalesInfo.IsDerivedStyle = true;
            lblSalesInfo.Location = new Point(100, 10);
            lblSalesInfo.Name = "lblSalesInfo";
            lblSalesInfo.Size = new Size(200, 60);
            lblSalesInfo.Style = Style.Light;
            lblSalesInfo.StyleManager = null;
            lblSalesInfo.TabIndex = 0;
            lblSalesInfo.Text = "Mon, 01 Jan 2001 00:00:00\nSales No.: 0001";
            lblSalesInfo.ThemeAuthor = "Narwin";
            lblSalesInfo.ThemeName = "MetroLite";
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 42);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(1304, 721);
            tabPage1.TabIndex = 1;
            tabPage1.Text = "Invoice";
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 42);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(1304, 721);
            tabPage2.TabIndex = 2;
            tabPage2.Text = "Maintenance";
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 42);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1304, 721);
            tabPage3.TabIndex = 3;
            tabPage3.Text = "User Details";
            // 
            // metroSetControlBox1
            // 
            metroSetControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            metroSetControlBox1.CloseHoverBackColor = Color.FromArgb(183, 40, 40);
            metroSetControlBox1.CloseHoverForeColor = Color.White;
            metroSetControlBox1.CloseNormalForeColor = Color.Gray;
            metroSetControlBox1.DisabledForeColor = Color.DimGray;
            metroSetControlBox1.IsDerivedStyle = true;
            metroSetControlBox1.Location = new Point(1224, 7);
            metroSetControlBox1.MaximizeBox = true;
            metroSetControlBox1.MaximizeHoverBackColor = Color.FromArgb(238, 238, 238);
            metroSetControlBox1.MaximizeHoverForeColor = Color.Gray;
            metroSetControlBox1.MaximizeNormalForeColor = Color.Gray;
            metroSetControlBox1.MinimizeBox = true;
            metroSetControlBox1.MinimizeHoverBackColor = Color.FromArgb(238, 238, 238);
            metroSetControlBox1.MinimizeHoverForeColor = Color.Gray;
            metroSetControlBox1.MinimizeNormalForeColor = Color.Gray;
            metroSetControlBox1.Name = "metroSetControlBox1";
            metroSetControlBox1.Size = new Size(100, 25);
            metroSetControlBox1.Style = Style.Light;
            metroSetControlBox1.StyleManager = null;
            metroSetControlBox1.TabIndex = 1;
            metroSetControlBox1.Text = "metroSetControlBox1";
            metroSetControlBox1.ThemeAuthor = "Narwin";
            metroSetControlBox1.ThemeName = "MetroLite";
            // 
            // btnLogout
            // 
            btnLogout.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnLogout.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnLogout.DisabledForeColor = Color.Gray;
            btnLogout.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnLogout.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnLogout.HoverColor = Color.FromArgb(95, 207, 255);
            btnLogout.HoverTextColor = Color.White;
            btnLogout.IsDerivedStyle = true;
            btnLogout.Location = new Point(1235, 38);
            btnLogout.Name = "btnLogout";
            btnLogout.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnLogout.NormalColor = Color.FromArgb(65, 177, 225);
            btnLogout.NormalTextColor = Color.White;
            btnLogout.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnLogout.PressColor = Color.FromArgb(35, 147, 195);
            btnLogout.PressTextColor = Color.White;
            btnLogout.Size = new Size(85, 35);
            btnLogout.Style = Style.Light;
            btnLogout.StyleManager = null;
            btnLogout.TabIndex = 2;
            btnLogout.Text = "LOGOUT";
            btnLogout.ThemeAuthor = "Narwin";
            btnLogout.ThemeName = "MetroLite";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1336, 849);
            Controls.Add(btnLogout);
            Controls.Add(metroSetControlBox1);
            Controls.Add(metroSetTabControl1);
            Name = "MainForm";
            Text = "Autotech POS System";
            metroSetTabControl1.ResumeLayout(false);
            tabPageSales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewItemList).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewOrderCart).EndInit();
            panelPayment.ResumeLayout(false);
            ResumeLayout(false);
        }

        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private MetroSetControlBox metroSetControlBox1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private MetroSetButton btnLogout;
    }
}

#endregion

