using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.Core.Models
{
    public class Sales : BaseModel
    {
        public DateTime DateSold { get; set; }
        public string Agent { get; set; }
        public double DiscountPercent { get; set; }
        public double DiscountPeso { get; set; }
        public double Tax { get; set; }
        public double TotalSales { get; set; }
        public string AccountName { get; set; }
        public string PaymentType { get; set; }
        public int Terms { get; set; }
        public DateTime DueDate { get; set; }
        public double RemainingBalance { get; set; }
        public string Status { get; set; }
        public double TotalLiters { get; set; }
        public string Cluster { get; set; }
        public Guid AccountId { get; set; }
        public Accounts Accounts { get; set; }

        // Foreign key to Location
        public Guid LocationId { get; set; }
        public Locations Location { get; set; }
    }
}
