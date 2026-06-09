using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Enums;
using Autotech.Desktop.Core.Models;
using Autotech.Desktop.Helper;
using ClosedXML.Excel;

namespace Autotech.Desktop.MAUI.Viewmodels;

public sealed class DashboardPageViewModel : ViewModelBase
{
    private readonly AppLoadService _loadService = AppLoadService.Current;
    private readonly SalesService _salesService = new();
    private readonly AgentsService _agentsService = new();
    private readonly AccountService _accountService = new();
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
    private string? _maintenanceAgentSearchText;
    private string? _maintenanceItemSearchText;
    private string _selectedMaintenanceTab = "Agents";
    private Accounts? _selectedMaintenanceAccount;
    private AgentDTO? _selectedMaintenanceAgent;
    private Items? _selectedMaintenanceItem;
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
    private bool _isMaintenanceEditorOpen;
    private bool _isMaintenanceEditorForNewRecord;
    private string _maintenanceEditorTitle = string.Empty;
    private string _editAgentUsername = string.Empty;
    private string _editAgentPassword = string.Empty;
    private string _editAgentName = string.Empty;
    private string _editAgentContact = string.Empty;
    private string _editAgentAddress = string.Empty;
    private string _editAgentRole = string.Empty;
    private string _editAccountName = string.Empty;
    private string _editAccountContactPerson = string.Empty;
    private string _editAccountEmail = string.Empty;
    private string _editAccountContactNumber = string.Empty;
    private string _editAccountAddress = string.Empty;
    private string _editAccountTerms = "0";
    private string _editItemCode = string.Empty;
    private string _editItemName = string.Empty;
    private string _editItemDescription = string.Empty;
    private string _editItemOnHand = "0";
    private string _editItemQuantityPerBox = "0";
    private string _editItemBataanRetail = "0";
    private string _editItemBataanWholesale = "0";
    private string _editItemPampangaRetail = "0";
    private string _editItemPampangaWholesale = "0";
    private string _editItemZambalesRetail = "0";
    private string _editItemZambalesWholesale = "0";

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
        SelectMaintenanceTabCommand = new Command<string>(async tab => await SelectMaintenanceTabAsync(tab));
        MaintenanceRefreshCommand = new Command(async () => await RefreshMaintenanceAsync(), () => !IsBusy);
        MaintenanceAddCommand = new Command(async () => await AddMaintenanceRecordAsync(), () => !IsBusy);
        MaintenanceEditCommand = new Command(async () => await EditMaintenanceRecordAsync(), () => !IsBusy);
        MaintenanceImportExcelCommand = new Command(async () => await ImportMaintenanceItemsAsync(), () => !IsBusy && IsMaintenanceItemsTab);
        MaintenanceSaveEditorCommand = new Command(async () => await SaveMaintenanceEditorAsync(), () => !IsBusy);
        MaintenanceCancelEditorCommand = new Command(CloseMaintenanceEditor);
        ProfitPerMonthReportCommand = new Command(OpenProfitPerMonthReport);
        ItemSalesReportCommand = new Command(OpenItemSalesReport);

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

    public ICommand SelectMaintenanceTabCommand { get; }

    public ICommand MaintenanceRefreshCommand { get; }

    public ICommand MaintenanceAddCommand { get; }

    public ICommand MaintenanceEditCommand { get; }

    public ICommand MaintenanceImportExcelCommand { get; }

    public ICommand MaintenanceSaveEditorCommand { get; }

    public ICommand MaintenanceCancelEditorCommand { get; }

    public ICommand ProfitPerMonthReportCommand { get; }

    public ICommand ItemSalesReportCommand { get; }

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

