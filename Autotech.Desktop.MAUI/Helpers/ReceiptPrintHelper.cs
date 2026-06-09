#if WINDOWS
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.Core.Models;
using DrawingFont = System.Drawing.Font;
using DrawingGraphics = System.Drawing.Graphics;
using DrawingStringFormat = System.Drawing.StringFormat;

namespace Autotech.Desktop.MAUI.Helpers;

public static class ReceiptPrintHelper
{
    public static async Task<string> PrintInvoiceAsync(InvoiceDetailsDTO invoice, Accounts? account)
    {
        if (invoice is null)
        {
            throw new ArgumentNullException(nameof(invoice));
        }

        var savePath = BuildSavePath(invoice);
        var settings = new PageSettings
        {
            Margins = new Margins(10, 10, 10, 10),
            PaperSize = new PaperSize("A4", 827, 1169)
        };

        using var previewDoc = new PrintDocument
        {
            DefaultPageSettings = settings
        };
        previewDoc.PrintPage += (_, e) => DrawReceipt(e, invoice, account);

        using (var previewDialog = new PrintPreviewDialog
        {
            Document = previewDoc,
            Width = 1200,
            Height = 900,
            StartPosition = FormStartPosition.CenterScreen,
            UseAntiAlias = true
        })
        {
            previewDialog.ShowDialog();
        }

        await Task.Run(() =>
        {
            using var pdfDoc = new PrintDocument
            {
                DefaultPageSettings = settings,
                PrinterSettings = new PrinterSettings
                {
                    PrinterName = "Microsoft Print to PDF",
                    PrintToFile = true,
                    PrintFileName = savePath
                }
            };

            if (!pdfDoc.PrinterSettings.IsValid)
            {
                throw new InvalidOperationException("Microsoft Print to PDF is not available on this computer.");
            }

            pdfDoc.PrintPage += (_, e) => DrawReceipt(e, invoice, account);
            pdfDoc.Print();
        });

        return savePath;
    }

    private static string BuildSavePath(InvoiceDetailsDTO invoice)
    {
        var invoiceNumber = SanitizeFileName(invoice.strInvoiceNumber);
        var customerName = SanitizeFileName(invoice.AccountName);
        var date = invoice.DateSold.ToString("yyyy-MM-dd");
        var fileName = $"{invoiceNumber}_{customerName}_{date}.pdf";
        var reportsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
        Directory.CreateDirectory(reportsFolder);
        return Path.Combine(reportsFolder, fileName);
    }

    private static string SanitizeFileName(string? input)
    {
        var value = string.IsNullOrWhiteSpace(input) ? "invoice" : input;
        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            value = value.Replace(invalidChar, '_');
        }

