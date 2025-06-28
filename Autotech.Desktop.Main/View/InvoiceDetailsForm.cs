using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
        private readonly MainForm _mainForm;
        private List<PaymentHistoryDTO> paymentHistoryDTOs;
        private PrintDocument printDoc = new PrintDocument();
        private PrintPreviewDialog previewDialog = new PrintPreviewDialog();

        public InvoiceDetailsForm(InvoiceDetailsDTO invoice, Guid invoiceId, MainForm mainForm)
        {
            InitializeComponent();

            _invoice = invoice;
            _mainForm = mainForm;

            lblInvoiceNumber.Text = $"Invoice #: {_invoice.strInvoiceNumber}";
            lblCustomer.Text = $"Customer: {_invoice.AccountName}";
            lblDate.Text = $"Date: {_invoice.DateSold.ToShortDateString()}";
            lblStatus.Text = $"Status: {_invoice.Status}";
            lblOrigin.Text = _invoice.isMobile == true ? "Origin: Mobile" : "Origin: Desktop";
            
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
                Name = "Discount"
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
            dataGridViewInvoiceDetails.CellFormatting += dataGridViewInvoiceDetails_CellFormatting;
            dataGridViewInvoiceDetails.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dataGridViewInvoiceDetails.IsCurrentCellDirty)
                    dataGridViewInvoiceDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
        }
        private void dataGridViewInvoiceDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dataGridViewInvoiceDetails.Columns[e.ColumnIndex].Name == "Discount")
                {
                    if (e.Value != null && double.TryParse(e.Value.ToString(), out double discount))
                    {
                        e.Value = discount.ToString("N2");
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
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

            txtTax.Text = _invoice.Tax.ToString("C");
            txtDiscount.Text = discount.ToString("C");
            txtTotal.Text = total.ToString("C");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_invoice != null)
            {
                var settings = new PageSettings
                {
                    Margins = new Margins(10, 10, 10, 10),
                    PaperSize = new PaperSize("A4", 827, 1169)
                };
                printDoc.DefaultPageSettings = settings;
                printDoc.PrintPage += PrintDoc_PrintPage;
                previewDialog.Document = printDoc;
                previewDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Invoice data is not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnCloseInvoiceDetail_Click(object sender, EventArgs e)
        {
            this.Close();
            await _mainForm.LoadInvoicesAsync(); // Refresh main invoice list
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
                paymentHistoryDTOs = payments;

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
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            int itemCount = _invoice.PurchasedItems.Count;
            int paymentCount = paymentHistoryDTOs?.Count ?? 0;
            float estimatedHeight = 300 + (itemCount * 20) + (paymentCount * 18);
            float halfA4Height = 792 / 2;

            if (estimatedHeight < halfA4Height)
                e.PageSettings.PaperSize = new PaperSize("HalfA4", 827, (int)estimatedHeight);

            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 10);
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float right = e.MarginBounds.Right;
            float usableWidth = right - x;
            float lineHeight = bodyFont.GetHeight(g) + 2;

            float colDescription = x;
            float colQty = x + usableWidth * 0.50f;
            float colUnit = x + usableWidth * 0.65f;
            float colDiscount = x + usableWidth * 0.75f;
            float colTotal = x + usableWidth * 0.85f;

            // Header
            g.DrawString("AUTOTECH CAR CARE CENTER", headerFont, Brushes.Black, x + usableWidth / 4, y); y += lineHeight;
            g.DrawString("Wawa, Abucay, Bataan", bodyFont, Brushes.Black, x + usableWidth / 3, y); y += lineHeight;
            g.DrawString("TRUST RECEIPT", headerFont, Brushes.Black, x + usableWidth / 3, y); y += lineHeight;
            g.DrawString("*THIS IS NOT YOUR OFFICIAL RECEIPT*", bodyFont, Brushes.Black, x + usableWidth / 4, y); y += lineHeight;

            // Info
            g.DrawString($"Receipt #: {_invoice.strInvoiceNumber}", bodyFont, Brushes.Black, x, y);
            g.DrawString($"Date: {DateTime.Now:g}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y); y += lineHeight;
            g.DrawString($"Prepared by: {SessionManager.AgentDetails?.AgentName ?? "N/A"}", bodyFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Customer: {_invoice.AccountName}", bodyFont, Brushes.Black, x, y); y += lineHeight;

            // Table Header
            g.DrawString("Description", bodyFont, Brushes.Black, colDescription, y);
            g.DrawString("Qty", bodyFont, Brushes.Black, colQty, y);
            g.DrawString("Unit", bodyFont, Brushes.Black, colUnit, y);
            g.DrawString("Disc", bodyFont, Brushes.Black, colDiscount, y);
            g.DrawString("Total", bodyFont, Brushes.Black, colTotal, y);
            y += lineHeight;

            g.DrawLine(Pens.Black, x, y, right, y); y += 4;

            // Items
            foreach (var item in _invoice.PurchasedItems)
            {
                g.DrawString(item.ItemName, bodyFont, Brushes.Black, colDescription, y);
                g.DrawString(item.Quantity.ToString(), bodyFont, Brushes.Black, colQty, y);
                g.DrawString(item.ItemPrice.ToString("C"), bodyFont, Brushes.Black, colUnit, y);
                g.DrawString(item.Discount.ToString("C"), bodyFont, Brushes.Black, colDiscount, y);
                g.DrawString(item.TotalPrice.ToString("C"), bodyFont, Brushes.Black, colTotal, y);
                y += lineHeight;
            }

            y += 6;
            g.DrawLine(Pens.Black, x, y, right, y); y += lineHeight;

            // Set up columns: Totals on right, Payment history on left
            float colSplit = x + usableWidth * 0.70f;
            float leftY = y;
            float rightY = y;

            // Totals (Right)
            double subtotal = _invoice.PurchasedItems.Sum(i => i.TotalPrice);
            double tax = _invoice.Tax;
            double discount = _invoice.DiscountPeso;
            double total = _invoice.TotalSales;

            g.DrawString($"Subtotal: {subtotal:C}", bodyFont, Brushes.Black, colSplit, rightY); rightY += lineHeight;
            g.DrawString($"Tax: {tax:C}", bodyFont, Brushes.Black, colSplit, rightY); rightY += lineHeight;
            g.DrawString($"Discount: {discount:C}", bodyFont, Brushes.Black, colSplit, rightY); rightY += lineHeight;
            g.DrawString($"Total: {total:C}", headerFont, Brushes.Black, colSplit, rightY); rightY += lineHeight * 2;

            // Payment History (Left)
            g.DrawString("PAYMENT HISTORY", headerFont, Brushes.Black, x, leftY); leftY += lineHeight;

            if (paymentHistoryDTOs != null && paymentHistoryDTOs.Any())
            {
                foreach (var p in paymentHistoryDTOs)
                {
                    string line = $"Paid: {p.PaymentAmount:C} on {p.DatePaid:g} ---- Balance history: {p.RemainingBalance:C}";
                    g.DrawString(line, bodyFont, Brushes.Black, x, leftY);
                    leftY += lineHeight;
                }
                g.DrawString($"Remaining Balance: {_invoice.RemainingBalance:C}", bodyFont, Brushes.Black, x, leftY);
                leftY += lineHeight * 2;
            }
            else
            {
                g.DrawString("No payment history available.", bodyFont, Brushes.Black, x, leftY);
                leftY += lineHeight * 2;
            }

            // Set Y to max of both columns
            y = Math.Max(leftY, rightY);

            // Signature
            g.DrawString("Received by: ___________________________", bodyFont, Brushes.Black, x, y); y += lineHeight + 5;
            g.DrawString("SIGNATURE OVER PRINTED NAME", bodyFont, Brushes.Black, x + 150, y); y += lineHeight * 2;


            
        }
    }
}
