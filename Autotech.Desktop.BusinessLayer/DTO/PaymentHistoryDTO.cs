using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.DTO
{
    public class PaymentHistoryDTO
    {
        public Guid Id { get; set; }
        public Guid SalesId { get; set; }
        public double PaymentAmount { get; set; }
        public DateTime DatePaid { get; set; }
        public Guid AccountId { get; set; }
        public Guid AgentId { get; set; }
        public string PaymentMethod { get; set; }
        public double RemainingBalance { get; set; }
    }
}
