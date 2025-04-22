using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.DTO
{
    public class InvoiceDTO
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
        public Guid LocationId { get; set; }
        public List<InvoiceItemDTO> PurchasedItems { get; set; }
        public string strInvoiceNumber { get; set; } = string.Empty; // dummy
    }

    public class InvoiceItemDTO
    {
        public Guid ItemId { get; set; }
        public double Quantity { get; set; }
        public double ItemPrice { get; set; }
        public double TotalPrice { get; set; }
        public string ItemName { get; set; } = string.Empty; // dummy
    }

}
