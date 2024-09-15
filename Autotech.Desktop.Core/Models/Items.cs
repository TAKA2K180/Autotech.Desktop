using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.Core.Models
{
    public class Items : BaseModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public DateTime DateAdded { get; set; }
        public ItemDetails itemDetails { get; set; }
    }
}
