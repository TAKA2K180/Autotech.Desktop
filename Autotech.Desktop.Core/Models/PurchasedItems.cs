using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.Core.Models
{
    public class PurchasedItems : BaseModel
    {

        public Guid InvoiceId { get; set; }

        public Agents Agent { get; set; }

        public Guid AgentId { get; set; }
        public Sales Invoice { get; set; }  // Link to Sales, not InvoicePayments

        public Guid ItemId { get; set; }
        public Items Item { get; set; }

        public double Quantity { get; set; }
        public double ItemPrice { get; set; }
        public double TotalPrice { get; set; }
        public double QuantyPerBox { get; set; }
        public double Discount { get; set; }

        public string Status { get; set; } = "Pending";
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
