using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.Core.Models
{
    public class Accounts : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string? ContactPerson { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public int Terms { get; set; }
        public double DiscountPercent { get; set; }
        public string? Cluster { get; set; }
        public bool isActive { get; set; }
        public DateTime RegisterDate { get; set; }
        public AccountDetails AccountDetails { get; set; }
        // Foreign key to Location
        public Guid LocationId { get; set; }
        public Locations Location { get; set; }
    }
}
