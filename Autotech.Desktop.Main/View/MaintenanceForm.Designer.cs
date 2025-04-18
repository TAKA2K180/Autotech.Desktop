﻿using MetroSet_UI.Controls;

namespace Autotech.Desktop.Main
{
    partial class MaintenanceForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            metroSetTabControl1 = new MetroSetTabControl();
            tabPageAccounts = new TabPage();
            tabPageAgents = new TabPage();
            tabPageItems = new TabPage();
            tabPageSales = new TabPage();
            tabPageLocations = new TabPage();
            tabPageReports = new TabPage();
            tabPageDashboard = new TabPage();
            btnAdd = new MetroSetButton();
            btnEdit = new MetroSetButton();
            btnDelete = new MetroSetButton();
            btnRefresh = new MetroSetButton();
            panel1 = new Panel();
            metroSetTabControl1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // metroSetTabControl1
            // 
            metroSetTabControl1.AnimateEasingType = MetroSet_UI.Enums.EasingType.CubeOut;
            metroSetTabControl1.AnimateTime = 200;
            metroSetTabControl1.BackgroundColor = Color.White;
            metroSetTabControl1.Controls.Add(tabPageAccounts);
            metroSetTabControl1.Controls.Add(tabPageAgents);
            metroSetTabControl1.Controls.Add(tabPageItems);
            metroSetTabControl1.Controls.Add(tabPageSales);
            metroSetTabControl1.Controls.Add(tabPageLocations);
            metroSetTabControl1.Controls.Add(tabPageReports);
            metroSetTabControl1.Controls.Add(tabPageDashboard);
            metroSetTabControl1.Dock = DockStyle.Top;
            metroSetTabControl1.IsDerivedStyle = true;
            metroSetTabControl1.ItemSize = new Size(100, 38);
            metroSetTabControl1.Location = new Point(12, 70);
            metroSetTabControl1.Name = "metroSetTabControl1";
            metroSetTabControl1.SelectedIndex = 0;
            metroSetTabControl1.SelectedTextColor = Color.White;
            metroSetTabControl1.Size = new Size(776, 543);
            metroSetTabControl1.SizeMode = TabSizeMode.Fixed;
            metroSetTabControl1.Speed = 100;
            metroSetTabControl1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetTabControl1.StyleManager = null;
            metroSetTabControl1.TabIndex = 0;
            metroSetTabControl1.ThemeAuthor = "Narwin";
            metroSetTabControl1.ThemeName = "MetroLite";
            metroSetTabControl1.UnselectedTextColor = Color.Gray;
            metroSetTabControl1.UseAnimation = false;
            // 
            // tabPageAccounts
            // 
            tabPageAccounts.BackColor = Color.White;
            tabPageAccounts.Location = new Point(4, 42);
            tabPageAccounts.Name = "tabPageAccounts";
            tabPageAccounts.Padding = new Padding(3);
            tabPageAccounts.Size = new Size(768, 497);
            tabPageAccounts.TabIndex = 0;
            tabPageAccounts.Text = "Accounts";
            // 
            // tabPageAgents
            // 
            tabPageAgents.BackColor = Color.White;
            tabPageAgents.Location = new Point(4, 42);
            tabPageAgents.Name = "tabPageAgents";
            tabPageAgents.Padding = new Padding(3);
            tabPageAgents.Size = new Size(768, 497);
            tabPageAgents.TabIndex = 1;
            tabPageAgents.Text = "Agents";
            // 
            // tabPageItems
            // 
            tabPageItems.BackColor = Color.White;
            tabPageItems.Location = new Point(4, 42);
            tabPageItems.Name = "tabPageItems";
            tabPageItems.Padding = new Padding(3);
            tabPageItems.Size = new Size(768, 497);
            tabPageItems.TabIndex = 2;
            tabPageItems.Text = "Items";
            // 
            // tabPageSales
            // 
            tabPageSales.BackColor = Color.White;
            tabPageSales.Location = new Point(4, 42);
            tabPageSales.Name = "tabPageSales";
            tabPageSales.Padding = new Padding(3);
            tabPageSales.Size = new Size(768, 497);
            tabPageSales.TabIndex = 3;
            tabPageSales.Text = "Sales";
            // 
            // tabPageLocations
            // 
            tabPageLocations.BackColor = Color.White;
            tabPageLocations.Location = new Point(4, 42);
            tabPageLocations.Name = "tabPageLocations";
            tabPageLocations.Padding = new Padding(3);
            tabPageLocations.Size = new Size(768, 497);
            tabPageLocations.TabIndex = 4;
            tabPageLocations.Text = "Locations";
            // 
            // tabPageReports
            // 
            tabPageReports.BackColor = Color.White;
            tabPageReports.Location = new Point(4, 42);
            tabPageReports.Name = "tabPageReports";
            tabPageReports.Padding = new Padding(3);
            tabPageReports.Size = new Size(768, 497);
            tabPageReports.TabIndex = 5;
            tabPageReports.Text = "Reports";
            // 
            // tabPageDashboard
            // 
            tabPageDashboard.BackColor = Color.White;
            tabPageDashboard.Location = new Point(4, 42);
            tabPageDashboard.Name = "tabPageDashboard";
            tabPageDashboard.Padding = new Padding(3);
            tabPageDashboard.Size = new Size(768, 497);
            tabPageDashboard.TabIndex = 6;
            tabPageDashboard.Text = "Dashboard";
            // 
            // btnAdd
            // 
            btnAdd.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnAdd.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnAdd.DisabledForeColor = Color.Gray;
            btnAdd.Dock = DockStyle.Left;
            btnAdd.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnAdd.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnAdd.HoverColor = Color.FromArgb(95, 207, 255);
            btnAdd.HoverTextColor = Color.White;
            btnAdd.IsDerivedStyle = true;
            btnAdd.Location = new Point(150, 0);
            btnAdd.Name = "btnAdd";
            btnAdd.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnAdd.NormalColor = Color.FromArgb(65, 177, 225);
            btnAdd.NormalTextColor = Color.White;
            btnAdd.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnAdd.PressColor = Color.FromArgb(35, 147, 195);
            btnAdd.PressTextColor = Color.White;
            btnAdd.Size = new Size(75, 65);
            btnAdd.Style = MetroSet_UI.Enums.Style.Light;
            btnAdd.StyleManager = null;
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Add";
            btnAdd.ThemeAuthor = "Narwin";
            btnAdd.ThemeName = "MetroLite";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnEdit.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnEdit.DisabledForeColor = Color.Gray;
            btnEdit.Dock = DockStyle.Left;
            btnEdit.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnEdit.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnEdit.HoverColor = Color.FromArgb(95, 207, 255);
            btnEdit.HoverTextColor = Color.White;
            btnEdit.IsDerivedStyle = true;
            btnEdit.Location = new Point(0, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnEdit.NormalColor = Color.FromArgb(65, 177, 225);
            btnEdit.NormalTextColor = Color.White;
            btnEdit.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnEdit.PressColor = Color.FromArgb(35, 147, 195);
            btnEdit.PressTextColor = Color.White;
            btnEdit.Size = new Size(75, 65);
            btnEdit.Style = MetroSet_UI.Enums.Style.Light;
            btnEdit.StyleManager = null;
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Edit";
            btnEdit.ThemeAuthor = "Narwin";
            btnEdit.ThemeName = "MetroLite";
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnDelete.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnDelete.DisabledForeColor = Color.Gray;
            btnDelete.Dock = DockStyle.Left;
            btnDelete.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnDelete.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnDelete.HoverColor = Color.FromArgb(95, 207, 255);
            btnDelete.HoverTextColor = Color.White;
            btnDelete.IsDerivedStyle = true;
            btnDelete.Location = new Point(75, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnDelete.NormalColor = Color.FromArgb(65, 177, 225);
            btnDelete.NormalTextColor = Color.White;
            btnDelete.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnDelete.PressColor = Color.FromArgb(35, 147, 195);
            btnDelete.PressTextColor = Color.White;
            btnDelete.Size = new Size(75, 65);
            btnDelete.Style = MetroSet_UI.Enums.Style.Light;
            btnDelete.StyleManager = null;
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            btnDelete.ThemeAuthor = "Narwin";
            btnDelete.ThemeName = "MetroLite";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnRefresh.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnRefresh.DisabledForeColor = Color.Gray;
            btnRefresh.Dock = DockStyle.Left;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnRefresh.HoverBorderColor = Color.FromArgb(95, 207, 255);
            btnRefresh.HoverColor = Color.FromArgb(95, 207, 255);
            btnRefresh.HoverTextColor = Color.White;
            btnRefresh.IsDerivedStyle = true;
            btnRefresh.Location = new Point(225, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.NormalBorderColor = Color.FromArgb(65, 177, 225);
            btnRefresh.NormalColor = Color.FromArgb(65, 177, 225);
            btnRefresh.NormalTextColor = Color.White;
            btnRefresh.PressBorderColor = Color.FromArgb(35, 147, 195);
            btnRefresh.PressColor = Color.FromArgb(35, 147, 195);
            btnRefresh.PressTextColor = Color.White;
            btnRefresh.Size = new Size(75, 65);
            btnRefresh.Style = MetroSet_UI.Enums.Style.Light;
            btnRefresh.StyleManager = null;
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Refresh";
            btnRefresh.ThemeAuthor = "Narwin";
            btnRefresh.ThemeName = "MetroLite";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(btnAdd);
            panel1.Controls.Add(btnDelete);
            panel1.Controls.Add(btnEdit);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(12, 613);
            panel1.Name = "panel1";
            panel1.Size = new Size(776, 65);
            panel1.TabIndex = 5;
            // 
            // MaintenanceForm
            // 
            ClientSize = new Size(800, 699);
            Controls.Add(panel1);
            Controls.Add(metroSetTabControl1);
            Name = "MaintenanceForm";
            Text = "Autotech POS System - Maintenance";
            WindowState = FormWindowState.Maximized;
            metroSetTabControl1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private MetroSetTabControl metroSetTabControl1;
        private TabPage tabPageAccounts;
        private TabPage tabPageAgents;
        private TabPage tabPageItems;
        private TabPage tabPageSales;
        private TabPage tabPageLocations;
        private TabPage tabPageReports;
        private TabPage tabPageDashboard;
        private MetroSetButton btnAdd;
        private MetroSetButton btnEdit;
        private MetroSetButton btnDelete;
        private MetroSetButton btnRefresh;
        private Panel panel1;
    }
}

    #endregion