    public string? MaintenanceAgentSearchText
    {
        get => _maintenanceAgentSearchText;
        set
        {
            if (SetProperty(ref _maintenanceAgentSearchText, value))
            {
                ApplyMaintenanceAgentFilter();
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

    public string SelectedMaintenanceTab
    {
        get => _selectedMaintenanceTab;
        private set
        {
            if (SetProperty(ref _selectedMaintenanceTab, value))
            {
                OnPropertyChanged(nameof(IsMaintenanceAccountsTab));
                OnPropertyChanged(nameof(IsMaintenanceAgentsTab));
                OnPropertyChanged(nameof(IsMaintenanceItemsTab));
                OnPropertyChanged(nameof(IsMaintenanceReportsTab));
                OnPropertyChanged(nameof(IsMaintenanceAgentEditor));
                OnPropertyChanged(nameof(IsMaintenanceAccountEditor));
                OnPropertyChanged(nameof(IsMaintenanceItemEditor));
                ((Command)MaintenanceImportExcelCommand).ChangeCanExecute();
            }
        }
    }

    public bool IsMaintenanceAccountsTab => SelectedMaintenanceTab == "Accounts";

    public bool IsMaintenanceAgentsTab => SelectedMaintenanceTab == "Agents";

    public bool IsMaintenanceItemsTab => SelectedMaintenanceTab == "Items";

    public bool IsMaintenanceReportsTab => SelectedMaintenanceTab == "Reports";

    public Accounts? SelectedMaintenanceAccount
    {
        get => _selectedMaintenanceAccount;
        set => SetProperty(ref _selectedMaintenanceAccount, value);
    }

    public AgentDTO? SelectedMaintenanceAgent
    {
        get => _selectedMaintenanceAgent;
        set => SetProperty(ref _selectedMaintenanceAgent, value);
    }

    public Items? SelectedMaintenanceItem
    {
        get => _selectedMaintenanceItem;
        set => SetProperty(ref _selectedMaintenanceItem, value);
    }

    public bool IsMaintenanceEditorOpen
    {
        get => _isMaintenanceEditorOpen;
        private set
        {
            if (SetProperty(ref _isMaintenanceEditorOpen, value))
            {
                OnPropertyChanged(nameof(IsMaintenanceAgentEditor));
                OnPropertyChanged(nameof(IsMaintenanceAccountEditor));
                OnPropertyChanged(nameof(IsMaintenanceItemEditor));
            }
        }
    }

    public string MaintenanceEditorTitle
    {
        get => _maintenanceEditorTitle;
        private set => SetProperty(ref _maintenanceEditorTitle, value);
    }

    public bool IsMaintenanceAgentEditor => IsMaintenanceEditorOpen && IsMaintenanceAgentsTab;

    public bool IsMaintenanceAccountEditor => IsMaintenanceEditorOpen && IsMaintenanceAccountsTab;

    public bool IsMaintenanceItemEditor => IsMaintenanceEditorOpen && IsMaintenanceItemsTab;

    public string EditAgentUsername { get => _editAgentUsername; set => SetProperty(ref _editAgentUsername, value); }

    public string EditAgentPassword { get => _editAgentPassword; set => SetProperty(ref _editAgentPassword, value); }

    public string EditAgentName { get => _editAgentName; set => SetProperty(ref _editAgentName, value); }

    public string EditAgentContact { get => _editAgentContact; set => SetProperty(ref _editAgentContact, value); }

    public string EditAgentAddress { get => _editAgentAddress; set => SetProperty(ref _editAgentAddress, value); }

    public string EditAgentRole { get => _editAgentRole; set => SetProperty(ref _editAgentRole, value); }

    public string EditAccountName { get => _editAccountName; set => SetProperty(ref _editAccountName, value); }

    public string EditAccountContactPerson { get => _editAccountContactPerson; set => SetProperty(ref _editAccountContactPerson, value); }

    public string EditAccountEmail { get => _editAccountEmail; set => SetProperty(ref _editAccountEmail, value); }

    public string EditAccountContactNumber { get => _editAccountContactNumber; set => SetProperty(ref _editAccountContactNumber, value); }

    public string EditAccountAddress { get => _editAccountAddress; set => SetProperty(ref _editAccountAddress, value); }

    public string EditAccountTerms { get => _editAccountTerms; set => SetProperty(ref _editAccountTerms, value); }

    public string EditItemCode { get => _editItemCode; set => SetProperty(ref _editItemCode, value); }

    public string EditItemName { get => _editItemName; set => SetProperty(ref _editItemName, value); }

    public string EditItemDescription { get => _editItemDescription; set => SetProperty(ref _editItemDescription, value); }

    public string EditItemOnHand { get => _editItemOnHand; set => SetProperty(ref _editItemOnHand, value); }

    public string EditItemQuantityPerBox { get => _editItemQuantityPerBox; set => SetProperty(ref _editItemQuantityPerBox, value); }

    public string EditItemBataanRetail { get => _editItemBataanRetail; set => SetProperty(ref _editItemBataanRetail, value); }

    public string EditItemBataanWholesale { get => _editItemBataanWholesale; set => SetProperty(ref _editItemBataanWholesale, value); }

    public string EditItemPampangaRetail { get => _editItemPampangaRetail; set => SetProperty(ref _editItemPampangaRetail, value); }

    public string EditItemPampangaWholesale { get => _editItemPampangaWholesale; set => SetProperty(ref _editItemPampangaWholesale, value); }

    public string EditItemZambalesRetail { get => _editItemZambalesRetail; set => SetProperty(ref _editItemZambalesRetail, value); }

    public string EditItemZambalesWholesale { get => _editItemZambalesWholesale; set => SetProperty(ref _editItemZambalesWholesale, value); }

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

    public void ApplyMaintenanceAgentFilter()
    {
        var keyword = MaintenanceAgentSearchText?.Trim() ?? string.Empty;
        var filtered = string.IsNullOrWhiteSpace(keyword)
            ? _allAgents
            : _allAgents.Where(agent =>
                Contains(agent.AgentName, keyword) ||
                Contains(agent.Username, keyword) ||
                Contains(agent.AgentRole, keyword) ||
                Contains(agent.AgentContactNumber, keyword));

        Replace(MaintenanceAgents, filtered.OrderBy(agent => agent.AgentName));
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

    private async Task SelectMaintenanceTabAsync(string? tab)
    {
        if (string.IsNullOrWhiteSpace(tab))
        {
            return;
        }

        SelectedMaintenanceTab = tab;
        if (!_maintenanceLoaded)
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
            ApplyMaintenanceAgentFilter();

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

    private async Task RefreshMaintenanceAsync()
    {
        _maintenanceLoaded = false;
        await LoadMaintenanceAsync();
        OperationMessage = "Maintenance data refreshed.";
    }

    private async Task AddMaintenanceRecordAsync()
    {
        if (IsMaintenanceAccountsTab)
        {
            OpenAccountEditor(null);
            return;
        }

        if (IsMaintenanceAgentsTab)
        {
            OpenAgentEditor(null);
            return;
        }

        if (IsMaintenanceItemsTab)
        {
            OpenItemEditor(null);
            return;
        }

        await Task.CompletedTask;
    }

    private async Task EditMaintenanceRecordAsync()
    {
        if (IsMaintenanceAccountsTab)
        {
            if (SelectedMaintenanceAccount is null)
            {
                OperationMessage = "Select an account to edit.";
                return;
            }

            OpenAccountEditor(SelectedMaintenanceAccount);
            return;
        }

        if (IsMaintenanceAgentsTab)
        {
            if (SelectedMaintenanceAgent is null)
            {
                OperationMessage = "Select an agent to edit.";
                return;
            }

            OpenAgentEditor(SelectedMaintenanceAgent);
            return;
        }

        if (IsMaintenanceItemsTab)
        {
            if (SelectedMaintenanceItem is null)
            {
                OperationMessage = "Select an item to edit.";
                return;
            }

            OpenItemEditor(SelectedMaintenanceItem);
            return;
        }

        await Task.CompletedTask;
    }

    private async Task ImportMaintenanceItemsAsync()
    {
        if (!IsMaintenanceItemsTab)
        {
            OperationMessage = "Import via Excel is available on the Items tab.";
            return;
        }

        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select item import Excel file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, [".xlsx"] },
                    { DevicePlatform.MacCatalyst, ["org.openxmlformats.spreadsheetml.sheet"] },
                    { DevicePlatform.iOS, ["org.openxmlformats.spreadsheetml.sheet"] },
                    { DevicePlatform.Android, ["application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"] }
                })
            });

            if (result is null)
            {
                return;
            }

            IsBusy = true;
            await using var stream = await result.OpenReadAsync();
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();
            var items = new List<ItemRequestDto>();

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var itemCode = row.Cell(1).GetString().Trim();
                var itemName = row.Cell(2).GetString().Trim();
                if (string.IsNullOrWhiteSpace(itemCode) && string.IsNullOrWhiteSpace(itemName))
                {
                    continue;
                }

                items.Add(new ItemRequestDto
                {
                    Id = Guid.NewGuid(),
                    ItemCode = itemCode,
                    ItemName = itemName,
                    ItemDescription = row.Cell(3).GetString().Trim(),
                    OnHand = GetCellDouble(row.Cell(4)),
                    QuantityPerBox = GetCellDouble(row.Cell(5)),
                    BataanRetail = GetCellDouble(row.Cell(6)),
                    BataanWholeSale = GetCellDouble(row.Cell(7)),
                    PampangaRetail = GetCellDouble(row.Cell(8)),
                    PampangaWholeSale = GetCellDouble(row.Cell(9)),
                    ZambalesRetail = GetCellDouble(row.Cell(10)),
                    ZambalesWholeSale = GetCellDouble(row.Cell(11)),
                    ItemsSold = 0,
                    Sales = 0,
                    Quantity = 0
                });
            }

