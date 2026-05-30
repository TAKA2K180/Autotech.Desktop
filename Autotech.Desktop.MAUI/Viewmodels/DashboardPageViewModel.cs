using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Enums;
using Autotech.Desktop.Core.Models;

namespace Autotech.Desktop.MAUI.Viewmodels;

public sealed class DashboardPageViewModel : ViewModelBase
{
    private readonly AppLoadService _loadService = AppLoadService.Current;
    private readonly SalesService _salesService = new();
    private readonly AgentsService _agentsService = new();
    private readonly ItemServices _itemServices = new();
    private readonly List<Items> _currentPageItems = new();
    private readonly List<Accounts> _allAccounts = new();
    private readonly List<SalesDTO> _allInvoices = new();
    private readonly List<AgentDTO> _allAgents = new();
    private readonly List<Items> _maintenanceItems = new();
    private int _currentItemPage = 1;
    private string _agentName = "Autotech";
    private string _agentMeta = string.Empty;
    private string _salesClock = string.Empty;
    private string _selectedTab = "POS";
    private string _itemsCount = "0";
    private string _accountsCount = "0";
    private string _invoicesCount = "0";
    private string _itemsPageText = "Page 1";
    private string? _itemSearchText;
    private string? _accountSearchText;
    private string? _invoiceSearchText;
    private InvoiceFilterChoice? _selectedInvoiceFilter;
    private DateTime _invoiceDateFrom = DateTime.Today;
    private DateTime _invoiceDateTo = DateTime.Today;
    private string _invoiceDateSortOption = "Descending";
    private string? _maintenanceAccountSearchText;
    private string? _maintenanceItemSearchText;
    private string _pricingMode = "Retail";
    private string _selectedLocation = "Bataan";
    private Accounts? _selectedAccount;
    private PaymentOption? _selectedPaymentMethod;
    private string _contactNumber = string.Empty;
    private string _terms = "0";
    private string _taxText = string.Empty;
    private string _invoiceDiscountText = string.Empty;
    private string _paidAmountText = string.Empty;
    private string _subtotalText = "0.00";
    private string _totalText = "0.00";
    private string _changeText = "0.00";
    private string _remainingText = "0.00";
    private string? _operationMessage;
    private bool _isBusy;
    private bool _maintenanceLoaded;

    public DashboardPageViewModel()
    {
        SelectTabCommand = new Command<string>(async tab => await SelectTabAsync(tab));
        RefreshCommand = new Command(async () => await LoadAsync(forceReload: true), () => !IsBusy);
        PreviousItemsCommand = new Command(async () => await PreviousItemsAsync(), () => !IsBusy && _currentItemPage > 1);
        NextItemsCommand = new Command(async () => await NextItemsAsync(), () => !IsBusy);
        AddItemCommand = new Command<Items>(AddItemToCart);
        RemoveCartItemCommand = new Command<CartLine>(RemoveCartItem);
        RemoveSelectedCartItemsCommand = new Command(RemoveSelectedCartItems);
        ClearCartCommand = new Command(ClearCart);
        PayCommand = new Command(async () => await PayAsync(), () => !IsBusy);
        LogoutCommand = new Command(async () => await LogoutAsync());
        SearchInvoicesCommand = new Command(ApplyInvoiceFilter);

        PaymentMethods = EnumHelper.GetPaymentMethodDescriptions()
            .Select(pair => new PaymentOption(pair.Key, pair.Value))
            .ToList();
        SelectedPaymentMethod = PaymentMethods.FirstOrDefault();
        SelectedInvoiceFilter = InvoiceFilterOptions.FirstOrDefault();
    }

    public ObservableCollection<Items> Items { get; } = new();

    public ObservableCollection<Accounts> Accounts { get; } = new();

    public ObservableCollection<InvoiceRow> Invoices { get; } = new();

    public ObservableCollection<CartLine> CartItems { get; } = new();

    public ObservableCollection<Accounts> MaintenanceAccounts { get; } = new();

    public ObservableCollection<AgentDTO> MaintenanceAgents { get; } = new();

    public ObservableCollection<Items> MaintenanceItems { get; } = new();

    public List<PaymentOption> PaymentMethods { get; }

    public List<string> PricingModes { get; } = ["Retail", "Wholesale"];

    public List<string> Locations { get; } = ["Bataan", "Zambales", "Pampanga"];

