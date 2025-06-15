using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.Core.Models
{
    public class Agents : BaseModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 20 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string AgentName { get; set; }
        public string AgentContactNumber { get; set; }
        public string AgentAddress { get; set; }
        public string AgentRole { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateLastLogin { get; set; }
        // Foreign key to Location
        public Guid LocationId { get; set; }
        public Locations Location { get; set; }
    }

    public class LoginHelper
    {
        public static bool isLoggedIn { get; set; }
    }
}
