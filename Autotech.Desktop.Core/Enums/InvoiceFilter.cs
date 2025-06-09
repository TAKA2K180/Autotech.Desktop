using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.Core.Enums
{
    public enum InvoiceFilterOption
    {
        [Description("Invoice #")]
        InvoiceNumber,

        [Description("Agent Name")]
        Agent,

        [Description("Date Sold")]
        DateSold,

        [Description("Customer Name")]
        AccountName,

        [Description("Payment Method")]
        PaymentType,

        [Description("Due Date")]
        DueDate,

        [Description("Status")]
        Status,

        [Description("Cluster")]
        Cluster
    }

}
