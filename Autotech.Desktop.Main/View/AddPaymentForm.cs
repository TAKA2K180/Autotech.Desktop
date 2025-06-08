using Autotech.Desktop.Core.Enums;
using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autotech.Desktop.Main.View
{
    public partial class AddPaymentForm : MetroSetForm
    {
        public double PaymentAmount { get; private set; }
        public PaymentMethod SelectedPaymentMethod { get; private set; }

        public AddPaymentForm()
        {
            InitializeComponent();
            PopulatePaymentMethods();
        }

        private void PopulatePaymentMethods()
        {
            comboPaymentMethod.DataSource = Enum.GetValues(typeof(PaymentMethod))
                .Cast<PaymentMethod>()
                .Select(p => new KeyValuePair<PaymentMethod, string>(p, GetEnumDescription(p)))
                .ToList();

            comboPaymentMethod.DisplayMember = "Value";
            comboPaymentMethod.ValueMember = "Key";
        }

        private string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                             .FirstOrDefault() as DescriptionAttribute;
            return attr?.Description ?? value.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtAmount.Text, out double amount) || amount <= 0)
            {
                MessageBox.Show("Enter a valid amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PaymentAmount = amount;
            SelectedPaymentMethod = ((KeyValuePair<PaymentMethod, string>)comboPaymentMethod.SelectedItem).Key;

            DialogResult = DialogResult.OK;
            Close();
        }
    }

}
