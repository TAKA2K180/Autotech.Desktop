using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Models;
using MetroSet_UI.Forms;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autotech.Desktop.Main.View
{
    public partial class ItemSalesReportForm : MetroSetForm
    {
        private List<SalesDTO> _invoices;

        public ItemSalesReportForm()
        {
            InitializeComponent();
            LoadMonths();
        }

        private async void LoadMonths()
        {
            DateTimePicker dtpMonthYear = new DateTimePicker();
            dtpMonthYear.Format = DateTimePickerFormat.Custom;
            dtpMonthYear.CustomFormat = "MMMM yyyy";  // Displays like "June 2025"
            dtpMonthYear.ShowUpDown = true; // disables calendar dropdown

            var service = new SalesService();
            _invoices = await service.GetAllInvoicesAsync();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtmFrom.Value.Date;
            DateTime toDate = dtmTo.Value.Date;

            if (fromDate > toDate)
            {
                MessageBox.Show("From date must not be later than To date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var filtered = _invoices
                .Where(i => i.DateSold.Date >= fromDate && i.DateSold.Date <= toDate)
                .SelectMany(i => i.PurchasedItems)
                .GroupBy(item => item.ItemName)
                .Select(group => new
                {
                    Item = group.Key,
                    TotalQty = group.Sum(x => x.Quantity),
                    TotalSales = group.Sum(x => x.TotalPrice)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToList();

            dtgItemSales.DataSource = filtered;
            dtgItemSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dtgItemSales.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }


        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dtgItemSales.DataSource is null)
            {
                MessageBox.Show("Nothing to export.");
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Workbook|*.xlsx",
                FileName = $"ItemSales_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Sales Per Item");

                // Define user-friendly names
                Dictionary<string, string> columnMap = new Dictionary<string, string>
        {
            { "Item", "Item Name" },
            { "TotalQty", "Total Quantity Sold" },
            { "TotalSales", "Total Sales (₱)" }
        };

                // Write headers
                for (int i = 0; i < dtgItemSales.Columns.Count; i++)
                {
                    string originalName = dtgItemSales.Columns[i].Name;
                    worksheet.Cells[1, i + 1].Value = columnMap.ContainsKey(originalName)
                        ? columnMap[originalName]
                        : dtgItemSales.Columns[i].HeaderText;
                }

                // Write data rows
                for (int row = 0; row < dtgItemSales.Rows.Count; row++)
                {
                    for (int col = 0; col < dtgItemSales.Columns.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = dtgItemSales.Rows[row].Cells[col].Value;
                    }
                }

                // Optional: auto-fit columns
                worksheet.Cells.AutoFitColumns();

                package.SaveAs(new System.IO.FileInfo(sfd.FileName));
                MessageBox.Show("Exported successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
