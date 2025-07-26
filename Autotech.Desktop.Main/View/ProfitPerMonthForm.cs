using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using ClosedXML.Excel;
using MetroSet_UI.Extensions;
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
    public partial class ProfitPerMonthForm : MetroSetForm
    {
        public ProfitPerMonthForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnCalculate_Click(object sender, EventArgs e)
        {
            if (cmbMonth.SelectedIndex < 0 || cmbYear.SelectedIndex < 0)
            {
                MessageBox.Show("Please select both month and year.");
                return;
            }

            int month = cmbMonth.SelectedIndex + 1;
            int year = (int)cmbYear.SelectedItem;

            ToastMessageForm loadingToast = null;

            try
            {
                loadingToast = new ToastMessageForm("Calculating profit...");
                loadingToast.Show();
                loadingToast.TopMost = true;
                loadingToast.BringToFront();

                var salesService = new SalesService();
                var locationService = new LocationServices();

                var allSales = await salesService.GetAllInvoicesAsync();
                var locations = await locationService.GetAllLocationsAsync();

                var filteredSales = allSales
                    .Where(s => s.DateSold.Month == month && s.DateSold.Year == year)
                    .ToList();

                // Create structure: LocationName -> { liters, sales, cost }
                var result = new Dictionary<string, (double Liters, double TotalSales, double Cost)>();

                foreach (var location in locations)
                {
                    var salesForLocation = filteredSales
                        .Where(s => s.LocationId == location.Id)
                        .Where(st => st.Status == "Fully paid" || st.Status == "Incomplete")
                        .ToList();

                    double liters = salesForLocation.Sum(s => s.TotalLiters);
                    double totalSales = salesForLocation.Sum(s => s.TotalSales);
                    double cost = liters * location.CostPerLiter;

                    result[location.LocationName] = (liters, totalSales, cost);
                }

                // Build DataTable for display
                DataTable table = new DataTable();
                table.Columns.Add("Metric");

                foreach (var locationName in result.Keys)
                    table.Columns.Add(locationName);

                // Fill metrics
                var rowLiters = table.NewRow();
                rowLiters["Metric"] = "Liters per month";

                var rowGrossProfit = table.NewRow();
                rowGrossProfit["Metric"] = "Gross Profit per month";

                foreach (var kvp in result)
                {
                    rowLiters[kvp.Key] = kvp.Value.Liters.ToString("N2");
                    double grossProfit = kvp.Value.TotalSales - kvp.Value.Cost;
                    rowGrossProfit[kvp.Key] = grossProfit.ToString("C2");
                }

                table.Rows.Add(rowLiters);
                table.Rows.Add(rowGrossProfit);
                table.Rows.Add(table.NewRow()["Metric"] = "Liters paid");
                dtgResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                table.Rows.Add(table.NewRow()["Metric"] = "Paid DR");
                table.Rows.Add(table.NewRow()["Metric"] = "Expenses");
                table.Rows.Add(table.NewRow()["Metric"] = "Net Profit Claimed");

                // Display
                dtgResult.DataSource = null;
                dtgResult.AutoGenerateColumns = true;
                dtgResult.DataSource = table;

                dtgResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dtgResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dtgResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            }
            catch (Exception ex)
            {
                LogHelper.Log("Error: ", ex);
                MessageBox.Show($"Failed to calculate profit.\r\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (loadingToast != null && !loadingToast.IsDisposed)
                {
                    loadingToast.Close();
                }
            }
        }


        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dtgResult.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Workbook|*.xlsx",
                FileName = $"ProfitReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Profit Report");

                            // Add headers
                            for (int col = 0; col < dtgResult.Columns.Count; col++)
                            {
                                worksheet.Cell(1, col + 1).Value = dtgResult.Columns[col].HeaderText;
                                worksheet.Cell(1, col + 1).Style.Font.Bold = true;
                            }

                            // Add rows
                            for (int row = 0; row < dtgResult.Rows.Count; row++)
                            {
                                for (int col = 0; col < dtgResult.Columns.Count; col++)
                                {
                                    var value = dtgResult.Rows[row].Cells[col].Value;
                                    worksheet.Cell(row + 2, col + 1).SetValue(value?.ToString() ?? "");
                                }
                            }

                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Export successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log("Error: ", ex);
                        MessageBox.Show($"Failed to export:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ProfitPerMonthForm_Load(object sender, EventArgs e)
        {
            // Fill month names
            cmbMonth.Items.AddRange(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
                .Where(m => !string.IsNullOrEmpty(m)).ToArray());

            // Fill years from 2020 to next year
            int currentYear = DateTime.Now.Year;
            for (int year = 2025; year <= currentYear + 15; year++)
            {
                cmbYear.Items.Add(year);
            }

            // Optional: set current month/year as selected
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
            cmbYear.SelectedItem = currentYear;
        }
    }
}
