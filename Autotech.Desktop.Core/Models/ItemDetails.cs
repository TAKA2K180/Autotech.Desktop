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
        public double BataanRetail { get; set; }
        public double BataanWholeSale { get; set; }
        public double PampangaRetail { get; set; }
        public double PampangaWholeSale { get; set; }
        public double ZambalesRetail { get; set; }
        public double ZambalesWholeSale { get; set; }
    }
}
