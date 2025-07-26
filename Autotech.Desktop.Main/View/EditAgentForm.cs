using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Enums;
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
    public partial class EditAgentForm : MetroSetForm
    {
        private AgentDTO _agent;
        private List<Locations> _locations;
        private readonly bool _isEdit;

        public EditAgentForm(AgentDTO agent)
        {
            InitializeComponent();
            _agent = agent ?? new AgentDTO();
            _isEdit = agent != null;
            this.Load += EditAgentForm_Load;

        }

        private async void EditAgentForm_Load(object sender, EventArgs e)
        {
            await LoadLocationsAsync();
            
            if (_agent != null)
            {
                PopulateFormFields();
            }
        }

        private async Task LoadLocationsAsync()
        {
            try
            {
                var service = new LocationServices();
                _locations = await service.GetAllLocationsAsync();

                cboLocation.DisplayMember = "LocationName";
                cboLocation.ValueMember = "Id";
                cboLocation.DataSource = _locations;

                if (_agent != null && _agent.LocationId != Guid.Empty)
                    cboLocation.SelectedValue = _agent.LocationId;
            }
            catch (Exception ex)
            {
                LogHelper.Log("Error: ", ex);
                MessageBox.Show("Failed to load locations: " + ex.Message);
                _locations = new List<Locations>();
            }
        }

        private void PopulateUserRoles()
        {
            var roles = Enum.GetValues(typeof(UserRoles))
                .Cast<UserRoles>()
                .Select(role => new
                {
                    Value = role,
                    Text = role.GetType()
                               .GetField(role.ToString())
                               ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                               is DescriptionAttribute[] attrs && attrs.Length > 0
                               ? attrs[0].Description
                               : role.ToString()
                }).ToList();

            cboUserRole.DisplayMember = "Text";
            cboUserRole.ValueMember = "Value";
            cboUserRole.DataSource = roles;
        }

        private void PopulateFormFields()
        {
            txtUsername.Text = _agent.Username;
            txtPassword.Text = _agent.Password;
            txtAgentName.Text = _agent.AgentName;
            txtContact.Text = _agent.AgentContactNumber;
            txtAddress.Text = _agent.AgentAddress;
            cboUserRole.Text = _agent.AgentRole;
            cboLocation.Text = _agent.Location.LocationName;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var newAgent = new Agents
                {
                    Id = _isEdit ? _agent.Id : Guid.NewGuid(),
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text.Trim(),
                    AgentName = txtAgentName.Text.Trim(),
                    AgentContactNumber = txtContact.Text.Trim(),
                    AgentAddress = txtAddress.Text.Trim(),
                    AgentRole = cboUserRole.Text,
                    DateCreated = _isEdit ? _agent.DateCreated : DateTime.Now,
                    DateLastLogin = _isEdit ? _agent.DateLastLogin : null,
                    LocationId = cboLocation.SelectedValue is Guid locId ? locId : Guid.Empty
                };

                var service = new AgentsService();

                if (_isEdit)
                    await service.UpdateAgentAsync(newAgent);
                else
                    await service.AddAgentAsync(newAgent); // ❗ You must implement this if not yet

                MessageBox.Show(_isEdit ? "Agent updated!" : "Agent added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update agent: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Log("Failed to update agent: ", ex);
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

        private void cboUserRole_Click(object sender, EventArgs e)
        {
            PopulateUserRoles();
        }
    }
}
