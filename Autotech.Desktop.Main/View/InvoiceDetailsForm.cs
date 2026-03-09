using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Models;
using Autotech.Desktop.Main.Helpers;
using MetroSet_UI.Forms;

namespace Autotech.Desktop.Main.View
{
    public partial class InvoiceDetailsForm : MetroSetForm
    {
        private InvoiceDetailsDTO _invoice;
        private readonly MainForm _mainForm;
        private List<PaymentHistoryDTO> paymentHistoryDTOs;
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

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            var reportHelper = new ReportHelper();
            await reportHelper.PrintInvoiceAsync(_invoice, _accounts);
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
