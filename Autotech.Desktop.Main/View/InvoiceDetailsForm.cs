using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.DTO; // Ensure this namespace contains InvoiceDTO and InvoiceItemDTO
//using Autotech.Desktop.Main.Reports;
using MetroSet_UI.Forms;
//using DevExpress.XtraReports.UI;
//using DevExpress.XtraReports.UserDesigner;
//using DevExpress.XtraPrinting;
//using DevExpress.XtraReports;

namespace Autotech.Desktop.Main.View
{
    public partial class InvoiceDetailsForm : MetroSetForm
    {
        private InvoiceDetailsDTO _invoice;

        public InvoiceDetailsForm(InvoiceDetailsDTO invoice, Guid invoiceId)
        {
            InitializeComponent();

            _invoice = invoice;

            lblInvoiceNumber.Text = $"Invoice #: {_invoice.strInvoiceNumber}";
            lblCustomer.Text = $"Customer: {_invoice.AccountName}";
            lblDate.Text = $"Date: {_invoice.DateSold.ToShortDateString()}";
            lblStatus.Text = $"Status: {_invoice.Status}";

            InitializeGrid();
            LoadItemsToGrid();
            LoadPaymentHistoryAsync(invoiceId);
        }

        private void InitializeGrid()
        {
            dataGridViewInvoiceDetails.Columns.Clear();
            dataGridViewInvoiceDetails.AutoGenerateColumns = false;
            dataGridViewInvoiceDetails.AllowUserToAddRows = false;
            dataGridViewInvoiceDetails.ColumnHeadersHeight = 35;


            dataGridViewInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Item",
                DataPropertyName = "ItemName",
                Name = "itemName",
                ReadOnly = true
            });

            dataGridViewInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Qty",
                DataPropertyName = "Quantity",
                Name = "quantity",
                ReadOnly = true
            });

            dataGridViewInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Unit Price",
                DataPropertyName = "ItemPrice",
                Name = "price"
            });

            dataGridViewInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Discount",
                DataPropertyName = "Discount",
                Name = "price"
            });

            dataGridViewInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Total",
                DataPropertyName = "TotalPrice",
                Name = "total",
                ReadOnly = true
            });

            dataGridViewInvoiceDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewInvoiceDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridViewInvoiceDetails.CellValueChanged += dataGridViewInvoiceDetails_CellValueChanged;
            dataGridViewInvoiceDetails.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dataGridViewInvoiceDetails.IsCurrentCellDirty)
                    dataGridViewInvoiceDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
        }

        private async void LoadItemsToGrid()
        {
            dataGridViewInvoiceDetails.DataSource = null;
            dataGridViewInvoiceDetails.DataSource = _invoice.PurchasedItems;

            RecalculateTotals();
        }

        private void dataGridViewInvoiceDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridViewInvoiceDetails.Columns[e.ColumnIndex].Name == "price")
            {
                var row = dataGridViewInvoiceDetails.Rows[e.RowIndex];
                if (row.DataBoundItem is InvoiceItemDTO item)
                {
                    if (double.TryParse(row.Cells["price"].Value?.ToString(), out double newPrice))
                    {
                        item.ItemPrice = newPrice;
                        item.TotalPrice = newPrice * item.Quantity;
                        row.Cells["total"].Value = item.TotalPrice;
                        RecalculateTotals();
                    }
                }
            }
        }

        private void RecalculateTotals()
        {
            double subtotal = 0;
            foreach (var item in _invoice.PurchasedItems)
            {
                subtotal += item.TotalPrice;
            }

            txtSubtotal.Text = subtotal.ToString("C");
            double tax = subtotal * 0.12;
            double discount = _invoice.DiscountPeso;
            double total = subtotal + tax - discount;

            txtTax.Text = tax.ToString("C");
            txtDiscount.Text = discount.ToString("C");
            txtTotal.Text = total.ToString("C");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_invoice != null)
            {
                ////var report = new InvoiceReport();

                //// Set the data source
                //report.DataSource = new List<InvoiceDetailsDTO> { _invoice };
                //report.DataMember = ""; // root object

                //// Wrap in ReportPrintTool to access preview features
                ////var printTool = new ReportPrintTool(report);
                ////printTool.ShowPreviewDialog();
            }
            else
            {
                MessageBox.Show("Invoice data is not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            try
            {
                var salesService = new SalesService();
                var payments = await salesService.GetPaymentsBySaleIdAsync(_invoice.Id);
                var invoice = await salesService.GetInvoiceByIdAsync(_invoice.Id);

                double totalPaid = Math.Round(payments.Sum(p => p.PaymentAmount));
                double invoiceTotal = _invoice.TotalSales;

                string newStatus = "";
                if (totalPaid > invoiceTotal || invoice.RemainingBalance == 0)
                {
                    newStatus = "Fully paid";
                }
                else
                {
                    newStatus = "Incomplete";
                }

                    await salesService.ConfirmPaymentStatusAsync(_invoice.Id, newStatus);

                MessageBox.Show($"Invoice status updated to: {newStatus}", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh the invoice object or UI if needed
                _invoice.Status = newStatus;

                await LoadPaymentHistoryAsync(_invoice.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error confirming payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadPaymentHistoryAsync(Guid saleId)
        {
            try
            {
                var service = new SalesService();
                var payments = await service.GetPaymentsBySaleIdAsync(saleId);

                // Use invoice-level data (_invoice) for payment method and remaining balance
                var paymentMethod = _invoice.PaymentType;
                var remainingBalance = _invoice.RemainingBalance;

                var displayPayments = payments.Select(p => new
                {
                    AmountPaid = p.PaymentAmount,
                    DatePaid = p.DatePaid.ToString("g"),
                    PaymentMethod = paymentMethod,
                    RemainingBalance = p.RemainingBalance.ToString("C")
                }).ToList();

                dvgPaymentHistory.DataSource = null;
                dvgPaymentHistory.DataSource = displayPayments;

                // Set user-friendly column headers
                dvgPaymentHistory.Columns["AmountPaid"].HeaderText = "Amount Paid";
                dvgPaymentHistory.Columns["DatePaid"].HeaderText = "Date Paid";
                dvgPaymentHistory.Columns["PaymentMethod"].HeaderText = "Payment Method";
                dvgPaymentHistory.Columns["RemainingBalance"].HeaderText = "Remaining Balance";

                // Optional: format currency
                dvgPaymentHistory.Columns["AmountPaid"].DefaultCellStyle.Format = "C";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payment history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAddPayment_Click(object sender, EventArgs e)
        {
            using var dialog = new AddPaymentForm();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var payment = new PaymentHistoryDTO
            {
                SalesId = _invoice.Id,
                AccountId = _invoice.AccountId,
                AgentId = SessionManager.AgentDetails.Id,
                DatePaid = DateTime.Now,
                PaymentAmount = Math.Round(dialog.PaymentAmount),
                PaymentMethod = dialog.SelectedPaymentMethod.ToString(),
                RemainingBalance = Math.Round(_invoice.RemainingBalance - dialog.PaymentAmount)
            };

            try
            {
                var service = new SalesService();
                await service.AddPaymentAsync(payment);
                var updatedInvoice = await service.GetInvoiceByIdAsync(_invoice.Id);

                await LoadPaymentHistoryAsync(_invoice.Id);
                _invoice = updatedInvoice; // update cached copy
                MessageBox.Show("Payment recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to record payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnCancelInvoice_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to cancel this invoice?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    var salesService = new SalesService();
                    await salesService.ConfirmPaymentStatusAsync(_invoice.Id, "Denied");

                    MessageBox.Show("Invoice status updated to 'Denied'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Optional: Close the form or refresh status
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to cancel invoice: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
