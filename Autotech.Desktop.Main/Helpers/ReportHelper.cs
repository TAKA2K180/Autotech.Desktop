using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.Core.Models;

namespace Autotech.Desktop.Main.Helpers
{
    public class ReportHelper
    {
        private PrintDocument printDoc;
        private PrintPreviewDialog previewDialog;
        private InvoiceDetailsDTO invoice;
        private Accounts accounts;

        public ReportHelper()
        {
            printDoc = new PrintDocument();
            previewDialog = new PrintPreviewDialog();
        }

        /// <summary>
        /// Handles invoice printing with preview and PDF export asynchronously
        /// </summary>
        public async Task PrintInvoiceAsync(InvoiceDetailsDTO invoiceData, Accounts accountsData)
        {
            if (invoiceData == null)
            {
                MessageBox.Show("Invoice data is not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                invoice = invoiceData;
                accounts = accountsData;

                // Prepare file paths and settings asynchronously
                var prepareTask = await Task.Run(() =>
                {
                    // Create sanitized file name
                    string invoiceNumber = SanitizeFileName(invoice.strInvoiceNumber);
                    string customerName = SanitizeFileName(invoice.AccountName);
                    string date = invoice.DateSold.ToString("yyyy-MM-dd");
                    string fileName = $"{invoiceNumber}_{customerName}_{date}.pdf";

                    // Get Reports folder under app root
                    string appRoot = AppDomain.CurrentDomain.BaseDirectory;
                    string reportsFolder = Path.Combine(appRoot, "Reports");
                    if (!Directory.Exists(reportsFolder))
                        Directory.CreateDirectory(reportsFolder);

                    string savePath = Path.Combine(reportsFolder, fileName);

                    var settings = new PageSettings
                    {
                        Margins = new Margins(10, 10, 10, 10),
                        PaperSize = new PaperSize("A4", 827, 1169)
                    };

                    return (savePath, settings);
                });

                var savePath = prepareTask.savePath;
                var settings = prepareTask.settings;

                // Clean print events first
                printDoc.PrintPage -= PrintDoc_PrintPage;
                printDoc.PrintPage += PrintDoc_PrintPage;

                // ---- 1. Show preview dialog ----
                printDoc.DefaultPageSettings = settings;
                previewDialog.Document = printDoc;
                previewDialog.ShowDialog();

                // ---- 2. Auto-save to PDF asynchronously ----
                await Task.Run(() =>
                {
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
                });

                MessageBox.Show($"PDF also saved to:\n{savePath}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogHelper.Log("PDF export error", ex);
                MessageBox.Show("Failed to save PDF:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Legacy synchronous method for backward compatibility. Use PrintInvoiceAsync instead.
        /// </summary>
        [Obsolete("Use PrintInvoiceAsync instead")]
        public void PrintInvoice(InvoiceDetailsDTO invoiceData, Accounts accountsData)
        {
            // Synchronously wait for the async method
            PrintInvoiceAsync(invoiceData, accountsData).GetAwaiter().GetResult();
        }

        private string SanitizeFileName(string input)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                input = input.Replace(c, '_');
            }
            return input.Trim();
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            int itemCount = invoice.PurchasedItems.Count;
            int paymentCount = 0; // Will be calculated from payment history if needed
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
            g.DrawString($"Receipt #: {invoice.strInvoiceNumber}", bodyFont, Brushes.Black, x, y);
            g.DrawString($"Date: {invoice.DateSold:g}   Prepared by: {SessionManager.AgentDetails?.AgentName ?? "N/A"}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y);
            y += lineHeight;

            g.DrawString($"Customer: {invoice.AccountName}", bodyFont, Brushes.Black, x, y);
            g.DrawString($"Terms: {invoice.Terms} day(s)", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, x + usableWidth * 0.55f, y);
            y += lineHeight;

            g.DrawString($"Contact: {accounts.ContactNumber}", bodyFont, Brushes.Black, x, y);
            g.DrawString($"Owner's Name: {accounts.ContactPerson}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y);
            y += lineHeight;

            // Handle customer address - shrink text if too long to prevent wrapping
            string addressLabel = "Address:";
            string fullAddress = $"{addressLabel} {accounts.Address}";
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
            foreach (var item in invoice.PurchasedItems)
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
                var discText = item.Discount.HasValue ? ($"%{item.Discount.Value:N2}") : "%0.00";
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
            double subtotal = invoice.PurchasedItems.Sum(i => i.TotalPrice);
            double tax = invoice.Tax;
            double discount = invoice.DiscountPeso;
            double total = invoice.TotalSales;

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
            leftY += (lineHeight * 3.5f); // Reduced spacing to save paper
            rightY += lineHeight * 2;
            y = Math.Min(leftY, rightY);

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
    }
}
