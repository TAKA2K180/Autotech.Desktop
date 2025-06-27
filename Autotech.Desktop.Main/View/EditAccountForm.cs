using Autotech.Desktop.BusinessLayer.DTO;
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
    public partial class EditAccountForm : MetroSetForm
    {
        private Accounts _account;
        private List<Locations> _locations;
        public EditAccountForm(Accounts account)
        {
            InitializeComponent();
            _account = account;
            PopulateFields();
        }

        private void PopulateFields()
        {
            txtName.Text = _account.Name;
            txtContactPerson.Text = _account.ContactPerson;
            txtEmail.Text = _account.Email;
            txtContactNumber.Text = _account.ContactNumber;
            txtAddress.Text = _account.Address;
            txtTerms.Value = _account.Terms;
            cboLocation.Text = _account.Cluster;
            chkIsActive.Checked = _account.isActive;
            dtmDateRegistered.Value = _account.RegisterDate;
            // Optional: populate Location if needed

        }

        private async Task<List<Locations>> LoadLocationsAsync()
        {
            try
            {
                var service = new LocationServices(); // Assume you have a service
                var locations = await service.GetAllLocationsAsync(); // Should return List<LocationDTO>
                _locations = locations;

                cboLocation.DataSource = locations;
                cboLocation.DisplayMember = "LocationName";
                cboLocation.ValueMember = "Id";
                return locations;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load locations: " + ex.Message);
                return new List<Locations> { };
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                var updatedAccount = new Accounts
                {
                    Id = _account.Id,
                    Name = txtName.Text.Trim(),
                    ContactPerson = txtContactPerson.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    ContactNumber = txtContactNumber.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Terms = int.TryParse(txtTerms.Text.Trim(), out var terms) ? terms : 0,
                    DiscountPercent = _account.DiscountPercent,
                    Cluster = cboLocation.Text.Trim(),
                    isActive = chkIsActive.Checked,
                    RegisterDate = _account.RegisterDate,
                    LocationId = (Guid)cboLocation.SelectedValue,

                    // 🚫 Do NOT assign AccountDetails or Location object here
                    Location = cboLocation.SelectedValue is Guid selectedId
                        ? _locations.FirstOrDefault(l => l.Id == selectedId)
                        : null
                };

                var service = new AccountService();
                await service.UpdateAccountAsync(updatedAccount);

                MessageBox.Show("Account updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update account: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void cboLocation_Click(object sender, EventArgs e)
        {
            await LoadLocationsAsync();
        }
    }
}