            if (items.Count == 0)
            {
                OperationMessage = "No valid items found in the selected Excel file.";
                return;
            }

            var success = await _itemServices.CreateBulkItemsAsync(items);
            OperationMessage = success ? $"{items.Count:N0} item(s) imported successfully." : "Import failed. Check the Excel data.";
            await RefreshMaintenanceAsync();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Import failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OpenAgentEditor(AgentDTO? agent)
    {
        _isMaintenanceEditorForNewRecord = agent is null;
        MaintenanceEditorTitle = _isMaintenanceEditorForNewRecord ? "Add Agent" : "Edit Agent";
        EditAgentUsername = agent?.Username ?? string.Empty;
        EditAgentPassword = string.Empty;
        EditAgentName = agent?.AgentName ?? string.Empty;
        EditAgentContact = agent?.AgentContactNumber ?? string.Empty;
        EditAgentAddress = agent?.AgentAddress ?? string.Empty;
        EditAgentRole = agent?.AgentRole ?? "Admin";
        IsMaintenanceEditorOpen = true;
    }

    private void OpenAccountEditor(Accounts? account)
    {
        _isMaintenanceEditorForNewRecord = account is null;
        MaintenanceEditorTitle = _isMaintenanceEditorForNewRecord ? "Add Account" : "Edit Account";
        EditAccountName = account?.Name ?? string.Empty;
        EditAccountContactPerson = account?.ContactPerson ?? string.Empty;
        EditAccountEmail = account?.Email ?? string.Empty;
        EditAccountContactNumber = account?.ContactNumber ?? string.Empty;
        EditAccountAddress = account?.Address ?? string.Empty;
        EditAccountTerms = (account?.Terms ?? 0).ToString(CultureInfo.InvariantCulture);
        IsMaintenanceEditorOpen = true;
    }

