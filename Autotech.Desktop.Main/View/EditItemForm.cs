using Autotech.Desktop.BusinessLayer.Helpers;
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
    public partial class EditItemForm : MetroSetForm
    {
        private Items _item;
        public EditItemForm(Items item)
        {
            _item = item;
            InitializeComponent();
            _item = item ?? new Items();
        }
        private void EditItemForm_Load(object sender, EventArgs e)
        {
            // Fill fields
            txtItemCode.Text = _item.ItemCode;
            txtItemName.Text = _item.ItemName;
            txtDescription.Text = _item.ItemDescription;

            txtOnHand.Text = _item.itemDetails?.OnHand.ToString("N2") ?? "0";
            txtBataanRetail.Text = _item.itemDetails?.BataanRetail.ToString("N2") ?? "0";
            txtBataanWholesale.Text = _item.itemDetails?.BataanWholeSale.ToString("N2") ?? "0";
            txtPampangaRetail.Text = _item.itemDetails?.PampangaRetail.ToString("N2") ?? "0";
            txtPampangaWholesale.Text = _item.itemDetails?.PampangaWholeSale.ToString("N2") ?? "0";
            txtZambalesRetail.Text = _item.itemDetails?.ZambalesRetail.ToString("N2") ?? "0";
            txtZambalesWholesale.Text = _item.itemDetails?.ZambalesWholeSale.ToString("N2") ?? "0";
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            // 🧠 Map UI input to _item
            _item.ItemCode = txtItemCode.Text.Trim();
            _item.ItemName = txtItemName.Text.Trim();
            _item.ItemDescription = txtDescription.Text.Trim();

            if (_item.itemDetails == null)
                _item.itemDetails = new ItemDetails();

            double.TryParse(txtOnHand.Text, out double onHand);
            double.TryParse(txtBataanRetail.Text, out double br);
            double.TryParse(txtBataanWholesale.Text, out double bw);
            double.TryParse(txtPampangaRetail.Text, out double pr);
            double.TryParse(txtPampangaWholesale.Text, out double pw);
            double.TryParse(txtZambalesRetail.Text, out double zr);
            double.TryParse(txtZambalesWholesale.Text, out double zw);

            _item.itemDetails.OnHand = onHand;
            _item.itemDetails.BataanRetail = br;
            _item.itemDetails.BataanWholeSale = bw;
            _item.itemDetails.PampangaRetail = pr;
            _item.itemDetails.PampangaWholeSale = pw;
            _item.itemDetails.ZambalesRetail = zr;
            _item.itemDetails.ZambalesWholeSale = zw;

            try
            {
                var service = new ItemServices();
                bool success = await service.UpdateItemAsync(_item);

                if (success)
                {
                    MessageBox.Show("Item updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to update item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("Error: ", ex);
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
