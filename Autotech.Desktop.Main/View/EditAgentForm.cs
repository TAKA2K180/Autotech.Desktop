using Autotech.Desktop.BusinessLayer.DTO;
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
        public EditAgentForm(AgentDTO agents)
        {
            InitializeComponent();
            _agent = agents;

            PopulateLocationCombo();

            PopulateFormFields();
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

        private void PopulateLocationCombo()
        {
            cboLocation.DisplayMember = "LocationName";
            cboLocation.ValueMember = "Id";
            cboLocation.DataSource = _locations;

            // Select current agent's location
            if (_agent.LocationId != Guid.Empty)
                cboLocation.SelectedValue = _agent.LocationId;
        }

        private void PopulateFormFields()
        {
            txtUsername.Text = _agent.Username;
            txtPassword.Text = _agent.Password; // Only if you're displaying passwords, otherwise hide this
            txtAgentName.Text = _agent.AgentName;
            txtContact.Text = _agent.AgentContactNumber;
            txtAddress.Text = _agent.AgentAddress;
            cboUserRole.Text = _agent.AgentRole;
            cboLocation.Text = _agent.Location.LocationName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var updatedAgent = new Agents
                {
                    Id = _agent.Id,
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    AgentName = txtAgentName.Text,
                    AgentContactNumber = txtContact.Text,
                    AgentAddress = txtAddress.Text,
                    AgentRole = cboUserRole.Text,
                    DateCreated = _agent.DateCreated,
                    DateLastLogin = _agent.DateLastLogin,
                    LocationId = _agent.LocationId,
                    Location = cboLocation.SelectedValue is Guid selectedId
                        ? _locations.FirstOrDefault(l => l.Id == selectedId)
                        : null
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void cboLocation_Click(object sender, EventArgs e)
        {
            await LoadLocationsAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboUserRole_Click(object sender, EventArgs e)
        {
            PopulateUserRoles();
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
    }
}
