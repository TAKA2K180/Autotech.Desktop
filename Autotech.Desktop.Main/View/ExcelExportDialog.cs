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
    public partial class ExcelExportDialog : MetroSetForm
    {
        public event EventHandler InvoicesButtonClicked;
        public event EventHandler PurchasedItemsButtonClicked;

        public ExcelExportDialog()
        {
            InitializeComponent();
            btnInvoices.Click += BtnInvoices_Click;
            btnPurchasedItems.Click += BtnPurchasedItems_Click;
        }

        private void BtnInvoices_Click(object sender, EventArgs e)
        {
            InvoicesButtonClicked?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void BtnPurchasedItems_Click(object sender, EventArgs e)
        {
            PurchasedItemsButtonClicked?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
