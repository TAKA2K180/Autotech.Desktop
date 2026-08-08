using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.Core.Models
{
    public class ItemDetails : BaseModel
    {
        public Guid ItemId { get; set; }
        public double ItemsSold { get; set; }
        public double Sales { get; set; }
        public double OnHand { get; set; }
        public double QuantityPerBox { get; set; }
        public double RetailPrice { get; set; }
        public double WholesalePrice { get; set; }
        public Items? Item { get; set; }
    }
}
