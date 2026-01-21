using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
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
            if (e.RowIndex >= 0 && dataGridViewInvoiceDetails.Columns[e.ColumnIndex].Name == "price" || e.RowIndex >= 0 && dataGridViewInvoiceDetails.Columns[e.ColumnIndex].Name == "Discount")
            {
                var row = dataGridViewInvoiceDetails.Rows[e.RowIndex];
                if (row.DataBoundItem is PurchasedItemDetailsDTO item)
                {
                    if (double.TryParse(row.Cells["price"].Value?.ToString(), out double newPrice))
                    {
                        item.ItemPrice = newPrice;
                        var discountValue = item.Discount ?? 0;
                        item.TotalPrice = newPrice * item.Quantity - discountValue;
                        row.Cells["total"].Value = item.TotalPrice;
                        dataGridViewInvoiceDetails.Refresh();
                        RecalculateTotalsAfterLoad();
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
            double tax = _invoice.Tax;
            double discount = _invoice.DiscountPeso;
            double total = subtotal + tax - discount;

            txtTax.Text = _invoice.Tax.ToString("₱#,##0.00");
            txtDiscount.Text = discount.ToString("₱#,##0.00");
            txtTotal.Text = total.ToString("₱#,##0.00");
        }

        private void RecalculateTotalsAfterLoad()
        {
            double subtotal = 0;
            foreach (var item in _invoice.PurchasedItems)
            {
                subtotal += item.TotalPrice;
            }

            txtSubtotal.Text = subtotal.ToString("₱#,##0.00");
            double tax = double.TryParse(txtTax.Text.Replace("₱", "").Replace(",", ""), out double parsedTax) ? parsedTax : 0;
            double discount = double.TryParse(txtDiscount.Text.Replace("₱", "").Replace(",", ""), out double parsedDiscount) ? parsedDiscount : 0;
            double total = subtotal + tax - discount;

            txtTax.Text = tax.ToString("₱#,##0.00");
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

                // Prevent auto-selection of first row
                dvgPaymentHistory.ClearSelection();
                try { dvgPaymentHistory.CurrentCell = null; } catch { }

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
            g.DrawString($"Date: {DateTime.Now:g}   Prepared by: {SessionManager.AgentDetails?.AgentName ?? "N/A"}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y);
            y += lineHeight;
            
            g.DrawString($"Customer: {_invoice.AccountName}", bodyFont, Brushes.Black, x, y);
            g.DrawString($"Terms: {_invoice.Terms} day(s)", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, x + usableWidth * 0.55f, y);
            y += lineHeight;
            
            g.DrawString($"Contact: {_accounts.ContactNumber}", bodyFont, Brushes.Black, x, y);
            g.DrawString($"Owner's Name: {_accounts.ContactPerson}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y);
            y += lineHeight;

            // Handle customer address - shrink text if too long to prevent wrapping
            string addressLabel = "Address:";
            string fullAddress = $"{addressLabel} {_accounts.Address}";
            float addressLineWidth = g.MeasureString(fullAddress, bodyFont).Width;
            float maxAddressWidth = usableWidth * 0.65f;

            if (addressLineWidth > maxAddressWidth)
            {
                // Text is too long - use a smaller font
                Font smallFont = new Font("Arial", 8);
                g.DrawString(fullAddress, smallFont, Brushes.Black, x, y);
            }
            else
            {
                // Fits in normal font
                g.DrawString(fullAddress, bodyFont, Brushes.Black, x, y);
            }
            y += lineHeight;

            StringFormat rightAlign = new StringFormat();
            rightAlign.Alignment = StringAlignment.Far;

            // Use fixed widths for right-hand numeric columns to ensure alignment
            float totalWidth = 110f;   // space for total amount
            float discWidth = 90f;     // space for discount
            float unitWidth = 110f;    // space for unit price
            float qtyWidth = 50f;      // space for qty

            float colTotalPos = right - totalWidth;
            float colDiscPos = colTotalPos - discWidth;
            float colUnitPos = colDiscPos - unitWidth;
            float colQtyPos = colUnitPos - qtyWidth;

            // Table Header
            // Use left-aligned numbers for a uniform appearance
            var numAlign = new StringFormat() { Alignment = StringAlignment.Near };
            g.DrawString("Description", bodyFont, Brushes.Black, new RectangleF(colDescription, y, colQtyPos - colDescription, lineHeight));
            g.DrawString("Qty", bodyFont, Brushes.Black, new RectangleF(colQtyPos, y, qtyWidth, lineHeight), numAlign);
            g.DrawString("Unit", bodyFont, Brushes.Black, new RectangleF(colUnitPos, y, unitWidth, lineHeight), numAlign);
            g.DrawString("Disc", bodyFont, Brushes.Black, new RectangleF(colDiscPos, y, discWidth, lineHeight), numAlign);
            g.DrawString("Total", bodyFont, Brushes.Black, new RectangleF(colTotalPos, y, totalWidth, lineHeight), numAlign);
            y += lineHeight;

            g.DrawLine(Pens.Black, x, y, right, y); y += 4;

            // Items
            foreach (var item in _invoice.PurchasedItems)
            {
                // Description should wrap if too long
                var descRect = new RectangleF(colDescription, y, colQtyPos - colDescription, lineHeight);
                g.DrawString(item.ItemName, bodyFont, Brushes.Black, descRect);

                // Qty
                g.DrawString(item.Quantity.ToString(), bodyFont, Brushes.Black, new RectangleF(colQtyPos, y, qtyWidth, lineHeight), numAlign);

                // Unit price (may be null)
                var unitText = item.ItemPrice.HasValue ? ($"₱{item.ItemPrice.Value:N2}") : "₱0.00";
                g.DrawString(unitText, bodyFont, Brushes.Black, new RectangleF(colUnitPos, y, unitWidth, lineHeight), numAlign);

                // Discount (may be null)
                var discText = item.Discount.HasValue ? ($"₱{item.Discount.Value:N2}") : "₱0.00";
                g.DrawString(discText, bodyFont, Brushes.Black, new RectangleF(colDiscPos, y, discWidth, lineHeight), numAlign);

                // Total
                g.DrawString($"₱{item.TotalPrice:N2}", bodyFont, Brushes.Black, new RectangleF(colTotalPos, y, totalWidth, lineHeight), numAlign);

                y += lineHeight;
            }

            y += 6;
            g.DrawLine(Pens.Black, x, y, right, y); y += 2;

            // Set up positions for totals section
            float colSplit = x + usableWidth * 0.65f;
            float totalsLabelCol = colSplit;
            float totalsValueCol = colSplit + 85f;  // Fixed offset for values
            float leftY = y;
            float rightY = y;

            // Right Totals - with proper alignment
            double subtotal = _invoice.PurchasedItems.Sum(i => i.TotalPrice);
            double tax = _invoice.Tax;
            double discount = _invoice.DiscountPeso;
            double total = _invoice.TotalSales;

            StringFormat rightAlignFormat = new StringFormat() { Alignment = StringAlignment.Far };

            g.DrawString("Subtotal:", bodyFont, Brushes.Black, totalsLabelCol, rightY);
            g.DrawString($"₱{subtotal:N2}", bodyFont, Brushes.Black, new RectangleF(totalsValueCol, rightY, 100, lineHeight), rightAlignFormat);
            rightY += lineHeight;

            g.DrawString("Tax:", bodyFont, Brushes.Black, totalsLabelCol, rightY);
            g.DrawString($"₱{tax:N2}", bodyFont, Brushes.Black, new RectangleF(totalsValueCol, rightY, 100, lineHeight), rightAlignFormat);
            rightY += lineHeight;

            g.DrawString("Discount:", bodyFont, Brushes.Black, totalsLabelCol, rightY);
            g.DrawString($"₱{discount:N2}", bodyFont, Brushes.Black, new RectangleF(totalsValueCol, rightY, 100, lineHeight), rightAlignFormat);
            rightY += lineHeight;

            g.DrawString("Total:", headerFont, Brushes.Black, totalsLabelCol, rightY);
            g.DrawString($"₱{total:N2}", headerFont, Brushes.Black, new RectangleF(totalsValueCol, rightY, 100, lineHeight), rightAlignFormat);
            rightY += lineHeight * 2;

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
            y += lineHeight * 2;

            // Signature
            g.DrawString("Received by:", bodyFont, Brushes.Black, x, y);
            y += lineHeight;
            g.DrawString("______________________________", bodyFont, Brushes.Black, x + 80, y);
            y += lineHeight;
            g.DrawString("SIGNATURE OVER PRINTED NAME", bodyFont, Brushes.Black, x + 80, y);
        }

        private void txtTax_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                RecalculateTotalsAfterLoad();
            }
        }

        private void txtDiscount_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                RecalculateTotalsAfterLoad();
            }
        }

        private async void btnSaveInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate invoice state
                if (_invoice == null || _invoice.Id == Guid.Empty)
                {
                    MessageBox.Show("Invoice data is not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse totals from UI
                double tax = double.TryParse(txtTax.Text.Replace("₱", "").Replace(",", ""), out double parsedTax) ? parsedTax : 0;
                double discount = double.TryParse(txtDiscount.Text.Replace("₱", "").Replace(",", ""), out double parsedDiscount) ? parsedDiscount : 0;
                double subtotal = _invoice.PurchasedItems.Sum(i => i.TotalPrice);
                double total = subtotal + tax - discount;

                // Update invoice object with edited values
                _invoice.Tax = tax;
                _invoice.DiscountPeso = discount;
                _invoice.TotalSales = total;

                // Calculate discount percent
                double priceBeforeDiscount = total + discount - tax;
                double discountPercent = priceBeforeDiscount == 0 ? 0 : (discount / priceBeforeDiscount) * 100;

                // Build update DTO
                var updateDto = new InvoiceDTO
                {
                    DateSold = _invoice.DateSold,
                    Agent = _invoice.Agent,
                    DiscountPercent = discountPercent,
                    DiscountPeso = discount,
                    Tax = tax,
                    TotalSales = total,
                    AccountName = _invoice.AccountName,
                    PaymentType = _invoice.PaymentType,
                    Terms = _invoice.Terms,
                    DueDate = _invoice.DueDate,
                    RemainingBalance = _invoice.RemainingBalance,
                    Status = _invoice.Status,
                    TotalLiters = _invoice.TotalLiters,
                    Cluster = _invoice.Cluster,
                    AccountId = _invoice.AccountId,
                    LocationId = _invoice.LocationId,
                    strInvoiceNumber = _invoice.strInvoiceNumber,
                    PurchasedItems = _invoice.PurchasedItems.Select(item => new InvoiceItemDTO
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemPrice = item.ItemPrice ?? 0,
                        TotalPrice = item.TotalPrice,
                        ItemName = item.ItemName,
                        Discount = item.Discount ?? 0,
                        AgentId = SessionManager.AgentDetails.Id
                    }).ToList()
                };

                // Call API to update invoice
                var service = new SalesService();
                await service.UpdateInvoiceAsync(_invoice.Id, updateDto);

                MessageBox.Show("Invoice saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh parent form
                await _mainForm.LoadInvoicesAsync();
            }
            catch (Exception ex)
            {
                LogHelper.Log("Error saving invoice: ", ex);
                MessageBox.Show($"Failed to save invoice: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
