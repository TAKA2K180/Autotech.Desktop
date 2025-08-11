using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;
using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Models;
using MetroSet_UI.Forms;

namespace Autotech.Desktop.Main.View
{
    public partial class InvoiceDetailsForm : MetroSetForm
    {
        private InvoiceDetailsDTO _invoice;
        private readonly MainForm _mainForm;
        private List<PaymentHistoryDTO> paymentHistoryDTOs;
        private PrintDocument printDoc = new PrintDocument();
        private PrintPreviewDialog previewDialog = new PrintPreviewDialog();
        private Accounts _accounts;

        public InvoiceDetailsForm(InvoiceDetailsDTO invoice, Guid invoiceId, Accounts accounts, MainForm mainForm)
        {
            InitializeComponent();

            _invoice = invoice;
            _mainForm = mainForm;
            _accounts = accounts;

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
                LogHelper.Log("Error: ", ex);
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

            txtSubtotal.Text = subtotal.ToString("₱#,##0.00");
            double tax = subtotal * 0.12;
            double discount = _invoice.DiscountPeso;
            double total = subtotal + tax - discount;

            txtTax.Text = _invoice.Tax.ToString("₱#,##0.00");
            txtDiscount.Text = discount.ToString("₱#,##0.00");
            txtTotal.Text = total.ToString("₱#,##0.00");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_invoice != null)
            {
                try
                {
                    // Create sanitized file name
                    string invoiceNumber = SanitizeFileName(_invoice.strInvoiceNumber);
                    string customerName = SanitizeFileName(_invoice.AccountName);
                    string date = _invoice.DateSold.ToString("yyyy-MM-dd");
                    string fileName = $"{invoiceNumber}_{customerName}_{date}.pdf";

                    // Get Reports folder under app root
                    string appRoot = AppDomain.CurrentDomain.BaseDirectory;
                    string reportsFolder = Path.Combine(appRoot, "Reports");
                    if (!Directory.Exists(reportsFolder))
                        Directory.CreateDirectory(reportsFolder);

                    string savePath = Path.Combine(reportsFolder, fileName);

                    // Configure for print preview
                    var settings = new PageSettings
                    {
                        Margins = new Margins(10, 10, 10, 10),
                        PaperSize = new PaperSize("A4", 827, 1169)
                    };

                    // Clean print events first
                    printDoc.PrintPage -= PrintDoc_PrintPage;
                    printDoc.PrintPage += PrintDoc_PrintPage;

                    // ---- 1. Show preview dialog ----
                    printDoc.DefaultPageSettings = settings;
                    previewDialog.Document = printDoc;
                    previewDialog.ShowDialog();

                    // ---- 2. Auto-save to PDF ----
                    var pdfDoc = new PrintDocument
                    {
                        DefaultPageSettings = settings,
                        PrinterSettings = new PrinterSettings
                        {
                            PrinterName = "Microsoft Print to PDF",
                            PrintToFile = true,
                            PrintFileName = savePath
                        }
                    };

                    pdfDoc.PrintPage += PrintDoc_PrintPage;
                    pdfDoc.Print();

                    MessageBox.Show($"PDF also saved to:\n{savePath}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    LogHelper.Log("PDF export error", ex);
                    MessageBox.Show("Failed to save PDF:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invoice data is not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string SanitizeFileName(string input)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                input = input.Replace(c, '_');
            }
            return input.Trim();
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
                LogHelper.Log("Error: ", ex);
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
                    RemainingBalance = p.RemainingBalance.ToString("₱#,##0.00")
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
                LogHelper.Log("Error: ", ex);
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
                LogHelper.Log("Error: ", ex);
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
                    LogHelper.Log("Error: ", ex);
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
            float colTotalWidth = 85; // Increase to fit ₱999,999.00 nicely
            float colTotal = right - colTotalWidth;
            float colDiscount = colTotal - 70;
            float colUnit = colDiscount - 90;
            float colQty = colUnit - 70;

            // Header
            g.DrawString("AUTOTECH CAR CARE CENTER", headerFont, Brushes.Black, x + usableWidth / 4, y); y += lineHeight;
            g.DrawString("Wawa, Abucay, Bataan", bodyFont, Brushes.Black, x + usableWidth / 3, y); y += lineHeight;
            g.DrawString("TRUST RECEIPT", headerFont, Brushes.Black, x + usableWidth / 3, y); y += lineHeight;
            g.DrawString("*THIS IS NOT YOUR OFFICIAL RECEIPT*", bodyFont, Brushes.Black, x + usableWidth / 4, y); y += lineHeight;

            // Info
            g.DrawString($"Receipt #: {_invoice.strInvoiceNumber}", bodyFont, Brushes.Black, x, y);
            g.DrawString($"Date: {DateTime.Now:g}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y); y += lineHeight;
            g.DrawString($"Terms: {_invoice.Terms} day(s)", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, x + usableWidth * 0.55f, y);
            g.DrawString($"Owner's Name: {_accounts.ContactPerson}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y + 22);
            g.DrawString($"Prepared by: {SessionManager.AgentDetails?.AgentName ?? "N/A"}", bodyFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Customer: {_invoice.AccountName}", bodyFont, Brushes.Black, x, y); y += lineHeight;
            string addressLabel = "Customer address:";
            string fullAddress = $"{addressLabel} {_accounts.Address}";
            float ownerColumnX = x + usableWidth * 0.55f; // same column as "Owner's Name:"
            float fullWidth = g.MeasureString(fullAddress, bodyFont).Width;

            if (x + fullWidth > ownerColumnX)
            {
                // Too long – split into two lines
                g.DrawString(addressLabel, bodyFont, Brushes.Black, x, y);
                y += lineHeight;
                g.DrawString(_accounts.Address, bodyFont, Brushes.Black, x, y);
            }
            else
            {
                // Fits in one line
                g.DrawString(fullAddress, bodyFont, Brushes.Black, x, y);
            }
            y += lineHeight;

            StringFormat rightAlign = new StringFormat();
            rightAlign.Alignment = StringAlignment.Far;

            // Table Header
            g.DrawString("Description", bodyFont, Brushes.Black, colDescription, y);
            g.DrawString("Qty", bodyFont, Brushes.Black, new RectangleF(colQty, y, colUnit - colQty, lineHeight), rightAlign);
            g.DrawString("Unit", bodyFont, Brushes.Black, new RectangleF(colUnit-2, y, colDiscount - colUnit, lineHeight), rightAlign);
            g.DrawString("Disc", bodyFont, Brushes.Black, new RectangleF(colDiscount-2, y, colTotal - colDiscount, lineHeight), rightAlign);
            g.DrawString("Total", bodyFont, Brushes.Black, new RectangleF(colTotal-2, y, right - colTotal, lineHeight), rightAlign);
            y += lineHeight;

            g.DrawLine(Pens.Black, x, y, right, y); y += 4;

            // Items
            foreach (var item in _invoice.PurchasedItems)
            {
                g.DrawString(item.ItemName, bodyFont, Brushes.Black, colDescription, y);
                g.DrawString(item.Quantity.ToString(), bodyFont, Brushes.Black, new RectangleF(colQty, y, colUnit - colQty, lineHeight), rightAlign);
                g.DrawString(item.ItemPrice.ToString("₱#,##0.00"), bodyFont, Brushes.Black, new RectangleF(colUnit, y, colDiscount - colUnit, lineHeight), rightAlign);
                g.DrawString(item.Discount.ToString("₱#,##0.00"), bodyFont, Brushes.Black, new RectangleF(colDiscount, y, colTotal - colDiscount, lineHeight), rightAlign);
                g.DrawString(item.TotalPrice.ToString("₱#,##0.00"), bodyFont, Brushes.Black, new RectangleF(colTotal - 15, y, 100, lineHeight), rightAlign);

                y += lineHeight;
            }

            y += 6;
            g.DrawLine(Pens.Black, x, y, right, y); y += 2;

            // Set up positions
            float colSplit = x + usableWidth * 0.70f;
            float leftY = y;
            float rightY = y;

            // Right Totals
            double subtotal = _invoice.PurchasedItems.Sum(i => i.TotalPrice);
            double tax = _invoice.Tax;
            double discount = _invoice.DiscountPeso;
            double total = _invoice.TotalSales;

            g.DrawString($"Subtotal: {subtotal:C}", bodyFont, Brushes.Black, colSplit, rightY); rightY += lineHeight;
            g.DrawString($"Tax: {tax:C}", bodyFont, Brushes.Black, colSplit, rightY); rightY += lineHeight;
            g.DrawString($"Discount: {discount:C}", bodyFont, Brushes.Black, colSplit, rightY); rightY += lineHeight;
            g.DrawString($"Total: {total:C}", headerFont, Brushes.Black, colSplit, rightY); rightY += lineHeight * 2;

            // Terms block (left side)
            string termsText = "Terms: Payable in cash otherwise stated. An interest of 3% per month will be charged on all overdue accounts. In case of non-payment of overdue accounts, the courts of Balanga City, Bataan will have jurisdictions and the customer hereby agree to pay the attorney's fees and court cost resulting therefrom.";
            RectangleF termsRect = new RectangleF(x, leftY, usableWidth * 0.65f, lineHeight * 5);
            g.DrawString(termsText, bodyFont, Brushes.Black, termsRect);
            leftY += (lineHeight * 4); // optional +2 padding
            rightY += lineHeight * 2;  // if you want
            y = Math.Min(leftY, rightY); // <<< use Max, NOT Min

            // Acknowledgment section
            g.DrawString("ALL CHECKS MUST BE PAYABLE TO: AUTOTECH CAR CARE CENTER", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, x, y);
            y += lineHeight;

            string ackText = "Received the items in good order, condition and accepted under the terms and conditions stipulated herein and at the back thereof.";
            RectangleF ackRect = new RectangleF(x, y, usableWidth, lineHeight * 3);
            g.DrawString(ackText, bodyFont, Brushes.Black, ackRect);
            y += lineHeight *2;

            // Signature
            g.DrawString("Received by:", bodyFont, Brushes.Black, x, y);
            y += lineHeight;
            g.DrawString("______________________________", bodyFont, Brushes.Black, x + 80, y);
            y += lineHeight;
            g.DrawString("SIGNATURE OVER PRINTED NAME", bodyFont, Brushes.Black, x + 80, y);
        }

    }
}