    public List<InvoiceFilterChoice> InvoiceFilterOptions { get; } =
        Enum.GetValues(typeof(InvoiceFilterOption))
            .Cast<InvoiceFilterOption>()
            .Select(option => new InvoiceFilterChoice(option, GetInvoiceFilterDescription(option)))
            .ToList();

    public List<string> InvoiceDateSortOptions { get; } = ["Descending", "Ascending"];

    public ICommand SelectTabCommand { get; }

    public ICommand RefreshCommand { get; }

    public ICommand PreviousItemsCommand { get; }

    public ICommand NextItemsCommand { get; }

    public ICommand AddItemCommand { get; }

    public ICommand RemoveCartItemCommand { get; }

    public ICommand RemoveSelectedCartItemsCommand { get; }

    public ICommand ClearCartCommand { get; }

    public ICommand PayCommand { get; }

    public ICommand LogoutCommand { get; }

    public ICommand SearchInvoicesCommand { get; }

    public string AgentName
    {
        get => _agentName;
        private set => SetProperty(ref _agentName, value);
    }

    public string AgentMeta
    {
        get => _agentMeta;
        private set => SetProperty(ref _agentMeta, value);
    }

    public string SalesClock
    {
        get => _salesClock;
        private set => SetProperty(ref _salesClock, value);
    }

    public string SelectedTab
    {
        get => _selectedTab;
        private set
        {
            if (SetProperty(ref _selectedTab, value))
            {
                OnPropertyChanged(nameof(IsPosTab));
                OnPropertyChanged(nameof(IsInvoiceTab));
                OnPropertyChanged(nameof(IsMaintenanceTab));
            }
        }
    }

    public bool IsPosTab => SelectedTab == "POS";

    public bool IsInvoiceTab => SelectedTab == "Invoice";

    public bool IsMaintenanceTab => SelectedTab == "Maintenance";

    public bool CanAccessMaintenance => SessionManager.AgentDetails?.AgentRole == "Admin";

    public string ItemsCount
    {
        get => _itemsCount;
        private set => SetProperty(ref _itemsCount, value);
    }

    public string AccountsCount
    {
        get => _accountsCount;
        private set => SetProperty(ref _accountsCount, value);
    }

    public string InvoicesCount
    {
        get => _invoicesCount;
        private set => SetProperty(ref _invoicesCount, value);
    }

    public string ItemsPageText
    {
        get => _itemsPageText;
        private set => SetProperty(ref _itemsPageText, value);
    }

    public string? ItemSearchText
    {
        get => _itemSearchText;
        set
        {
            if (SetProperty(ref _itemSearchText, value))
            {
                ApplyItemFilter();
            }
        }
    }

    public string? AccountSearchText
    {
        get => _accountSearchText;
        set
        {
            if (SetProperty(ref _accountSearchText, value))
            {
                ApplyAccountFilter();
            }
        }
    }

    public string? InvoiceSearchText
    {
        get => _invoiceSearchText;
        set
        {
            if (SetProperty(ref _invoiceSearchText, value))
            {
                ApplyInvoiceFilter();
            }
        }
    }

    public InvoiceFilterChoice? SelectedInvoiceFilter
    {
        get => _selectedInvoiceFilter;
        set
        {
            if (SetProperty(ref _selectedInvoiceFilter, value))
            {
                OnPropertyChanged(nameof(IsInvoiceDateFilter));
                ApplyInvoiceFilter();
            }
        }
    }

    public bool IsInvoiceDateFilter => SelectedInvoiceFilter?.Key is InvoiceFilterOption.DateSold or InvoiceFilterOption.DueDate;

    public DateTime InvoiceDateFrom
    {
        get => _invoiceDateFrom;
        set
        {
            if (SetProperty(ref _invoiceDateFrom, value))
            {
                ApplyInvoiceFilter();
            }
        }
    }

    public DateTime InvoiceDateTo
    {
        get => _invoiceDateTo;
        set
        {
            if (SetProperty(ref _invoiceDateTo, value))
            {
                ApplyInvoiceFilter();
            }
        }
    }

    public string InvoiceDateSortOption
    {
        get => _invoiceDateSortOption;
        set
        {
            if (SetProperty(ref _invoiceDateSortOption, value))
            {
                ApplyInvoiceFilter();
            }
        }
    }

    public string? MaintenanceAccountSearchText
    {
        get => _maintenanceAccountSearchText;
        set
        {
            if (SetProperty(ref _maintenanceAccountSearchText, value))
            {
                ApplyMaintenanceAccountFilter();
            }
        }
    }