    private void OpenItemEditor(Items? item)
    {
        _isMaintenanceEditorForNewRecord = item is null;
        MaintenanceEditorTitle = _isMaintenanceEditorForNewRecord ? "Add Item" : "Edit Item";
        var details = item?.itemDetails;
        EditItemCode = item?.ItemCode ?? string.Empty;
        EditItemName = item?.ItemName ?? string.Empty;
        EditItemDescription = item?.ItemDescription ?? string.Empty;
        EditItemOnHand = FormatEditorNumber(details?.OnHand ?? 0);
        EditItemQuantityPerBox = FormatEditorNumber(details?.QuantityPerBox ?? 0);
        EditItemBataanRetail = FormatEditorNumber(details?.BataanRetail ?? 0);
        EditItemBataanWholesale = FormatEditorNumber(details?.BataanWholeSale ?? 0);
        EditItemPampangaRetail = FormatEditorNumber(details?.PampangaRetail ?? 0);
        EditItemPampangaWholesale = FormatEditorNumber(details?.PampangaWholeSale ?? 0);
        EditItemZambalesRetail = FormatEditorNumber(details?.ZambalesRetail ?? 0);
        EditItemZambalesWholesale = FormatEditorNumber(details?.ZambalesWholeSale ?? 0);
        IsMaintenanceEditorOpen = true;
    }

    private void CloseMaintenanceEditor()
    {
        IsMaintenanceEditorOpen = false;
    }

    private async Task SaveMaintenanceEditorAsync()
    {
        if (IsMaintenanceAgentsTab)
        {
            await SaveAgentEditorAsync();
            return;
        }

        if (IsMaintenanceAccountsTab)
        {
            await SaveAccountEditorAsync();
            return;
        }

        if (IsMaintenanceItemsTab)
        {
            await SaveItemEditorAsync();
        }
    }

