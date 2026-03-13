using System;

namespace Autotech.Desktop.BusinessLayer.DTO
{
    public class PurchasedItemExportDTO
    {
        public Guid Id { get; set; }
        public string strInvoiceNumber { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double ItemPrice { get; set; }
        public double TotalPrice { get; set; }
        public double QuantyPerBox { get; set; }
        public double Discount { get; set; }
    }
}
