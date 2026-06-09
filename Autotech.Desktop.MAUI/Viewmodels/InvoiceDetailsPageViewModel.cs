using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Enums;
using Autotech.Desktop.Core.Models;
using Autotech.Desktop.MAUI.Helpers;

namespace Autotech.Desktop.MAUI.Viewmodels;

public sealed class InvoiceDetailsPageViewModel : ViewModelBase
{
    private readonly SalesService _salesService = new();
    private readonly AccountService _accountsService = new();
    private readonly Guid _invoiceId;
    private InvoiceDetailsDTO? _invoice;
    private Accounts? _account;
    private string _title = "Invoice Details";
    private string _customerText = string.Empty;
    private string _dateText = string.Empty;
    private string _statusText = string.Empty;
    private string _originText = string.Empty;
    private string _subtotalText = "0.00";
    private string _taxText = "0.00";
    private string _discountText = "0.00";
    private string _totalText = "0.00";
    private string _paymentAmountText = string.Empty;
    private string? _message;
    private bool _isBusy;
    private PaymentOption? _selectedPaymentMethod;

    public InvoiceDetailsPageViewModel(Guid invoiceId)
    {
        _invoiceId = invoiceId;
        PaymentMethods = EnumHelper.GetPaymentMethodDescriptions()
            .Select(pair => new PaymentOption(pair.Key, pair.Value))
            .ToList();
        SelectedPaymentMethod = PaymentMethods.FirstOrDefault();

        LoadCommand = new Command(async () => await LoadAsync(), () => !IsBusy);
        SaveCommand = new Command(async () => await SaveAsync(), () => !IsBusy && _invoice is not null);
        AddPaymentCommand = new Command(async () => await AddPaymentAsync(), () => !IsBusy && _invoice is not null);
        ConfirmPaymentCommand = new Command(async () => await ConfirmPaymentAsync(), () => !IsBusy && _invoice is not null);
        CancelInvoiceCommand = new Command(async () => await CancelInvoiceAsync(), () => !IsBusy && _invoice is not null);
        CloseCommand = new Command(async () => await Shell.Current.Navigation.PopModalAsync());
        PrintCommand = new Command(async () => await PrintAsync(), () => !IsBusy && _invoice is not null);
    }

    public ObservableCollection<InvoiceItemLine> Items { get; } = new();

    public ObservableCollection<PaymentHistoryLine> Payments { get; } = new();

    public List<PaymentOption> PaymentMethods { get; }

    public ICommand LoadCommand { get; }

    public ICommand SaveCommand { get; }

    public ICommand AddPaymentCommand { get; }

    public ICommand ConfirmPaymentCommand { get; }

    public ICommand CancelInvoiceCommand { get; }

    public ICommand CloseCommand { get; }

    public ICommand PrintCommand { get; }

    public string Title
    {
        get => _title;
        private set => SetProperty(ref _title, value);
    }

    public string CustomerText
    {
        get => _customerText;
        private set => SetProperty(ref _customerText, value);
    }

    public string DateText
    {
        get => _dateText;
        private set => SetProperty(ref _dateText, value);
    }

    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    public string OriginText
    {
        get => _originText;
        private set => SetProperty(ref _originText, value);
    }

    public string SubtotalText
    {
        get => _subtotalText;
        private set => SetProperty(ref _subtotalText, value);
    }

    public string TaxText
    {
        get => _taxText;
        set
        {
            if (SetProperty(ref _taxText, value))
            {
                RecalculateTotals();
            }
        }
    }

    public string DiscountText
    {
        get => _discountText;
        set
        {
            if (SetProperty(ref _discountText, value))
            {
                RecalculateTotals();
            }
        }
    }

    public string TotalText
    {
        get => _totalText;
        private set => SetProperty(ref _totalText, value);
    }

    public string PaymentAmountText
    {
        get => _paymentAmountText;
        set => SetProperty(ref _paymentAmountText, value);
    }

    public PaymentOption? SelectedPaymentMethod
    {
        get => _selectedPaymentMethod;
        set => SetProperty(ref _selectedPaymentMethod, value);
    }

    public string? Message
    {
        get => _message;
        private set
        {
            if (SetProperty(ref _message, value))
            {
                OnPropertyChanged(nameof(HasMessage));
            }
        }
    }