        return value.Trim();
    }

    private static void DrawReceipt(PrintPageEventArgs e, InvoiceDetailsDTO invoice, Accounts? account)
    {
        var graphics = e.Graphics ?? throw new InvalidOperationException("The printer did not provide a drawing surface.");
        var itemCount = invoice.PurchasedItems.Count;
        var estimatedHeight = 300 + itemCount * 20;
        var halfA4Height = 792 / 2;

        if (estimatedHeight < halfA4Height)
        {
            e.PageSettings.PaperSize = new PaperSize("HalfA4", 827, (int)estimatedHeight);
        }

        using var headerFont = new DrawingFont("Arial", 12, FontStyle.Bold);
        using var bodyFont = new DrawingFont("Arial", 10);
        using var termsFont = new DrawingFont("Arial", 10, FontStyle.Bold);
        using var termsDaysFont = new DrawingFont("Arial", 14, FontStyle.Bold);

        var x = e.MarginBounds.Left;
        var y = (float)e.MarginBounds.Top;
        var right = e.MarginBounds.Right;
        var usableWidth = right - x;
        var lineHeight = bodyFont.GetHeight(graphics) + 2;
        const string peso = "\u20B1";

        graphics.DrawString("AUTOTECH CAR CARE CENTER", headerFont, Brushes.Black, x + usableWidth / 4, y);
        y += lineHeight;
        graphics.DrawString("Wawa, Abucay, Bataan", bodyFont, Brushes.Black, x + usableWidth / 3, y);
        y += lineHeight;
        graphics.DrawString("TRUST RECEIPT", headerFont, Brushes.Black, x + usableWidth / 3, y);
        y += lineHeight;
        graphics.DrawString("*THIS IS NOT YOUR OFFICIAL RECEIPT*", bodyFont, Brushes.Black, x + usableWidth / 4, y);
        y += lineHeight;

        graphics.DrawString($"Receipt #: {invoice.strInvoiceNumber}", bodyFont, Brushes.Black, x, y);
        graphics.DrawString($"Date: {invoice.DateSold:g}   Prepared by: {SessionManager.AgentDetails?.AgentName ?? "N/A"}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y);
        y += lineHeight;

        graphics.DrawString($"Customer: {invoice.AccountName}", bodyFont, Brushes.Black, x, y);
        graphics.DrawString($"Terms: {invoice.Terms} day(s)", termsDaysFont, Brushes.Black, x + usableWidth * 0.55f, y);
        y += lineHeight;

        graphics.DrawString($"Contact: {account?.ContactNumber ?? string.Empty}", bodyFont, Brushes.Black, x, y);
        graphics.DrawString($"Owner's Name: {account?.ContactPerson ?? string.Empty}", bodyFont, Brushes.Black, x + usableWidth * 0.55f, y);
        y += lineHeight;

        var fullAddress = $"Address: {account?.Address ?? string.Empty}";
        if (graphics.MeasureString(fullAddress, bodyFont).Width > usableWidth * 0.65f)
        {
            using var smallFont = new DrawingFont("Arial", 8);
            graphics.DrawString(fullAddress, smallFont, Brushes.Black, x, y);
        }
        else
        {
            graphics.DrawString(fullAddress, bodyFont, Brushes.Black, x, y);
        }

        y += lineHeight;

        using var numberAlign = new DrawingStringFormat { Alignment = StringAlignment.Near };
        var totalWidth = 110f;
        var discountWidth = 90f;
        var unitWidth = 110f;
        var quantityWidth = 50f;
        var colTotal = right - totalWidth;
        var colDiscount = colTotal - discountWidth;
        var colUnit = colDiscount - unitWidth;
        var colQuantity = colUnit - quantityWidth;

        graphics.DrawString("Description", bodyFont, Brushes.Black, new RectangleF(x, y, colQuantity - x, lineHeight));
        graphics.DrawString("Qty", bodyFont, Brushes.Black, new RectangleF(colQuantity, y, quantityWidth, lineHeight), numberAlign);
        graphics.DrawString("Unit", bodyFont, Brushes.Black, new RectangleF(colUnit, y, unitWidth, lineHeight), numberAlign);
        graphics.DrawString("Disc", bodyFont, Brushes.Black, new RectangleF(colDiscount, y, discountWidth, lineHeight), numberAlign);
        graphics.DrawString("Total", bodyFont, Brushes.Black, new RectangleF(colTotal, y, totalWidth, lineHeight), numberAlign);
        y += lineHeight;

        graphics.DrawLine(Pens.Black, x, y, right, y);
        y += 4;

        foreach (var item in invoice.PurchasedItems)
        {
            graphics.DrawString(item.ItemName, bodyFont, Brushes.Black, new RectangleF(x, y, colQuantity - x, lineHeight));
            graphics.DrawString(item.Quantity.ToString(), bodyFont, Brushes.Black, new RectangleF(colQuantity, y, quantityWidth, lineHeight), numberAlign);
            graphics.DrawString(item.ItemPrice.HasValue ? $"{peso}{item.ItemPrice.Value:N2}" : $"{peso}0.00", bodyFont, Brushes.Black, new RectangleF(colUnit, y, unitWidth, lineHeight), numberAlign);
            graphics.DrawString(item.Discount.HasValue ? $"%{item.Discount.Value:N2}" : "%0.00", bodyFont, Brushes.Black, new RectangleF(colDiscount, y, discountWidth, lineHeight), numberAlign);
            graphics.DrawString($"{peso}{item.TotalPrice:N2}", bodyFont, Brushes.Black, new RectangleF(colTotal, y, totalWidth, lineHeight), numberAlign);
            y += lineHeight;
        }

        y += 6;
        graphics.DrawLine(Pens.Black, x, y, right, y);
        y += 2;

        var colSplit = x + usableWidth * 0.65f;
        var totalsLabelCol = colSplit;
        var totalsValueCol = colSplit + 85f;
        var leftY = y;
        var rightY = y;

        using var rightAlign = new DrawingStringFormat { Alignment = StringAlignment.Far };
        DrawTotalLine(graphics, bodyFont, "Subtotal:", $"{peso}{invoice.PurchasedItems.Sum(i => i.TotalPrice):N2}", totalsLabelCol, totalsValueCol, ref rightY, lineHeight, rightAlign);
        DrawTotalLine(graphics, bodyFont, "Tax:", $"{peso}{invoice.Tax:N2}", totalsLabelCol, totalsValueCol, ref rightY, lineHeight, rightAlign);
        DrawTotalLine(graphics, bodyFont, "Discount:", $"{peso}{invoice.DiscountPeso:N2}", totalsLabelCol, totalsValueCol, ref rightY, lineHeight, rightAlign);
        DrawTotalLine(graphics, headerFont, "Total:", $"{peso}{invoice.TotalSales:N2}", totalsLabelCol, totalsValueCol, ref rightY, lineHeight, rightAlign);
        rightY += lineHeight * 2;

        const string termsText = "Terms: Payable in cash otherwise stated. An interest of 3% per month will be charged on all overdue accounts. In case of non-payment of overdue accounts, the courts of Balanga City, Bataan will have jurisdictions and the customer hereby agree to pay the attorney's fees and court cost resulting therefrom.";
        graphics.DrawString(termsText, bodyFont, Brushes.Black, new RectangleF(x, leftY, usableWidth * 0.65f, lineHeight * 5));
        leftY += lineHeight * 3.5f;
        y = Math.Min(leftY, rightY);

        graphics.DrawString("ALL CHECKS MUST BE PAYABLE TO: AUTOTECH CAR CARE CENTER", termsFont, Brushes.Black, x, y);
        y += lineHeight;

        const string ackText = "Received the items in good order, condition and accepted under the terms and conditions stipulated herein and at the back thereof.";
        graphics.DrawString(ackText, bodyFont, Brushes.Black, new RectangleF(x, y, usableWidth, lineHeight * 3));
        y += lineHeight * 2;

        graphics.DrawString("Received by:", bodyFont, Brushes.Black, x, y);
        y += lineHeight;
        graphics.DrawString("______________________________", bodyFont, Brushes.Black, x + 80, y);
        y += lineHeight;
        graphics.DrawString("SIGNATURE OVER PRINTED NAME", bodyFont, Brushes.Black, x + 80, y);
    }

    private static void DrawTotalLine(DrawingGraphics graphics, DrawingFont font, string label, string value, float labelX, float valueX, ref float y, float lineHeight, DrawingStringFormat align)
    {
        graphics.DrawString(label, font, Brushes.Black, labelX, y);
        graphics.DrawString(value, font, Brushes.Black, new RectangleF(valueX, y, 100, lineHeight), align);
        y += lineHeight;
    }
}
#else
using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.Core.Models;

namespace Autotech.Desktop.MAUI.Helpers;

public static class ReceiptPrintHelper
{
    public static Task<string> PrintInvoiceAsync(InvoiceDetailsDTO invoice, Accounts? account)
    {
        throw new PlatformNotSupportedException("Invoice receipt printing is only available on Windows.");
    }
}
#endif
