using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Models;
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
    public partial class ImportExcelForm : MetroSetForm
    {
        private List<Items> itemsToImport = new();
        public ImportExcelForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreateTemplate_Click(object sender, EventArgs e)
        {
            var dt = new DataTable();
            dt.Columns.Add("ItemCode");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("ItemDescription");
            dt.Columns.Add("OnHand");
            dt.Columns.Add("BataanRetail");
            dt.Columns.Add("BataanWholeSale");
            dt.Columns.Add("PampangaRetail");
            dt.Columns.Add("PampangaWholeSale");
            dt.Columns.Add("ZambalesRetail");
            dt.Columns.Add("ZambalesWholeSale");

            using var wb = new ClosedXML.Excel.XLWorkbook();
            var ws = wb.Worksheets.Add(dt, "Template");
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ItemTemplate.xlsx");
            wb.SaveAs(path);
            MessageBox.Show($"Template saved to: {path}");
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.Filter = "Excel Files|*.xlsx";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    using var wb = new ClosedXML.Excel.XLWorkbook(filePath);
                    var ws = wb.Worksheet(1);
                    itemsToImport.Clear();

                    foreach (var row in ws.RowsUsed().Skip(1))
                    {
                        var item = new Items
                        {
                            ItemCode = row.Cell(1).GetString(),
                            ItemName = row.Cell(2).GetString(),
                            ItemDescription = row.Cell(3).GetString(),
                            itemDetails = new ItemDetails
                            {
                                ItemsSold = row.Cell(4).GetDouble(),
                                Sales= row.Cell(6).GetDouble(),
                                OnHand = row.Cell(5).GetDouble(),
                                BataanRetail = row.Cell(7).GetDouble(),
                                BataanWholeSale = row.Cell(8).GetDouble(),
                                PampangaRetail = row.Cell(9).GetDouble(),
                                PampangaWholeSale = row.Cell(10).GetDouble(),
                                ZambalesRetail = row.Cell(11).GetDouble(),
                                ZambalesWholeSale = row.Cell(12).GetDouble()
                            }
                        };
                        itemsToImport.Add(item);
                    }

                    dtgPreview.DataSource = itemsToImport.Select(i => new
                    {
                        i.ItemCode,
                        i.ItemName,
                        i.ItemDescription,
                        i.itemDetails.OnHand,
                        i.itemDetails.BataanRetail,
                        i.itemDetails.BataanWholeSale,
                        i.itemDetails.PampangaRetail,
                        i.itemDetails.PampangaWholeSale,
                        i.itemDetails.ZambalesRetail,
                        i.itemDetails.ZambalesWholeSale
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            var service = new ItemServices();

            var requestDtos = itemsToImport.Select(i => new ItemRequestDto
            {
                ItemCode = i.ItemCode,
                ItemName = i.ItemName,
                ItemDescription = i.ItemDescription,
                OnHand = i.itemDetails?.OnHand ?? 0,
                ItemsSold = i.itemDetails?.ItemsSold ?? 0,
                Sales = i.itemDetails?.Sales ?? 0,
                BataanRetail = i.itemDetails?.BataanRetail ?? 0,
                BataanWholeSale = i.itemDetails?.BataanWholeSale ?? 0,
                PampangaRetail = i.itemDetails?.PampangaRetail ?? 0,
                PampangaWholeSale = i.itemDetails?.PampangaWholeSale ?? 0,
                ZambalesRetail = i.itemDetails?.ZambalesRetail ?? 0,
                ZambalesWholeSale = i.itemDetails?.ZambalesWholeSale ?? 0
            }).ToList();

            try
            {
                bool success = await service.CreateBulkItemsAsync(requestDtos);
                if (success)
                {
                    MessageBox.Show("Items imported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Import failed. Check your data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during import: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