    public string? MaintenanceItemSearchText
    {
        get => _maintenanceItemSearchText;
        set
        {
            if (SetProperty(ref _maintenanceItemSearchText, value))
            {
                ApplyMaintenanceItemFilter();
            }
        }
    }

    public string PricingMode
    {
        get => _pricingMode;
        set
        {
            if (SetProperty(ref _pricingMode, value))
            {
                RefreshCartPrices();
            }
        }
    }

    public string SelectedLocation
    {
        get => _selectedLocation;
        set
        {
            if (SetProperty(ref _selectedLocation, value))
            {
                RefreshCartPrices();
            }
        }
    }

    public Accounts? SelectedAccount
    {
        get => _selectedAccount;
        set
        {
            if (SetProperty(ref _selectedAccount, value))
            {
                ContactNumber = value?.ContactNumber ?? string.Empty;
                Terms = (value?.Terms ?? 0).ToString(CultureInfo.InvariantCulture);
            }
        }
    }

    public PaymentOption? SelectedPaymentMethod
    {
        get => _selectedPaymentMethod;
        set => SetProperty(ref _selectedPaymentMethod, value);
    }

    public string ContactNumber
    {
        get => _contactNumber;
        private set => SetProperty(ref _contactNumber, value);
    }

    public string Terms
    {
        get => _terms;
        set => SetProperty(ref _terms, value);
    }

    public string TaxText
    {
        get => _taxText;
        set
        {
            if (SetProperty(ref _taxText, value))
            {
                CalculateTotals();
            }
        }
    }

    public string InvoiceDiscountText
    {
        get => _invoiceDiscountText;
        set
        {
            if (SetProperty(ref _invoiceDiscountText, value))
            {
                CalculateTotals();
            }
        }
    }

    public string PaidAmountText
    {
        get => _paidAmountText;
        set
        {
            if (SetProperty(ref _paidAmountText, value))
            {
                CalculateTotals();
            }
        }
    }

    public string SubtotalText
    {
        get => _subtotalText;
        private set => SetProperty(ref _subtotalText, value);
    }

    public string TotalText
    {
        get => _totalText;
        private set => SetProperty(ref _totalText, value);
    }

    public string ChangeText
    {
        get => _changeText;
        private set => SetProperty(ref _changeText, value);
    }

    public string RemainingText
    {
        get => _remainingText;
        private set => SetProperty(ref _remainingText, value);
    }

    public string? OperationMessage
    {
        get => _operationMessage;
        private set
        {
            if (SetProperty(ref _operationMessage, value))
            {
                OnPropertyChanged(nameof(HasOperationMessage));
            }
        }
    }

