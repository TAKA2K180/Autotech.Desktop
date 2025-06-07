using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.DTO
{
    public class SalesDTO
    {
        public Guid Id { get; set; }
        public string strInvoiceNumber { get; set; }
        public DateTime DateSold { get; set; }
        public string Agent { get; set; }
        public string AccountName { get; set; }
        public string PaymentType { get; set; }
        public double TotalSales { get; set; }
        public double Tax { get; set; }
        public double DiscountPeso { get; set; }
        public int Terms { get; set; }
        public DateTime DueDate { get; set; }
        public double RemainingBalance { get; set; }
        public string Status { get; set; }
        public string Cluster { get; set; }
        public double TotalLiters { get; set; }
        public Guid LocationId { get; set; }
        public Guid AccountId { get; set; }

        public List<PurchasedItemDTO> PurchasedItems { get; set; }
    }

    public class PurchasedItemDTO
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }  // May come from Item?.ItemName in the backend
        public double Quantity { get; set; }
        public double QuantityPerBox { get; set; }
        public double ItemPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}