    private async Task SaveAgentEditorAsync()
    {
        if (string.IsNullOrWhiteSpace(EditAgentUsername) ||
            string.IsNullOrWhiteSpace(EditAgentName) ||
            string.IsNullOrWhiteSpace(EditAgentRole))
        {
            OperationMessage = "Username, agent name, and role are required.";
            return;
        }

        if (_isMaintenanceEditorForNewRecord && string.IsNullOrWhiteSpace(EditAgentPassword))
        {
            OperationMessage = "Password is required for a new agent.";
            return;
        }

        try
        {
            IsBusy = true;
            var source = _isMaintenanceEditorForNewRecord ? null : SelectedMaintenanceAgent;
            var locationId = source?.LocationId
                ?? SessionManager.AgentDetails?.LocationId
                ?? _loadService.Context.Accounts.FirstOrDefault()?.LocationId
                ?? Guid.Empty;

            var request = new AgentRequestDTO
            {
                Id = source?.Id ?? Guid.NewGuid(),
                Username = EditAgentUsername.Trim(),
                Password = string.IsNullOrWhiteSpace(EditAgentPassword)
                    ? source?.Password ?? string.Empty
                    : PasswordHelper.HashPassword(EditAgentPassword),
                AgentName = EditAgentName.Trim(),
                AgentContactNumber = EditAgentContact.Trim(),
                AgentAddress = EditAgentAddress.Trim(),
                AgentRole = EditAgentRole.Trim(),
                DateCreated = source?.DateCreated ?? DateTime.Now,
                DateLastLogin = source?.DateLastLogin,
                LocationId = locationId
            };

            if (_isMaintenanceEditorForNewRecord)
            {
                await _agentsService.AddAgentAsync(request);
                OperationMessage = "Agent added successfully.";
            }
            else
            {
                await _agentsService.UpdateAgentAsync(request);
                OperationMessage = "Agent updated successfully.";
            }

            CloseMaintenanceEditor();
            await RefreshMaintenanceAsync();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to save agent: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveAccountEditorAsync()
    {
        if (string.IsNullOrWhiteSpace(EditAccountName))
        {
            OperationMessage = "Account name is required.";
            return;
        }

        if (!int.TryParse(EditAccountTerms, NumberStyles.Integer, CultureInfo.InvariantCulture, out var terms))
        {
            OperationMessage = "Terms must be a whole number.";
            return;
        }

        try
        {
            IsBusy = true;
            var source = _isMaintenanceEditorForNewRecord ? null : SelectedMaintenanceAccount;
            var agent = SessionManager.AgentDetails;
            var account = source ?? new Accounts
            {
                Id = Guid.NewGuid(),
                DiscountPercent = 0,
                Cluster = agent?.Location?.LocationName ?? string.Empty,
                isActive = true,
                RegisterDate = DateTime.Now,
                LocationId = agent?.LocationId ?? _loadService.Context.Accounts.FirstOrDefault()?.LocationId ?? Guid.Empty,
                Location = agent?.Location!
            };

            account.Name = EditAccountName.Trim();
            account.ContactPerson = EditAccountContactPerson.Trim();
            account.Email = EditAccountEmail.Trim();
            account.ContactNumber = EditAccountContactNumber.Trim();
            account.Address = EditAccountAddress.Trim();
            account.Terms = terms;

            if (_isMaintenanceEditorForNewRecord)
            {
                await _accountService.AddAccountAsync(account);
                await _loadService.LoadInitialDataAsync();
                LoadFromContext();
                OperationMessage = "Account added successfully.";
            }
            else
            {
                await _accountService.UpdateAccountAsync(account);
                OperationMessage = "Account updated successfully.";
            }

            CloseMaintenanceEditor();
            await RefreshMaintenanceAsync();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to save account: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveItemEditorAsync()
    {
        if (string.IsNullOrWhiteSpace(EditItemCode) || string.IsNullOrWhiteSpace(EditItemName))
        {
            OperationMessage = "Code and item name are required.";
            return;
        }

        if (!TryReadItemEditorNumbers(out var onHand, out var quantityPerBox, out var bataanRetail, out var bataanWholesale,
                out var pampangaRetail, out var pampangaWholesale, out var zambalesRetail, out var zambalesWholesale))
        {
            return;
        }

        try
        {
            IsBusy = true;
            if (_isMaintenanceEditorForNewRecord)
            {
                var success = await _itemServices.CreateBulkItemsAsync(
                [
                    new ItemRequestDto
                    {
                        Id = Guid.NewGuid(),
                        ItemCode = EditItemCode.Trim(),
                        ItemName = EditItemName.Trim(),
                        ItemDescription = EditItemDescription.Trim(),
                        OnHand = onHand,
                        QuantityPerBox = quantityPerBox,
                        BataanRetail = bataanRetail,
                        BataanWholeSale = bataanWholesale,
                        PampangaRetail = pampangaRetail,
                        PampangaWholeSale = pampangaWholesale,
                        ZambalesRetail = zambalesRetail,
                        ZambalesWholeSale = zambalesWholesale,
                        ItemsSold = 0,
                        Sales = 0,
                        Quantity = 0
                    }
                ]);

                OperationMessage = success ? "Item added successfully." : "Failed to add item.";
            }
            else if (SelectedMaintenanceItem is not null)
            {
                var item = SelectedMaintenanceItem;
                item.itemDetails ??= new ItemDetails { ItemId = item.Id };
                item.ItemCode = EditItemCode.Trim();
                item.ItemName = EditItemName.Trim();
                item.ItemDescription = EditItemDescription.Trim();
                item.itemDetails.OnHand = onHand;
                item.itemDetails.QuantityPerBox = quantityPerBox;
                item.itemDetails.BataanRetail = bataanRetail;
                item.itemDetails.BataanWholeSale = bataanWholesale;
                item.itemDetails.PampangaRetail = pampangaRetail;
                item.itemDetails.PampangaWholeSale = pampangaWholesale;
                item.itemDetails.ZambalesRetail = zambalesRetail;
                item.itemDetails.ZambalesWholeSale = zambalesWholesale;

                var success = await _itemServices.UpdateItemAsync(item);
                OperationMessage = success ? "Item updated successfully." : "Failed to update item.";
            }

            CloseMaintenanceEditor();
            await RefreshMaintenanceAsync();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to save item: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool TryReadItemEditorNumbers(
        out double onHand,
        out double quantityPerBox,
        out double bataanRetail,
        out double bataanWholesale,
        out double pampangaRetail,
        out double pampangaWholesale,
        out double zambalesRetail,
        out double zambalesWholesale)
    {
        onHand = quantityPerBox = bataanRetail = bataanWholesale = pampangaRetail = pampangaWholesale = zambalesRetail = zambalesWholesale = 0;
        return TryParseEditorNumber(EditItemOnHand, "On hand", out onHand)
            && TryParseEditorNumber(EditItemQuantityPerBox, "Quantity per box", out quantityPerBox)
            && TryParseEditorNumber(EditItemBataanRetail, "Bataan retail", out bataanRetail)
            && TryParseEditorNumber(EditItemBataanWholesale, "Bataan wholesale", out bataanWholesale)
            && TryParseEditorNumber(EditItemPampangaRetail, "Pampanga retail", out pampangaRetail)
            && TryParseEditorNumber(EditItemPampangaWholesale, "Pampanga wholesale", out pampangaWholesale)
            && TryParseEditorNumber(EditItemZambalesRetail, "Zambales retail", out zambalesRetail)
            && TryParseEditorNumber(EditItemZambalesWholesale, "Zambales wholesale", out zambalesWholesale);
    }

    private bool TryParseEditorNumber(string text, string label, out double value)
    {
        if (double.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
        {
            return true;
        }

        OperationMessage = $"{label} must be a valid number.";
        return false;
    }

    private static string FormatEditorNumber(double value)
    {
        return value.ToString("0.##", CultureInfo.InvariantCulture);
    }

    private void OpenProfitPerMonthReport()
    {
        OperationMessage = "Profit Per Month report still needs a native MAUI report view.";
    }

    private void OpenItemSalesReport()
    {
        OperationMessage = "Item sales report still needs a native MAUI report view.";
    }

    private async Task EditAccountAsync(Accounts account)
    {
        var name = await PromptAsync("Edit Account", "Account name", account.Name);
        if (name is null) return;

        var contactPerson = await PromptAsync("Edit Account", "Contact person", account.ContactPerson);
        if (contactPerson is null) return;

        var email = await PromptAsync("Edit Account", "Email", account.Email);
        if (email is null) return;

        var contactNumber = await PromptAsync("Edit Account", "Contact number", account.ContactNumber);
        if (contactNumber is null) return;

        var address = await PromptAsync("Edit Account", "Address", account.Address);
        if (address is null) return;

        var termsText = await PromptAsync("Edit Account", "Terms", account.Terms.ToString(CultureInfo.InvariantCulture), Keyboard.Numeric);
        if (termsText is null) return;

        if (!int.TryParse(termsText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var terms))
        {
            OperationMessage = "Terms must be a whole number.";
            return;
        }

        try
        {
            IsBusy = true;
            account.Name = name.Trim();
            account.ContactPerson = contactPerson.Trim();
            account.Email = email.Trim();
            account.ContactNumber = contactNumber.Trim();
            account.Address = address.Trim();
            account.Terms = terms;

            await _accountService.UpdateAccountAsync(account);
            await RefreshMaintenanceAsync();
            OperationMessage = "Account updated successfully.";
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to update account: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task AddAccountAsync()
    {
        var name = await PromptAsync("Add Account", "Account name");
        if (name is null) return;

        var contactPerson = await PromptAsync("Add Account", "Contact person");
        if (contactPerson is null) return;

        var email = await PromptAsync("Add Account", "Email");
        if (email is null) return;

        var contactNumber = await PromptAsync("Add Account", "Contact number");
        if (contactNumber is null) return;

        var address = await PromptAsync("Add Account", "Address");
        if (address is null) return;

        var termsText = await PromptAsync("Add Account", "Terms", "0", Keyboard.Numeric);
        if (termsText is null) return;

        if (!int.TryParse(termsText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var terms))
        {
            OperationMessage = "Terms must be a whole number.";
            return;
        }

        try
        {
            IsBusy = true;
            var agent = SessionManager.AgentDetails;
            var account = new Accounts
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                ContactPerson = contactPerson.Trim(),
                Email = email.Trim(),
                ContactNumber = contactNumber.Trim(),
                Address = address.Trim(),
                Terms = terms,
                DiscountPercent = 0,
                Cluster = agent?.Location?.LocationName ?? string.Empty,
                isActive = true,
                RegisterDate = DateTime.Now,
                LocationId = agent?.LocationId ?? _loadService.Context.Accounts.FirstOrDefault()?.LocationId ?? Guid.Empty,
                Location = agent?.Location!
            };

            await _accountService.AddAccountAsync(account);
            await _loadService.LoadInitialDataAsync();
            LoadFromContext();
            await RefreshMaintenanceAsync();
            OperationMessage = "Account added successfully.";
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to add account: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task AddAgentAsync()
    {
        var username = await PromptAsync("Add Agent", "Username");
        if (username is null) return;

        var password = await PromptAsync("Add Agent", "Password");
        if (password is null) return;

        var name = await PromptAsync("Add Agent", "Agent name");
        if (name is null) return;

        var contact = await PromptAsync("Add Agent", "Contact number");
        if (contact is null) return;

        var address = await PromptAsync("Add Agent", "Address");
        if (address is null) return;

        var role = await PromptAsync("Add Agent", "Role", "Admin");
        if (role is null) return;

        try
        {
            IsBusy = true;
            var locationId = SessionManager.AgentDetails?.LocationId ?? _loadService.Context.Accounts.FirstOrDefault()?.LocationId ?? Guid.Empty;
            await _agentsService.AddAgentAsync(new AgentRequestDTO
            {
                Id = Guid.NewGuid(),
                Username = username.Trim(),
                Password = PasswordHelper.HashPassword(password),
                AgentName = name.Trim(),
                AgentContactNumber = contact.Trim(),
                AgentAddress = address.Trim(),
                AgentRole = role.Trim(),
                DateCreated = DateTime.Now,
                DateLastLogin = null,
                LocationId = locationId
            });

            await RefreshMaintenanceAsync();
            OperationMessage = "Agent added successfully.";
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to add agent: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task AddItemAsync()
    {
        var code = await PromptAsync("Add Item", "Code");
        if (code is null) return;

        var name = await PromptAsync("Add Item", "Name");
        if (name is null) return;

        var description = await PromptAsync("Add Item", "Description");
        if (description is null) return;

        var onHand = await PromptNumberAsync("Add Item", "On hand", 0);
        if (onHand is null) return;

        var qtyPerBox = await PromptNumberAsync("Add Item", "Quantity per box", 0);
        if (qtyPerBox is null) return;

        var bataanRetail = await PromptNumberAsync("Add Item", "Bataan retail", 0);
        if (bataanRetail is null) return;

        var bataanWholesale = await PromptNumberAsync("Add Item", "Bataan wholesale", 0);
        if (bataanWholesale is null) return;

        var pampangaRetail = await PromptNumberAsync("Add Item", "Pampanga retail", 0);
        if (pampangaRetail is null) return;

        var pampangaWholesale = await PromptNumberAsync("Add Item", "Pampanga wholesale", 0);
        if (pampangaWholesale is null) return;

        var zambalesRetail = await PromptNumberAsync("Add Item", "Zambales retail", 0);
        if (zambalesRetail is null) return;

        var zambalesWholesale = await PromptNumberAsync("Add Item", "Zambales wholesale", 0);
        if (zambalesWholesale is null) return;

        try
        {
            IsBusy = true;
            var success = await _itemServices.CreateBulkItemsAsync(
            [
                new ItemRequestDto
                {
                    Id = Guid.NewGuid(),
                    ItemCode = code.Trim(),
                    ItemName = name.Trim(),
                    ItemDescription = description.Trim(),
                    OnHand = onHand.Value,
                    QuantityPerBox = qtyPerBox.Value,
                    BataanRetail = bataanRetail.Value,
                    BataanWholeSale = bataanWholesale.Value,
                    PampangaRetail = pampangaRetail.Value,
                    PampangaWholeSale = pampangaWholesale.Value,
                    ZambalesRetail = zambalesRetail.Value,
                    ZambalesWholeSale = zambalesWholesale.Value,
                    ItemsSold = 0,
                    Sales = 0,
                    Quantity = 0
                }
            ]);

            OperationMessage = success ? "Item added successfully." : "Failed to add item.";
            await RefreshMaintenanceAsync();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to add item: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task EditAgentAsync(AgentDTO agent)
    {
        var username = await PromptAsync("Edit Agent", "Username", agent.Username);
        if (username is null) return;

        var password = await PromptAsync("Edit Agent", "Password", string.Empty);
        if (password is null) return;

        var name = await PromptAsync("Edit Agent", "Agent name", agent.AgentName);
        if (name is null) return;

        var contact = await PromptAsync("Edit Agent", "Contact number", agent.AgentContactNumber);
        if (contact is null) return;

        var address = await PromptAsync("Edit Agent", "Address", agent.AgentAddress);
        if (address is null) return;

        var role = await PromptAsync("Edit Agent", "Role", agent.AgentRole);
        if (role is null) return;

        try
        {
            IsBusy = true;
            await _agentsService.UpdateAgentAsync(new AgentRequestDTO
            {
                Id = agent.Id,
                Username = username.Trim(),
                Password = string.IsNullOrWhiteSpace(password) ? agent.Password : PasswordHelper.HashPassword(password),
                AgentName = name.Trim(),
                AgentContactNumber = contact.Trim(),
                AgentAddress = address.Trim(),
                AgentRole = role.Trim(),
                DateCreated = agent.DateCreated,
                DateLastLogin = agent.DateLastLogin,
                LocationId = agent.LocationId
            });

            await RefreshMaintenanceAsync();
            OperationMessage = "Agent updated successfully.";
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to update agent: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task EditItemAsync(Items item)
    {
        item.itemDetails ??= new ItemDetails { ItemId = item.Id };

        var code = await PromptAsync("Edit Item", "Code", item.ItemCode);
        if (code is null) return;

        var name = await PromptAsync("Edit Item", "Name", item.ItemName);
        if (name is null) return;

        var description = await PromptAsync("Edit Item", "Description", item.ItemDescription);
        if (description is null) return;

        var onHand = await PromptNumberAsync("Edit Item", "On hand", item.itemDetails.OnHand);
        if (onHand is null) return;

        var qtyPerBox = await PromptNumberAsync("Edit Item", "Quantity per box", item.itemDetails.QuantityPerBox);
        if (qtyPerBox is null) return;

        var bataanRetail = await PromptNumberAsync("Edit Item", "Bataan retail", item.itemDetails.BataanRetail);
        if (bataanRetail is null) return;

        var bataanWholesale = await PromptNumberAsync("Edit Item", "Bataan wholesale", item.itemDetails.BataanWholeSale);
        if (bataanWholesale is null) return;

        var pampangaRetail = await PromptNumberAsync("Edit Item", "Pampanga retail", item.itemDetails.PampangaRetail);
        if (pampangaRetail is null) return;

        var pampangaWholesale = await PromptNumberAsync("Edit Item", "Pampanga wholesale", item.itemDetails.PampangaWholeSale);
        if (pampangaWholesale is null) return;

        var zambalesRetail = await PromptNumberAsync("Edit Item", "Zambales retail", item.itemDetails.ZambalesRetail);
        if (zambalesRetail is null) return;

        var zambalesWholesale = await PromptNumberAsync("Edit Item", "Zambales wholesale", item.itemDetails.ZambalesWholeSale);
        if (zambalesWholesale is null) return;

        try
        {
            IsBusy = true;
            item.ItemCode = code.Trim();
            item.ItemName = name.Trim();
            item.ItemDescription = description.Trim();
            item.itemDetails.OnHand = onHand.Value;
            item.itemDetails.QuantityPerBox = qtyPerBox.Value;
            item.itemDetails.BataanRetail = bataanRetail.Value;
            item.itemDetails.BataanWholeSale = bataanWholesale.Value;
            item.itemDetails.PampangaRetail = pampangaRetail.Value;
            item.itemDetails.PampangaWholeSale = pampangaWholesale.Value;
            item.itemDetails.ZambalesRetail = zambalesRetail.Value;
            item.itemDetails.ZambalesWholeSale = zambalesWholesale.Value;

            var success = await _itemServices.UpdateItemAsync(item);
            OperationMessage = success ? "Item updated successfully." : "Failed to update item.";
            await RefreshMaintenanceAsync();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to update item: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static Task<string?> PromptAsync(string title, string message, string? initialValue = "", Keyboard? keyboard = null)
    {
        return Shell.Current.DisplayPromptAsync(title, message, initialValue: initialValue ?? string.Empty, keyboard: keyboard ?? Keyboard.Text);
    }

    private static async Task<double?> PromptNumberAsync(string title, string message, double value)
    {
        var text = await PromptAsync(title, message, value.ToString("N2", CultureInfo.CurrentCulture), Keyboard.Numeric);
        if (text is null)
        {
            return null;
        }

        if (double.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out var result) ||
            double.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
        {
            return result;
        }

        await Shell.Current.DisplayAlertAsync("Invalid Number", $"{message} must be a valid number.", "OK");
        return null;
    }

    private static double GetCellDouble(IXLCell cell)
    {
        if (cell.TryGetValue<double>(out var value))
        {
            return value;
        }

        return double.TryParse(cell.GetString(), NumberStyles.Number, CultureInfo.CurrentCulture, out value) ||
               double.TryParse(cell.GetString(), NumberStyles.Number, CultureInfo.InvariantCulture, out value)
            ? value
            : 0;
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

        public Guid Id => _invoice.Id;

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