    public bool HasMessage => !string.IsNullOrWhiteSpace(Message);

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
            {
                ((Command)LoadCommand).ChangeCanExecute();
                ((Command)SaveCommand).ChangeCanExecute();
                ((Command)AddPaymentCommand).ChangeCanExecute();
                ((Command)ConfirmPaymentCommand).ChangeCanExecute();
                ((Command)CancelInvoiceCommand).ChangeCanExecute();
                ((Command)PrintCommand).ChangeCanExecute();
            }
        }
    }

    public async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            Message = null;
            _invoice = await _salesService.GetInvoiceByIdAsync(_invoiceId);
            _account = AppLoadService.Current.Context.Accounts.FirstOrDefault(account => account.Id == _invoice.AccountId);

            Title = $"Invoice #: {_invoice.strInvoiceNumber}";
            CustomerText = $"Customer: {_invoice.AccountName}";
            DateText = $"Date: {_invoice.DateSold:d}";
            StatusText = $"Status: {_invoice.Status}";
            OriginText = _invoice.isMobile == true ? "Origin: Mobile" : "Origin: Desktop";
            TaxText = _invoice.Tax.ToString("N2", CultureInfo.CurrentCulture);
            DiscountText = _invoice.DiscountPeso.ToString("N2", CultureInfo.CurrentCulture);

            Replace(Items, _invoice.PurchasedItems.Select(item => new InvoiceItemLine(item, RecalculateTotals)));
            await LoadPaymentsAsync();
            RecalculateTotals();
        }
        catch (Exception ex)
        {
            Message = $"Failed to load invoice: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadPaymentsAsync()
    {
        if (_invoice is null)
        {
            return;
        }

        var payments = await _salesService.GetPaymentsBySaleIdAsync(_invoice.Id);
        Replace(Payments, payments.Select(payment => new PaymentHistoryLine(
            payment.PaymentAmount.ToString("N2", CultureInfo.CurrentCulture),
            payment.DatePaid.ToString("g", CultureInfo.CurrentCulture),
            _invoice.PaymentType,
            payment.RemainingBalance.ToString("N2", CultureInfo.CurrentCulture))));
    }

    private async Task SaveAsync()
    {
        if (_invoice is null)
        {
            return;
        }

        try
        {
            IsBusy = true;
            var tax = ParseNumber(TaxText);
            var discount = ParseNumber(DiscountText);
            var subtotal = Items.Sum(item => item.TotalPrice);
            var total = subtotal + tax - discount;
            var priceBeforeDiscount = total + discount - tax;
            var discountPercent = priceBeforeDiscount == 0 ? 0 : (discount / priceBeforeDiscount) * 100;

            _invoice.Tax = tax;
            _invoice.DiscountPeso = discount;
            _invoice.TotalSales = total;

            var dto = new InvoiceDTO
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
                PurchasedItems = Items.Select(item => new InvoiceItemDTO
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    ItemPrice = item.ItemPrice,
                    TotalPrice = item.TotalPrice,
                    ItemName = item.ItemName,
                    Discount = item.Discount,
                    AgentId = SessionManager.AgentDetails.Id
                }).ToList()
            };

            await _salesService.UpdateInvoiceAsync(_invoice.Id, dto);
            Message = "Invoice saved successfully.";
        }
        catch (Exception ex)
        {
            Message = $"Failed to save invoice: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task PrintAsync()
    {
        if (_invoice is null)
        {
            return;
        }

        try
        {
            IsBusy = true;
            Message = null;
            _account ??= await _accountsService.GetAccountByIdAsync(_invoice.AccountId);
            var savePath = await ReceiptPrintHelper.PrintInvoiceAsync(_invoice, _account);
            Message = $"Receipt saved to: {savePath}";
        }
        catch (Exception ex)
        {
            Message = $"Failed to print invoice: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task AddPaymentAsync()
    {
        if (_invoice is null || SelectedPaymentMethod is null)
        {
            return;
        }

        var amount = ParseNumber(PaymentAmountText);
        if (amount <= 0)
        {
            Message = "Enter a payment amount first.";
            return;
        }

        try
        {
            IsBusy = true;
            var payment = new PaymentHistoryDTO
            {
                SalesId = _invoice.Id,
                AccountId = _invoice.AccountId,
                AgentId = SessionManager.AgentDetails.Id,
                DatePaid = DateTime.Now,
                PaymentAmount = Math.Round(amount),
                PaymentMethod = SelectedPaymentMethod.Method.ToString(),
                RemainingBalance = Math.Round(_invoice.RemainingBalance - amount)
            };

            await _salesService.AddPaymentAsync(payment);
            _invoice = await _salesService.GetInvoiceByIdAsync(_invoice.Id);
            PaymentAmountText = string.Empty;
            await LoadPaymentsAsync();
            RecalculateTotals();
            Message = "Payment recorded successfully.";
        }
        catch (Exception ex)
        {
            Message = $"Failed to record payment: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ConfirmPaymentAsync()
    {
        if (_invoice is null)
        {
            return;
        }

        try
        {
            IsBusy = true;
            var payments = await _salesService.GetPaymentsBySaleIdAsync(_invoice.Id);
            var updatedInvoice = await _salesService.GetInvoiceByIdAsync(_invoice.Id);
            var totalPaid = Math.Round(payments.Sum(payment => payment.PaymentAmount));
            var newStatus = totalPaid > _invoice.TotalSales || updatedInvoice.RemainingBalance == 0
                ? "Fully paid"
                : "Incomplete";

            await _salesService.ConfirmPaymentStatusAsync(_invoice.Id, newStatus);
            _invoice.Status = newStatus;
            StatusText = $"Status: {newStatus}";
            await LoadPaymentsAsync();
            Message = $"Invoice status updated to: {newStatus}";
        }
        catch (Exception ex)
        {
            Message = $"Error confirming payment: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CancelInvoiceAsync()
    {
        if (_invoice is null)
        {
            return;
        }

        var confirm = await Shell.Current.DisplayAlertAsync("Confirm Cancel", "Are you sure you want to cancel this invoice?", "Yes", "No");
        if (!confirm)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await _salesService.ConfirmPaymentStatusAsync(_invoice.Id, "Denied");
            _invoice.Status = "Denied";
            StatusText = "Status: Denied";
            Message = "Invoice status updated to 'Denied'.";
        }
        catch (Exception ex)
        {
            Message = $"Failed to cancel invoice: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void RecalculateTotals()
    {
        var subtotal = Items.Sum(item => item.TotalPrice);
        var tax = ParseNumber(TaxText);
        var discount = ParseNumber(DiscountText);
        SubtotalText = subtotal.ToString("N2", CultureInfo.CurrentCulture);
        TotalText = (subtotal + tax - discount).ToString("N2", CultureInfo.CurrentCulture);
    }

    private static double ParseNumber(string? text)
    {
        if (double.TryParse(text, NumberStyles.Currency, CultureInfo.CurrentCulture, out var value))
        {
            return value;
        }

        return double.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value) ? value : 0;
    }

    private static void Replace<T>(ObservableCollection<T> target, IEnumerable<T> values)
    {
        target.Clear();
        foreach (var value in values)
        {
            target.Add(value);
        }
    }

    public sealed class InvoiceItemLine : ViewModelBase
    {
        private readonly Action _changed;
        private double _itemPrice;
        private double _discount;

        public InvoiceItemLine(PurchasedItemDetailsDTO item, Action changed)
        {
            _changed = changed;
            ItemId = item.ItemId;
            ItemName = item.ItemName;
            Quantity = item.Quantity;
            _itemPrice = item.ItemPrice ?? 0;
            _discount = item.Discount ?? 0;
        }

        public Guid ItemId { get; }

        public string ItemName { get; }

        public double Quantity { get; }

        public double ItemPrice
        {
            get => _itemPrice;
            private set
            {
                if (SetProperty(ref _itemPrice, value))
                {
                    OnPropertyChanged(nameof(ItemPriceText));
                    OnPropertyChanged(nameof(TotalPrice));
                    OnPropertyChanged(nameof(TotalPriceText));
                    _changed();
                }
            }
        }

        public string ItemPriceText
        {
            get => ItemPrice.ToString("N2", CultureInfo.CurrentCulture);
            set => ItemPrice = ParseNumber(value);
        }

        public double Discount
        {
            get => _discount;
            private set
            {
                if (SetProperty(ref _discount, value))
                {
                    OnPropertyChanged(nameof(DiscountText));
                    OnPropertyChanged(nameof(TotalPrice));
                    OnPropertyChanged(nameof(TotalPriceText));
                    _changed();
                }
            }
        }

        public string DiscountText
        {
            get => Discount.ToString("N2", CultureInfo.CurrentCulture);
            set => Discount = ParseNumber(value);
        }

        public double TotalPrice => ItemPrice * Quantity - Discount;

        public string TotalPriceText => TotalPrice.ToString("N2", CultureInfo.CurrentCulture);
    }

    public sealed record PaymentHistoryLine(string AmountPaid, string DatePaid, string PaymentMethod, string RemainingBalance);

    public sealed class PaymentOption(PaymentMethod method, string description)
    {
        public PaymentMethod Method { get; } = method;

        public string Description { get; } = description;
    }
}
