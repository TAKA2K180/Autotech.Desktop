using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.Core.Enums
{
    public enum PaymentMethod
    {
        [Description("Cash")]
        Cash,

        [Description("Credit Card")]
        CreditCard,

        [Description("GCash")]
        GCash,

        [Description("Bank Transfer")]
        BankTransfer,

        [Description("Check")]
        Check
    }

    public static class EnumHelper
    {
        public static List<KeyValuePair<PaymentMethod, string>> GetPaymentMethodDescriptions()
        {
            return Enum.GetValues(typeof(PaymentMethod))
                       .Cast<PaymentMethod>()
                       .Select(pm => new KeyValuePair<PaymentMethod, string>(
                           pm,
                           GetEnumDescription(pm)
                       ))
                       .ToList();
        }

        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}
