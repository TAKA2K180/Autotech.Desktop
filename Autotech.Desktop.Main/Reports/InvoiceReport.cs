//using DevExpress.XtraPrinting;
//using DevExpress.XtraReports.UI;
//using System;
//using System.Drawing;

//namespace Autotech.Desktop.Main.Reports
//{
//    public partial class InvoiceReport : XtraReport
//    {
//        public InvoiceReport()
//        {
//            InitializeComponent();
//            BuildLayout();
//        }

//        private void BuildLayout()
//        {
//            this.Bands.Clear();

//            // Margins
//            TopMarginBand topMargin = new TopMarginBand() { HeightF = 50 };
//            BottomMarginBand bottomMargin = new BottomMarginBand() { HeightF = 50 };
//            DetailBand detail = new DetailBand() { HeightF = 25 };

//            ReportHeaderBand header = new ReportHeaderBand() { HeightF = 200 };
//            PageFooterBand footer = new PageFooterBand() { HeightF = 100 };

//            // Logo
//            XRPictureBox logo = new XRPictureBox
//            {
//                ImageUrl = "file://C:/PathToYourLogo/logo.png", // Replace with your local logo path
//                Sizing = ImageSizeMode.Squeeze,
//                LocationF = new PointF(0, 0),
//                SizeF = new SizeF(100, 100)
//            };

//            // Title
//            XRLabel companyName = new XRLabel
//            {
//                Text = "AUTOTECH CORPORATION",
//                Font = new Font("Arial", 16, FontStyle.Bold),
//                BoundsF = new RectangleF(110, 0, 500, 30)
//            };

//            XRLabel address = new XRLabel
//            {
//                Text = "Balanga City, Bataan",
//                Font = new Font("Arial", 10),
//                BoundsF = new RectangleF(110, 30, 500, 20)
//            };

//            XRLabel contact = new XRLabel
//            {
//                Text = "0912 345 6789 | email@autotech.com",
//                Font = new Font("Arial", 10),
//                BoundsF = new RectangleF(110, 50, 500, 20)
//            };

//            XRLabel invoiceLabel = new XRLabel
//            {
//                Text = "SALES INVOICE",
//                Font = new Font("Arial", 14, FontStyle.Bold),
//                BoundsF = new RectangleF(0, 110, 650, 30),
//                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
//            };

//            // Invoice Info
//            XRLabel invoiceNumber = new XRLabel
//            {
//                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[strInvoiceNumber]") },
//                Font = new Font("Arial", 10),
//                BoundsF = new RectangleF(0, 150, 325, 20),
//                TextFormatString = "Invoice #: {0}"
//            };

//            XRLabel invoiceDate = new XRLabel
//            {
//                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[DateSold]") },
//                Font = new Font("Arial", 10),
//                BoundsF = new RectangleF(325, 150, 325, 20),
//                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
//                TextFormatString = "Date: {0:MM/dd/yyyy}"
//            };

//            // Customer Info
//            XRLabel customer = new XRLabel
//            {
//                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[AccountName]") },
//                Font = new Font("Arial", 10),
//                BoundsF = new RectangleF(0, 175, 650, 20),
//                TextFormatString = "Customer: {0}"
//            };

//            // Barcode
//            XRBarCode barcode = new XRBarCode
//            {
//                Symbology = new DevExpress.XtraPrinting.BarCode.Code128Generator(),
//                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[strInvoiceNumber]") },
//                AutoModule = true,
//                BoundsF = new RectangleF(0, 195, 200, 40)
//            };

//            // Add header content
//            header.Controls.AddRange(new XRControl[] {
//                logo, companyName, address, contact,
//                invoiceLabel, invoiceNumber, invoiceDate, customer, barcode
//            });

//            // Table header
//            XRTable tableHeader = new XRTable
//            {
//                BoundsF = new RectangleF(0, 0, 650, 25),
//                Borders = DevExpress.XtraPrinting.BorderSide.All,
//                Font = new Font("Arial", 10, FontStyle.Bold),
//            };
//            XRTableRow headerRow = new XRTableRow();
//            headerRow.Cells.AddRange(new[]
//            {
//                new XRTableCell { Text = "Item", WidthF = 300 },
//                new XRTableCell { Text = "Qty", WidthF = 100 },
//                new XRTableCell { Text = "Price", WidthF = 100 },
//                new XRTableCell { Text = "Total", WidthF = 150 }
//            });
//            tableHeader.Rows.Add(headerRow);

//            // Table data
//            XRTable tableData = new XRTable
//            {
//                BoundsF = new RectangleF(0, 0, 650, 25),
//                Borders = DevExpress.XtraPrinting.BorderSide.All,
//                Font = new Font("Arial", 10),
//            };
//            XRTableRow dataRow = new XRTableRow();
//            dataRow.Cells.AddRange(new[]
//            {
//                new XRTableCell { ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[ItemName]") }, WidthF = 300 },
//                new XRTableCell { ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[Quantity]") }, WidthF = 100 },
//                new XRTableCell { ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[ItemPrice]") }, WidthF = 100, TextFormatString = "{0:C}" },
//                new XRTableCell { ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[TotalPrice]") }, WidthF = 150, TextFormatString = "{0:C}" },
//            });
//            tableData.Rows.Add(dataRow);

//            detail.Controls.Add(tableData);

//            // Footer with totals
//            XRLabel subtotal = new XRLabel
//            {
//                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[TotalSales]") },
//                TextFormatString = "Subtotal: {0:C}",
//                BoundsF = new RectangleF(400, 0, 250, 20),
//                Font = new Font("Arial", 10)
//            };

//            XRLabel tax = new XRLabel
//            {
//                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[Tax]") },
//                TextFormatString = "Tax: {0:C}",
//                BoundsF = new RectangleF(400, 20, 250, 20),
//                Font = new Font("Arial", 10)
//            };

//            XRLabel discount = new XRLabel
//            {
//                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[DiscountPeso]") },
//                TextFormatString = "Discount: {0:C}",
//                BoundsF = new RectangleF(400, 40, 250, 20),
//                Font = new Font("Arial", 10)
//            };

//            XRLabel total = new XRLabel
//            {
//                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "[TotalSales] + [Tax] - [DiscountPeso]") },
//                TextFormatString = "TOTAL: {0:C}",
//                BoundsF = new RectangleF(400, 60, 250, 25),
//                Font = new Font("Arial", 12, FontStyle.Bold)
//            };

//            XRLabel footerNote = new XRLabel
//            {
//                Text = "Thank you for your purchase!",
//                Font = new Font("Arial", 10, FontStyle.Italic),
//                BoundsF = new RectangleF(0, 80, 650, 20),
//                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
//            };

//            footer.Controls.AddRange(new[] { subtotal, tax, discount, total, footerNote });

//            // Assemble report
//            this.Bands.AddRange(new Band[] { topMargin, bottomMargin, header, detail, footer });
//            this.Margins = new System.Drawing.Printing.Margins(50, 50, 50, 50);
//            this.DataMember = "PurchasedItems"; // Assumes the report's DataSource is InvoiceDetailsDTO with PurchasedItems list
//        }
//    }
//}
