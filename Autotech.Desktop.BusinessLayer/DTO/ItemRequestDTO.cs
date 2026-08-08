using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.DTO
{
    public class ItemRequestDto
    {
        public Guid Id { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public string? UnitOfMeasure { get; set; }
        public double CostPrice { get; set; }
        public double MinimumStockLevel { get; set; }
        public long? Quantity { get; set; }
        public double QuantityPerBox { get; set; }
        public double ItemsSold { get; set; }
        public double Sales { get; set; }
        public double OnHand { get; set; }
        public double OriginalPrice { get; set; }
        public double RetailPrice { get; set; }
        public double WholesalePrice { get; set; }
    }
}