    public bool HasOperationMessage => !string.IsNullOrWhiteSpace(OperationMessage);

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
            {
                ((Command)RefreshCommand).ChangeCanExecute();
                ((Command)PreviousItemsCommand).ChangeCanExecute();
                ((Command)NextItemsCommand).ChangeCanExecute();
                ((Command)PayCommand).ChangeCanExecute();
            }
        }
    }

    public async Task LoadAsync(bool forceReload = false)
    {
        IsBusy = true;
        OperationMessage = null;

        try
        {
            UpdateAgentHeader();
            UpdateClock();
            if (forceReload || !_loadService.Context.HasLoaded)
            {
                await _loadService.LoadInitialDataAsync();
            }

            LoadFromContext();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Unable to load dashboard: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void UpdateClock()
    {
        SalesClock = $"{TimeHelper.GetPhilippineTime():ddd, dd MMM yyyy HH:mm:ss}";
    }

    public void ApplyItemFilter()
    {
        var keyword = ItemSearchText?.Trim() ?? string.Empty;
        // Use the full item set when searching so user can find items not present on current page
        if (string.IsNullOrWhiteSpace(keyword))
        {
            // When search is cleared, restore the last paged items and page text
            Replace(Items, _currentPageItems);
            ItemsPageText = $"Page {_currentItemPage}";
        }
        else
        {
            var source = _loadService.Context.AllItems.Count > 0 ? _loadService.Context.AllItems : _currentPageItems;
            var filtered = source.Where(item =>
                Contains(item.ItemName, keyword) ||
                Contains(item.ItemCode, keyword) ||
                Contains(item.ItemDescription, keyword));

            Replace(Items, filtered);
        }

        ItemsCount = Items.Count.ToString("N0");
    }

    public void ApplyAccountFilter()
    {
        var keyword = AccountSearchText?.Trim() ?? string.Empty;
        var filtered = string.IsNullOrWhiteSpace(keyword)
            ? _allAccounts
            : _allAccounts.Where(account =>
                Contains(account.Name, keyword) ||
                Contains(account.Cluster, keyword) ||
                Contains(account.ContactPerson, keyword));

        Replace(Accounts, filtered);
        AccountsCount = _allAccounts.Count.ToString("N0");
    }

    public void ApplyInvoiceFilter()
    {
        var keyword = InvoiceSearchText?.Trim() ?? string.Empty;
        var selectedFilter = SelectedInvoiceFilter?.Key ?? InvoiceFilterOption.InvoiceNumber;
        IEnumerable<SalesDTO> filtered = _allInvoices;

        if (selectedFilter is InvoiceFilterOption.DateSold or InvoiceFilterOption.DueDate)
        {
            var from = InvoiceDateFrom.Date;
            var to = InvoiceDateTo.Date;
            filtered = selectedFilter == InvoiceFilterOption.DateSold
                ? filtered.Where(invoice => invoice.DateSold.Date >= from && invoice.DateSold.Date <= to)
                : filtered.Where(invoice => invoice.DueDate.Date >= from && invoice.DueDate.Date <= to);

            filtered = InvoiceDateSortOption == "Ascending"
                ? (selectedFilter == InvoiceFilterOption.DateSold ? filtered.OrderBy(invoice => invoice.DateSold) : filtered.OrderBy(invoice => invoice.DueDate))
                : (selectedFilter == InvoiceFilterOption.DateSold ? filtered.OrderByDescending(invoice => invoice.DateSold) : filtered.OrderByDescending(invoice => invoice.DueDate));
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filtered = selectedFilter switch
                {
                    InvoiceFilterOption.InvoiceNumber => filtered.Where(invoice => Contains(invoice.strInvoiceNumber, keyword)),
                    InvoiceFilterOption.Agent => filtered.Where(invoice => Contains(invoice.Agent, keyword)),
                    InvoiceFilterOption.AccountName => filtered.Where(invoice => Contains(invoice.AccountName, keyword)),
                    InvoiceFilterOption.PaymentType => filtered.Where(invoice => Contains(invoice.PaymentType, keyword)),
                    InvoiceFilterOption.Status => filtered.Where(invoice => Contains(invoice.Status, keyword)),
                    InvoiceFilterOption.Cluster => filtered.Where(invoice => Contains(invoice.Cluster, keyword)),
                    _ => filtered
                };
            }

            filtered = filtered.OrderByDescending(invoice => invoice.DateSold);
        }

        Replace(Invoices, filtered.Select(invoice => new InvoiceRow(invoice)));
        InvoicesCount = Invoices.Count.ToString("N0");
    }

    public void ApplyMaintenanceAccountFilter()
    {
        var keyword = MaintenanceAccountSearchText?.Trim() ?? string.Empty;
        var filtered = string.IsNullOrWhiteSpace(keyword)
            ? _allAccounts
            : _allAccounts.Where(account =>
                Contains(account.Name, keyword) ||
                Contains(account.ContactPerson, keyword) ||
                Contains(account.Address, keyword));

        Replace(MaintenanceAccounts, filtered);
    }

    public void ApplyMaintenanceItemFilter()
    {
        var keyword = MaintenanceItemSearchText?.Trim() ?? string.Empty;
        var filtered = string.IsNullOrWhiteSpace(keyword)
            ? _maintenanceItems
            : _maintenanceItems.Where(item =>
                Contains(item.ItemName, keyword) ||
                Contains(item.ItemCode, keyword) ||
                Contains(item.ItemDescription, keyword));

        Replace(MaintenanceItems, filtered);
    }

    private async Task SelectTabAsync(string? tab)
    {
        if (string.IsNullOrWhiteSpace(tab))
        {
            return;
        }

        if (tab == "Maintenance" && !CanAccessMaintenance)
        {
            OperationMessage = "Maintenance is available to admin users only.";
            return;
        }

        SelectedTab = tab;

        if (tab == "Invoice")
        {
            ApplyInvoiceFilter();
        }
        else if (tab == "Maintenance" && !_maintenanceLoaded)
        {
            await LoadMaintenanceAsync();
        }
    }

    private async Task PreviousItemsAsync()
    {
        if (_currentItemPage <= 1)
        {
            return;
        }

        _currentItemPage--;
        await LoadItemsAsync();
    }

    private async Task NextItemsAsync()
    {
        _currentItemPage++;
        await LoadItemsAsync();
    }

    private void AddItemToCart(Items? item)
    {
        if (item is null || CartItems.Any(cartItem => cartItem.Item.Id == item.Id))
        {
            return;
        }

        CartItems.Add(new CartLine(item, GetPriceBasedOnSelection(item), CalculateTotals));
        CalculateTotals();
    }

    private void RemoveCartItem(CartLine? cartLine)
    {
        if (cartLine is null)
        {
            return;
        }

        CartItems.Remove(cartLine);
        CalculateTotals();
    }

    private void RemoveSelectedCartItems()
    {
        foreach (var cartLine in CartItems.Where(line => line.IsSelected).ToList())
        {
            CartItems.Remove(cartLine);
        }

        CalculateTotals();
    }

    private void ClearCart()
    {
        CartItems.Clear();
        TaxText = string.Empty;
        InvoiceDiscountText = string.Empty;
        PaidAmountText = string.Empty;
        CalculateTotals();
    }

    private async Task PayAsync()
    {
        OperationMessage = null;

        if (SelectedAccount is null)
        {
            OperationMessage = "Please select an account.";
            return;
        }

        if (SelectedPaymentMethod is null)
        {
            OperationMessage = "Please select a payment method.";
            return;
        }

        if (CartItems.Count == 0)
        {
            OperationMessage = "Cart is empty.";
            return;
        }

        IsBusy = true;

        try
        {
            var total = ParseCurrency(TotalText);
            var tax = ParseCurrency(TaxText);
            var discount = ParseCurrency(InvoiceDiscountText);
            var remaining = ParseCurrency(RemainingText);
            var priceBeforeDiscount = total + discount - tax;
            var discountPercent = priceBeforeDiscount == 0 ? 0 : discount / priceBeforeDiscount * 100;
            var terms = int.TryParse(Terms, out var termsValue) ? termsValue : 0;
            var now = TimeHelper.GetPhilippineTime();

            var invoiceDto = new InvoiceDTO
            {
                DateSold = now,
                Agent = SessionManager.AgentDetails.AgentName,
                DiscountPercent = (double)discountPercent,
                DiscountPeso = (double)discount,
                Tax = (double)tax,
                TotalSales = (double)total,
                AccountName = SelectedAccount.Name,
                PaymentType = SelectedPaymentMethod.Method.ToString(),
                Terms = terms,
                DueDate = now.AddDays(terms),
                RemainingBalance = Math.Round((double)remaining),
                Status = "For approval",
                TotalLiters = 0,
                Cluster = SelectedAccount.Cluster ?? string.Empty,
                AccountId = SelectedAccount.Id,
                LocationId = SelectedAccount.LocationId,
                strInvoiceNumber = string.Empty,
                PurchasedItems = CartItems.Select(line => new InvoiceItemDTO
                {
                    ItemId = line.Item.Id,
                    Quantity = (double)line.Quantity,
                    ItemPrice = (double)line.UnitPrice,
                    TotalPrice = (double)line.Subtotal,
                    ItemName = string.Empty,
                    QuantyPerBox = line.Item.itemDetails?.QuantityPerBox ?? 0,
                    Discount = (double)Math.Round(line.DiscountPercent, 2),
                    AgentId = SessionManager.AgentDetails.Id
                }).ToList()
            };

            var (_, invoiceNumber) = await _salesService.CreateInvoiceAsync(invoiceDto);
            OperationMessage = $"Invoice {invoiceNumber} created successfully.";
            ClearCart();
            await _loadService.LoadInitialDataAsync();
            LoadFromContext();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to create invoice: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LogoutAsync()
    {
        SessionManager.ClearSession();
        LoginHelper.isLoggedIn = false;
        _loadService.Context.Clear();
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async Task LoadItemsAsync()
    {
        await _loadService.LoadItemPageAsync(_currentItemPage);
        LoadItemsFromContext();
    }

    private void LoadFromContext()
    {
        _currentItemPage = _loadService.Context.CurrentItemPage;
        LoadItemsFromContext();
        LoadAccountsFromContext();
        LoadInvoicesFromContext();
    }

    private void LoadItemsFromContext()
    {
        _currentPageItems.Clear();
        _currentPageItems.AddRange(_loadService.Context.CurrentPageItems);
        ApplyItemFilter();
        ItemsPageText = $"Page {_currentItemPage}";
        ((Command)PreviousItemsCommand).ChangeCanExecute();
    }

    private void LoadAccountsFromContext()
    {
        _allAccounts.Clear();
        _allAccounts.AddRange(_loadService.Context.Accounts.OrderBy(account => account.Name));
        ApplyAccountFilter();
        ApplyMaintenanceAccountFilter();
    }

    private void LoadInvoicesFromContext()
    {
        _allInvoices.Clear();
        _allInvoices.AddRange(_loadService.Context.Invoices);
        ApplyInvoiceFilter();
    }

    private async Task LoadMaintenanceAsync()
    {
        IsBusy = true;

        try
        {
            _allAgents.Clear();
            _allAgents.AddRange(await _agentsService.GetAllAgentsAsync());
            Replace(MaintenanceAgents, _allAgents.OrderBy(agent => agent.AgentName));

            _maintenanceItems.Clear();
            _maintenanceItems.AddRange(await _itemServices.GetAllItemsAsync());
            ApplyMaintenanceItemFilter();
            ApplyMaintenanceAccountFilter();
            _maintenanceLoaded = true;
        }
        catch (Exception ex)
        {
            OperationMessage = $"Unable to load maintenance data: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void RefreshCartPrices()
    {
        foreach (var cartLine in CartItems)
        {
            cartLine.UnitPrice = (decimal)GetPriceBasedOnSelection(cartLine.Item);
        }

        CalculateTotals();
    }

    private void CalculateTotals()
    {
        var subtotal = CartItems.Sum(line => line.Subtotal);
        var tax = ParseCurrency(TaxText);
        var discount = ParseCurrency(InvoiceDiscountText);
        var paidAmount = ParseCurrency(PaidAmountText);
        var total = subtotal + tax - discount;
        var change = paidAmount > total ? paidAmount - total : 0;
        var remaining = total > paidAmount ? total - paidAmount : 0;

        SubtotalText = FormatMoney(subtotal);
        TotalText = FormatMoney(total);
        ChangeText = FormatMoney(change);
        RemainingText = FormatMoney(remaining);
    }

    private double GetPriceBasedOnSelection(Items item)
    {
        if (item.itemDetails is null)
        {
            return 0;
        }

        var isRetail = PricingMode == "Retail";
        return SelectedLocation switch
        {
            "Bataan" => isRetail ? item.itemDetails.BataanRetail : item.itemDetails.BataanWholeSale,
            "Pampanga" => isRetail ? item.itemDetails.PampangaRetail : item.itemDetails.PampangaWholeSale,
            "Zambales" => isRetail ? item.itemDetails.ZambalesRetail : item.itemDetails.ZambalesWholeSale,
            _ => 0
        };
    }

    private void UpdateAgentHeader()
    {
        var agent = SessionManager.AgentDetails;
        AgentName = agent?.AgentName ?? "Autotech";

        var role = agent?.AgentRole ?? "User";
        var location = agent?.Location?.LocationName ?? "No location";
        AgentMeta = $"{role} - {location}";

        SelectedLocation = location switch
        {
            "Bataan" => "Bataan",
            "Zambales" => "Zambales",
            "Upper Pampanga" or "Lower Pampanga" => "Pampanga",
            _ => SelectedLocation
        };

        OnPropertyChanged(nameof(CanAccessMaintenance));
    }

    private static decimal ParseCurrency(string? text)
    {
        if (decimal.TryParse(text, NumberStyles.Currency, CultureInfo.CurrentCulture, out var value))
        {
            return value;
        }

        return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value) ? value : 0;
    }

    private static string FormatMoney(decimal value)
    {
        return value.ToString("N2", CultureInfo.CurrentCulture);
    }

    private static bool Contains(string? value, string keyword)
    {
        return value?.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static void Replace<T>(ObservableCollection<T> target, IEnumerable<T> values)
    {
        target.Clear();

        foreach (var value in values)
        {
            target.Add(value);
        }
    }

    private static string GetInvoiceFilterDescription(InvoiceFilterOption option)
    {
        var field = typeof(InvoiceFilterOption).GetField(option.ToString());
        return field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .OfType<DescriptionAttribute>()
            .FirstOrDefault()?.Description ?? option.ToString();
    }

    public sealed class InvoiceFilterChoice(InvoiceFilterOption key, string description)
    {
        public InvoiceFilterOption Key { get; } = key;

        public string Description { get; } = description;
    }

    public sealed class PaymentOption(PaymentMethod method, string description)
    {
        public PaymentMethod Method { get; } = method;

        public string Description { get; } = description;
    }

    public sealed class InvoiceRow
    {
        private readonly SalesDTO _invoice;

        public InvoiceRow(SalesDTO invoice)
        {
            _invoice = invoice;
        }

        public string InvoiceNumber => _invoice.strInvoiceNumber;

        public string DateSoldText => _invoice.DateSold.ToString("dd/MM/yyyy h:mm tt", CultureInfo.CurrentCulture);

        public string AgentName => _invoice.Agent ?? string.Empty;

        public string CustomerName => _invoice.AccountName ?? string.Empty;

        public string PaymentMethod => _invoice.PaymentType ?? string.Empty;

        public string TotalSalesText => _invoice.TotalSales.ToString("N2", CultureInfo.CurrentCulture);

        public string TaxAmountText => _invoice.Tax.ToString("N2", CultureInfo.CurrentCulture);

        public string DiscountText => _invoice.DiscountPeso.ToString("N2", CultureInfo.CurrentCulture);

        public string TermsText => _invoice.Terms.ToString("N0", CultureInfo.CurrentCulture);

        public string DueDateText => _invoice.DueDate.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);

        public string RemainingBalanceText => _invoice.RemainingBalance.ToString("N2", CultureInfo.CurrentCulture);

        public string Status => _invoice.Status ?? string.Empty;

        public string Cluster => _invoice.Cluster ?? string.Empty;

        public string RowBackground => Status.ToLowerInvariant() switch
        {
            "fully paid" => "#2ecc71",
            "incomplete" => "#e74c3c",
            "for approval" => "#f1c40f",
            "denied" => "#95a5a6",
            _ => "#2c3e50"
        };

        public string RowTextColor => Status.ToLowerInvariant() switch
        {
            "incomplete" => "#FFFFFF",
            "denied" => "#FFFFFF",
            _ => "#52615E"
        };
    }

    public sealed class CartLine : ViewModelBase
    {
        private readonly Action _changed;
        private bool _isSelected;
        private decimal _unitPrice;
        private decimal _quantity = 1;
        private decimal _discountPercent;
        private string _quantityText = "1";
        private string _discountText = "0";

        public CartLine(Items item, double unitPrice, Action changed)
        {
            Item = item;
            _unitPrice = (decimal)unitPrice;
            _changed = changed;
        }

        public Items Item { get; }

        public string ItemCode => Item.ItemCode;

        public string ItemName => Item.ItemName;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (SetProperty(ref _unitPrice, value))
                {
                    OnPropertyChanged(nameof(UnitPriceText));
                    OnPropertyChanged(nameof(Subtotal));
                    OnPropertyChanged(nameof(SubtotalText));
                    _changed();
                }
            }
        }

        public string UnitPriceText => UnitPrice.ToString("N2", CultureInfo.CurrentCulture);

        public decimal Quantity => _quantity;

        public string QuantityText
        {
            get => _quantityText;
            set
            {
                if (SetProperty(ref _quantityText, value))
                {
                    _quantity = ParseCurrency(value);
                    if (_quantity <= 0)
                    {
                        _quantity = 1;
                    }

                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(Subtotal));
                    OnPropertyChanged(nameof(SubtotalText));
                    _changed();
                }
            }
        }

        public decimal DiscountPercent => _discountPercent;

        public string DiscountText
        {
            get => _discountText;
            set
            {
                if (SetProperty(ref _discountText, value))
                {
                    _discountPercent = ParseCurrency(value);
                    OnPropertyChanged(nameof(DiscountPercent));
                    OnPropertyChanged(nameof(Subtotal));
                    OnPropertyChanged(nameof(SubtotalText));
                    _changed();
                }
            }
        }

        public decimal Subtotal
        {
            get
            {
                var discountAmount = UnitPrice * (DiscountPercent / 100);
                return (UnitPrice - discountAmount) * Quantity;
            }
        }

        public string SubtotalText => Subtotal.ToString("N2", CultureInfo.CurrentCulture);
    }
}
