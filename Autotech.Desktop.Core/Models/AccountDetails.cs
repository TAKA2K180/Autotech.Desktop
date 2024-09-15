using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Autotech.Desktop.Core.Models
{
    public class AccountDetails : BaseModel
    {
        public Guid AccountId { get; set; }
        public double LitersOrdered { get; set; }
        public int OpenReceipts { get; set; }
        [JsonIgnore]
        public Accounts? Accounts { get; set; }
    }
}
