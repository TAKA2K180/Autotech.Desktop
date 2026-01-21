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
using System.Text.Json;
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
            txtQtyPerBox.Text = _item.itemDetails?.QuantityPerBox.ToString("N2") ?? "0";

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
            {
                _item.itemDetails = new ItemDetails();
                _item.itemDetails.ItemId = _item.Id; // Ensure ItemId is set
            }
            else
            {
                // Make sure ItemId points to the item
                _item.itemDetails.ItemId = _item.Id;
            }

            // Parse all numeric values with proper error handling and validation
            if (!double.TryParse(txtOnHand.Text, out double onHand))
            {
                MessageBox.Show("Invalid value for On Hand quantity. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOnHand.Focus();
                return;
            }

            if (!double.TryParse(txtBataanRetail.Text, out double br))
            {
                MessageBox.Show("Invalid value for Bataan Retail price. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBataanRetail.Focus();
                return;
            }

            if (!double.TryParse(txtBataanWholesale.Text, out double bw))
            {
                MessageBox.Show("Invalid value for Bataan Wholesale price. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBataanWholesale.Focus();
                return;
            }

            if (!double.TryParse(txtPampangaRetail.Text, out double pr))
            {
                MessageBox.Show("Invalid value for Pampanga Retail price. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPampangaRetail.Focus();
                return;
            }

            if (!double.TryParse(txtPampangaWholesale.Text, out double pw))
            {
                MessageBox.Show("Invalid value for Pampanga Wholesale price. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPampangaWholesale.Focus();
                return;
            }

            if (!double.TryParse(txtZambalesRetail.Text, out double zr))
            {
                MessageBox.Show("Invalid value for Zambales Retail price. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtZambalesRetail.Focus();
                return;
            }

            if (!double.TryParse(txtZambalesWholesale.Text, out double zw))
            {
                MessageBox.Show("Invalid value for Zambales Wholesale price. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtZambalesWholesale.Focus();
                return;
            }

            if (!double.TryParse(txtQtyPerBox.Text, out double qtyPerBox))
            {
                MessageBox.Show("Invalid value for Quantity Per Box. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQtyPerBox.Focus();
                return;
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(_item.ItemCode))
            {
                MessageBox.Show("Item Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtItemCode.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_item.ItemName))
            {
                MessageBox.Show("Item Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtItemName.Focus();
                return;
            }

            // Update item details
            _item.itemDetails.OnHand = onHand;
            _item.itemDetails.BataanRetail = br;
            _item.itemDetails.BataanWholeSale = bw;
            _item.itemDetails.PampangaRetail = pr;
            _item.itemDetails.PampangaWholeSale = pw;
            _item.itemDetails.ZambalesRetail = zr;
            _item.itemDetails.ZambalesWholeSale = zw;
            _item.itemDetails.QuantityPerBox = qtyPerBox;

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
                    MessageBox.Show("Failed to update item. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                LogHelper.Log("HTTP Error updating item: ", ex);
                MessageBox.Show("Unable to connect to the server. Please check your internet connection and try again.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("does not contain a definition") || ex.Message.Contains("AnonymousType"))
            {
                LogHelper.Log("Data serialization error: ", ex);
                MessageBox.Show("There was a problem saving the item data. Please ensure all fields contain valid information and try again.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                LogHelper.Log("JSON serialization error: ", ex);
                MessageBox.Show("There was a problem formatting the data. Please ensure all prices and quantities are valid numbers and try again.", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LogHelper.Log("Unexpected error updating item: ", ex);
                MessageBox.Show($"An unexpected error occurred: {ex.GetType().Name}. Please try again or contact support if the problem persists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
