using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
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
    private readonly SupplierService _supplierService = new();
    private readonly StockMovementService _stockMovementService = new();
    private readonly ExpenseService _expenseService = new();
    private readonly List<Items> _currentPageItems = new();
    private readonly List<Accounts> _allAccounts = new();
    private readonly List<SalesDTO> _allInvoices = new();
    private readonly List<AgentDTO> _allAgents = new();
    private readonly List<Items> _maintenanceItems = new();
    private readonly List<Supplier> _allSuppliers = new();
    private readonly List<StockMovement> _allStockMovements = new();
    private readonly List<Expense> _allExpenses = new();
    private int _currentItemPage = 1;
    private string _agentName = "StockPilot Pro ERP";
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
    private string? _maintenanceSupplierSearchText;
    private string _selectedMaintenanceTab = "Agents";
    private Accounts? _selectedMaintenanceAccount;
    private AgentDTO? _selectedMaintenanceAgent;
    private Items? _selectedMaintenanceItem;
    private Supplier? _selectedMaintenanceSupplier;
    private StockMovement? _selectedMaintenanceStockMovement;
    private Expense? _selectedMaintenanceExpense;
    private string _pricingMode = "Retail";
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
    private string _editAccountCreditLimit = "0";
    private string _editItemCode = string.Empty;
    private string _editItemName = string.Empty;
    private string _editItemDescription = string.Empty;
    private string _editItemUnitOfMeasure = "pcs";
    private string _editItemCostPrice = "0";
    private string _editItemMinimumStockLevel = "0";
    private string _editItemOnHand = "0";
    private string _editItemQuantityPerBox = "0";
    private string _editItemRetailPrice = "0";
    private string _editItemWholesalePrice = "0";
    private string _editSupplierName = string.Empty;
    private string _editSupplierContactPerson = string.Empty;
    private string _editSupplierContactNumber = string.Empty;
    private string _editSupplierEmail = string.Empty;
    private string _editSupplierAddress = string.Empty;
    private string _editStockMovementItemCode = string.Empty;
    private string _editStockMovementType = "Stock In";
    private string _editStockMovementQuantity = "0";
    private string _editStockMovementUnitCost = "0";
    private string _editStockMovementReference = string.Empty;
    private string _editStockMovementNotes = string.Empty;
    private string _editExpenseCategory = "Miscellaneous";
    private string _editExpenseDescription = string.Empty;
    private string _editExpenseAmount = "0";
    private string _editExpenseReference = string.Empty;
    private string _selectedReportPeriod = "This Month";
    private string _reportDateRangeText = string.Empty;
    private string _reportGeneratedText = string.Empty;
    private string _reportExecutiveSummary = string.Empty;
    private CancellationTokenSource? _operationMessageClearToken;

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
        ExportReportsPdfCommand = new Command(async () => await ExportReportsPdfAsync(), () => !IsBusy);
        ExportReportsExcelCommand = new Command(async () => await ExportReportsExcelAsync(), () => !IsBusy);

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

    public ObservableCollection<Supplier> MaintenanceSuppliers { get; } = new();

    public ObservableCollection<StockMovement> MaintenanceStockMovements { get; } = new();

    public ObservableCollection<Expense> MaintenanceExpenses { get; } = new();

    public ObservableCollection<ReportKpi> ReportKpis { get; } = new();

    public ObservableCollection<ReportBreakdownRow> SalesByStatusReport { get; } = new();

    public ObservableCollection<ReportBreakdownRow> SalesByAgentReport { get; } = new();

    public ObservableCollection<ReportBreakdownRow> SalesByCustomerReport { get; } = new();

    public ObservableCollection<ReportBreakdownRow> SalesByPaymentReport { get; } = new();

    public ObservableCollection<ReportBreakdownRow> MonthlySalesTrendReport { get; } = new();

    public ObservableCollection<ReportBreakdownRow> TopSellingItemsReport { get; } = new();

    public ObservableCollection<InventoryValuationRow> InventoryValuationReport { get; } = new();

    public ObservableCollection<InventoryValuationRow> LowStockReport { get; } = new();

    public List<PaymentOption> PaymentMethods { get; }

    public List<string> PricingModes { get; } = ["Retail", "Wholesale"];

    public List<InvoiceFilterChoice> InvoiceFilterOptions { get; } =
        Enum.GetValues(typeof(InvoiceFilterOption))
            .Cast<InvoiceFilterOption>()
            .Select(option => new InvoiceFilterChoice(option, GetInvoiceFilterDescription(option)))
            .ToList();

    public List<string> InvoiceDateSortOptions { get; } = ["Descending", "Ascending"];

    public List<string> ReportPeriodOptions { get; } = ["Today", "This Week", "This Month", "This Quarter", "This Year", "All Time"];

    public List<string> StockMovementTypes { get; } = ["Stock In", "Stock Out", "Adjustment", "Return", "Defective"];

    public List<string> ExpenseCategories { get; } = ["Salary", "Commission", "Fuel", "Food Allowance", "Truck Maintenance", "Warehouse Maintenance", "Office Supplies", "Miscellaneous"];

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

    public ICommand ExportReportsPdfCommand { get; }

    public ICommand ExportReportsExcelCommand { get; }

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

    public bool CanAccessMaintenance
    {
        get
        {
            var role = SessionManager.AgentDetails?.AgentRole?.Trim();
            return !string.IsNullOrWhiteSpace(role) &&
                   role.Contains("admin", StringComparison.OrdinalIgnoreCase);
        }
    }

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
                OnPropertyChanged(nameof(IsMaintenanceSuppliersTab));
                OnPropertyChanged(nameof(IsMaintenanceStockTab));
                OnPropertyChanged(nameof(IsMaintenanceExpensesTab));
                OnPropertyChanged(nameof(IsMaintenanceReportsTab));
                OnPropertyChanged(nameof(IsMaintenanceAgentEditor));
                OnPropertyChanged(nameof(IsMaintenanceAccountEditor));
                OnPropertyChanged(nameof(IsMaintenanceItemEditor));
                OnPropertyChanged(nameof(IsMaintenanceSupplierEditor));
                OnPropertyChanged(nameof(IsMaintenanceStockEditor));
                OnPropertyChanged(nameof(IsMaintenanceExpenseEditor));
                ((Command)MaintenanceImportExcelCommand).ChangeCanExecute();
            }
        }
    }

    public bool IsMaintenanceAccountsTab => SelectedMaintenanceTab == "Accounts";

    public bool IsMaintenanceAgentsTab => SelectedMaintenanceTab == "Agents";

    public bool IsMaintenanceItemsTab => SelectedMaintenanceTab == "Items";

    public bool IsMaintenanceSuppliersTab => SelectedMaintenanceTab == "Suppliers";

    public bool IsMaintenanceStockTab => SelectedMaintenanceTab == "Stock";

    public bool IsMaintenanceExpensesTab => SelectedMaintenanceTab == "Expenses";

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

    public Supplier? SelectedMaintenanceSupplier
    {
        get => _selectedMaintenanceSupplier;
        set => SetProperty(ref _selectedMaintenanceSupplier, value);
    }

    public StockMovement? SelectedMaintenanceStockMovement
    {
        get => _selectedMaintenanceStockMovement;
        set => SetProperty(ref _selectedMaintenanceStockMovement, value);
    }

    public Expense? SelectedMaintenanceExpense
    {
        get => _selectedMaintenanceExpense;
        set => SetProperty(ref _selectedMaintenanceExpense, value);
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
                OnPropertyChanged(nameof(IsMaintenanceSupplierEditor));
                OnPropertyChanged(nameof(IsMaintenanceStockEditor));
                OnPropertyChanged(nameof(IsMaintenanceExpenseEditor));
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

    public bool IsMaintenanceSupplierEditor => IsMaintenanceEditorOpen && IsMaintenanceSuppliersTab;

    public bool IsMaintenanceStockEditor => IsMaintenanceEditorOpen && IsMaintenanceStockTab;

    public bool IsMaintenanceExpenseEditor => IsMaintenanceEditorOpen && IsMaintenanceExpensesTab;

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

    public string EditAccountCreditLimit { get => _editAccountCreditLimit; set => SetProperty(ref _editAccountCreditLimit, value); }

    public string EditItemCode { get => _editItemCode; set => SetProperty(ref _editItemCode, value); }

    public string EditItemName { get => _editItemName; set => SetProperty(ref _editItemName, value); }

    public string EditItemDescription { get => _editItemDescription; set => SetProperty(ref _editItemDescription, value); }

    public string EditItemUnitOfMeasure { get => _editItemUnitOfMeasure; set => SetProperty(ref _editItemUnitOfMeasure, value); }

    public string EditItemCostPrice { get => _editItemCostPrice; set => SetProperty(ref _editItemCostPrice, value); }

    public string EditItemMinimumStockLevel { get => _editItemMinimumStockLevel; set => SetProperty(ref _editItemMinimumStockLevel, value); }

    public string EditItemOnHand { get => _editItemOnHand; set => SetProperty(ref _editItemOnHand, value); }

    public string EditItemQuantityPerBox { get => _editItemQuantityPerBox; set => SetProperty(ref _editItemQuantityPerBox, value); }

    public string EditItemRetailPrice { get => _editItemRetailPrice; set => SetProperty(ref _editItemRetailPrice, value); }

    public string EditItemWholesalePrice { get => _editItemWholesalePrice; set => SetProperty(ref _editItemWholesalePrice, value); }

    public string? MaintenanceSupplierSearchText
    {
        get => _maintenanceSupplierSearchText;
        set
        {
            if (SetProperty(ref _maintenanceSupplierSearchText, value))
            {
                ApplyMaintenanceSupplierFilter();
            }
        }
    }

    public string EditSupplierName { get => _editSupplierName; set => SetProperty(ref _editSupplierName, value); }

    public string EditSupplierContactPerson { get => _editSupplierContactPerson; set => SetProperty(ref _editSupplierContactPerson, value); }

    public string EditSupplierContactNumber { get => _editSupplierContactNumber; set => SetProperty(ref _editSupplierContactNumber, value); }

    public string EditSupplierEmail { get => _editSupplierEmail; set => SetProperty(ref _editSupplierEmail, value); }

    public string EditSupplierAddress { get => _editSupplierAddress; set => SetProperty(ref _editSupplierAddress, value); }

    public string EditStockMovementItemCode { get => _editStockMovementItemCode; set => SetProperty(ref _editStockMovementItemCode, value); }

    public string EditStockMovementType { get => _editStockMovementType; set => SetProperty(ref _editStockMovementType, value); }

    public string EditStockMovementQuantity { get => _editStockMovementQuantity; set => SetProperty(ref _editStockMovementQuantity, value); }

    public string EditStockMovementUnitCost { get => _editStockMovementUnitCost; set => SetProperty(ref _editStockMovementUnitCost, value); }

    public string EditStockMovementReference { get => _editStockMovementReference; set => SetProperty(ref _editStockMovementReference, value); }

    public string EditStockMovementNotes { get => _editStockMovementNotes; set => SetProperty(ref _editStockMovementNotes, value); }

    public string EditExpenseCategory { get => _editExpenseCategory; set => SetProperty(ref _editExpenseCategory, value); }

    public string EditExpenseDescription { get => _editExpenseDescription; set => SetProperty(ref _editExpenseDescription, value); }

    public string EditExpenseAmount { get => _editExpenseAmount; set => SetProperty(ref _editExpenseAmount, value); }

    public string EditExpenseReference { get => _editExpenseReference; set => SetProperty(ref _editExpenseReference, value); }

    public string SelectedReportPeriod
    {
        get => _selectedReportPeriod;
        set
        {
            if (SetProperty(ref _selectedReportPeriod, value))
            {
                BuildReports();
            }
        }
    }

    public string ReportDateRangeText
    {
        get => _reportDateRangeText;
        private set => SetProperty(ref _reportDateRangeText, value);
    }

    public string ReportGeneratedText
    {
        get => _reportGeneratedText;
        private set => SetProperty(ref _reportGeneratedText, value);
    }

    public string ReportExecutiveSummary
    {
        get => _reportExecutiveSummary;
        private set => SetProperty(ref _reportExecutiveSummary, value);
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
                ScheduleOperationMessageClear(value);
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
        BuildReports();
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

    public void ApplyMaintenanceSupplierFilter()
    {
        var keyword = MaintenanceSupplierSearchText?.Trim() ?? string.Empty;
        var filtered = string.IsNullOrWhiteSpace(keyword)
            ? _allSuppliers
            : _allSuppliers.Where(supplier =>
                Contains(supplier.SupplierName, keyword) ||
                Contains(supplier.ContactPerson, keyword) ||
                Contains(supplier.ContactNumber, keyword) ||
                Contains(supplier.Address, keyword));

        Replace(MaintenanceSuppliers, filtered.OrderBy(supplier => supplier.SupplierName));
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

            _allSuppliers.Clear();
            _allSuppliers.AddRange(await _supplierService.GetAllAsync());
            ApplyMaintenanceSupplierFilter();

            _allStockMovements.Clear();
            _allStockMovements.AddRange(await _stockMovementService.GetAllAsync());
            Replace(MaintenanceStockMovements, _allStockMovements.OrderByDescending(m => m.MovementDate));

            _allExpenses.Clear();
            _allExpenses.AddRange(await _expenseService.GetAllAsync());
            Replace(MaintenanceExpenses, _allExpenses.OrderByDescending(e => e.ExpenseDate));

            BuildReports();
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

        if (IsMaintenanceSuppliersTab)
        {
            OpenSupplierEditor(null);
            return;
        }

        if (IsMaintenanceStockTab)
        {
            OpenStockMovementEditor();
            return;
        }

        if (IsMaintenanceExpensesTab)
        {
            OpenExpenseEditor(null);
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

        if (IsMaintenanceSuppliersTab)
        {
            if (SelectedMaintenanceSupplier is null)
            {
                OperationMessage = "Select a supplier to edit.";
                return;
            }

            OpenSupplierEditor(SelectedMaintenanceSupplier);
            return;
        }

        if (IsMaintenanceExpensesTab)
        {
            if (SelectedMaintenanceExpense is null)
            {
                OperationMessage = "Select an expense to edit.";
                return;
            }

            OpenExpenseEditor(SelectedMaintenanceExpense);
            return;
        }

        if (IsMaintenanceStockTab)
        {
            OperationMessage = "Stock movements are audit records. Add a new correction or adjustment instead.";
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
                    RetailPrice = GetCellDouble(row.Cell(6)),
                    WholesalePrice = GetCellDouble(row.Cell(7)),
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
        EditAccountCreditLimit = FormatEditorNumber(account?.CreditLimit ?? 0);
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
        EditItemUnitOfMeasure = item?.UnitOfMeasure ?? "pcs";
        EditItemCostPrice = FormatEditorNumber(item?.CostPrice ?? 0);
        EditItemMinimumStockLevel = FormatEditorNumber(item?.MinimumStockLevel ?? 0);
        EditItemOnHand = FormatEditorNumber(details?.OnHand ?? 0);
        EditItemQuantityPerBox = FormatEditorNumber(details?.QuantityPerBox ?? 0);
        EditItemRetailPrice = FormatEditorNumber(details?.RetailPrice ?? 0);
        EditItemWholesalePrice = FormatEditorNumber(details?.WholesalePrice ?? 0);
        IsMaintenanceEditorOpen = true;
    }

    private void OpenSupplierEditor(Supplier? supplier)
    {
        _isMaintenanceEditorForNewRecord = supplier is null;
        MaintenanceEditorTitle = _isMaintenanceEditorForNewRecord ? "Add Supplier" : "Edit Supplier";
        EditSupplierName = supplier?.SupplierName ?? string.Empty;
        EditSupplierContactPerson = supplier?.ContactPerson ?? string.Empty;
        EditSupplierContactNumber = supplier?.ContactNumber ?? string.Empty;
        EditSupplierEmail = supplier?.Email ?? string.Empty;
        EditSupplierAddress = supplier?.Address ?? string.Empty;
        IsMaintenanceEditorOpen = true;
    }

    private void OpenStockMovementEditor()
    {
        _isMaintenanceEditorForNewRecord = true;
        MaintenanceEditorTitle = "Add Stock Movement";
        EditStockMovementItemCode = SelectedMaintenanceItem?.ItemCode ?? string.Empty;
        EditStockMovementType = "Stock In";
        EditStockMovementQuantity = "0";
        EditStockMovementUnitCost = "0";
        EditStockMovementReference = string.Empty;
        EditStockMovementNotes = string.Empty;
        IsMaintenanceEditorOpen = true;
    }

    private void OpenExpenseEditor(Expense? expense)
    {
        _isMaintenanceEditorForNewRecord = expense is null;
        MaintenanceEditorTitle = _isMaintenanceEditorForNewRecord ? "Add Expense" : "Edit Expense";
        EditExpenseCategory = expense?.Category ?? "Miscellaneous";
        EditExpenseDescription = expense?.Description ?? string.Empty;
        EditExpenseAmount = FormatEditorNumber(expense?.Amount ?? 0);
        EditExpenseReference = expense?.ReferenceNumber ?? string.Empty;
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
            return;
        }

        if (IsMaintenanceSuppliersTab)
        {
            await SaveSupplierEditorAsync();
            return;
        }

        if (IsMaintenanceStockTab)
        {
            await SaveStockMovementEditorAsync();
            return;
        }

        if (IsMaintenanceExpensesTab)
        {
            await SaveExpenseEditorAsync();
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
                DateLastLogin = source?.DateLastLogin
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

        if (!TryParseEditorNumber(EditAccountCreditLimit, "Credit limit", out var creditLimit))
        {
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
                Cluster = string.Empty,
                isActive = true,
                RegisterDate = DateTime.Now
            };

            account.Name = EditAccountName.Trim();
            account.ContactPerson = EditAccountContactPerson.Trim();
            account.Email = EditAccountEmail.Trim();
            account.ContactNumber = EditAccountContactNumber.Trim();
            account.Address = EditAccountAddress.Trim();
            account.Terms = terms;
            account.CreditLimit = creditLimit;

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

        if (!TryReadItemEditorNumbers(out var onHand, out var quantityPerBox, out var costPrice, out var minimumStockLevel, out var retailPrice, out var wholesalePrice))
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
                        UnitOfMeasure = EditItemUnitOfMeasure.Trim(),
                        CostPrice = costPrice,
                        MinimumStockLevel = minimumStockLevel,
                        OnHand = onHand,
                        QuantityPerBox = quantityPerBox,
                        RetailPrice = retailPrice,
                        WholesalePrice = wholesalePrice,
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
                item.UnitOfMeasure = EditItemUnitOfMeasure.Trim();
                item.CostPrice = costPrice;
                item.MinimumStockLevel = minimumStockLevel;
                item.itemDetails.OnHand = onHand;
                item.itemDetails.QuantityPerBox = quantityPerBox;
                item.itemDetails.RetailPrice = retailPrice;
                item.itemDetails.WholesalePrice = wholesalePrice;

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

    private async Task SaveSupplierEditorAsync()
    {
        if (string.IsNullOrWhiteSpace(EditSupplierName))
        {
            OperationMessage = "Supplier name is required.";
            return;
        }

        try
        {
            IsBusy = true;
            var supplier = _isMaintenanceEditorForNewRecord ? new Supplier { Id = Guid.Empty, DateAdded = DateTime.Now } : SelectedMaintenanceSupplier;
            if (supplier is null)
            {
                OperationMessage = "Select a supplier to edit.";
                return;
            }

            supplier.SupplierName = EditSupplierName.Trim();
            supplier.ContactPerson = EditSupplierContactPerson.Trim();
            supplier.ContactNumber = EditSupplierContactNumber.Trim();
            supplier.Email = EditSupplierEmail.Trim();
            supplier.Address = EditSupplierAddress.Trim();
            supplier.IsActive = true;

            await _supplierService.SaveAsync(supplier);
            OperationMessage = _isMaintenanceEditorForNewRecord ? "Supplier added successfully." : "Supplier updated successfully.";
            CloseMaintenanceEditor();
            await RefreshMaintenanceAsync();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to save supplier: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveStockMovementEditorAsync()
    {
        var item = _maintenanceItems.FirstOrDefault(i => string.Equals(i.ItemCode, EditStockMovementItemCode.Trim(), StringComparison.OrdinalIgnoreCase));
        if (item is null)
        {
            OperationMessage = "Enter a valid item code for the stock movement.";
            return;
        }

        if (!TryParseEditorNumber(EditStockMovementQuantity, "Quantity", out var quantity) ||
            !TryParseEditorNumber(EditStockMovementUnitCost, "Unit cost", out var unitCost))
        {
            return;
        }

        try
        {
            IsBusy = true;
            await _stockMovementService.AddAsync(new StockMovement
            {
                Id = Guid.NewGuid(),
                ItemId = item.Id,
                MovementType = EditStockMovementType,
                Quantity = quantity,
                UnitCost = unitCost,
                ReferenceNumber = EditStockMovementReference.Trim(),
                Notes = EditStockMovementNotes.Trim(),
                MovementDate = DateTime.Now
            });

            OperationMessage = "Stock movement saved and on-hand quantity updated.";
            CloseMaintenanceEditor();
            await RefreshMaintenanceAsync();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to save stock movement: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveExpenseEditorAsync()
    {
        if (!TryParseEditorNumber(EditExpenseAmount, "Expense amount", out var amount))
        {
            return;
        }

        try
        {
            IsBusy = true;
            var expense = _isMaintenanceEditorForNewRecord ? new Expense { Id = Guid.Empty, ExpenseDate = DateTime.Now } : SelectedMaintenanceExpense;
            if (expense is null)
            {
                OperationMessage = "Select an expense to edit.";
                return;
            }

            expense.Category = EditExpenseCategory;
            expense.Description = EditExpenseDescription.Trim();
            expense.Amount = amount;
            expense.ReferenceNumber = EditExpenseReference.Trim();
            expense.ExpenseDate = expense.ExpenseDate == default ? DateTime.Now : expense.ExpenseDate;

            await _expenseService.SaveAsync(expense);
            OperationMessage = _isMaintenanceEditorForNewRecord ? "Expense added successfully." : "Expense updated successfully.";
            CloseMaintenanceEditor();
            await RefreshMaintenanceAsync();
        }
        catch (Exception ex)
        {
            OperationMessage = $"Failed to save expense: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool TryReadItemEditorNumbers(
        out double onHand,
        out double quantityPerBox,
        out double costPrice,
        out double minimumStockLevel,
        out double retailPrice,
        out double wholesalePrice)
    {
        onHand = quantityPerBox = costPrice = minimumStockLevel = retailPrice = wholesalePrice = 0;
        return TryParseEditorNumber(EditItemOnHand, "On hand", out onHand)
            && TryParseEditorNumber(EditItemQuantityPerBox, "Quantity per box", out quantityPerBox)
            && TryParseEditorNumber(EditItemCostPrice, "Cost price", out costPrice)
            && TryParseEditorNumber(EditItemMinimumStockLevel, "Minimum stock level", out minimumStockLevel)
            && TryParseEditorNumber(EditItemRetailPrice, "Retail price", out retailPrice)
            && TryParseEditorNumber(EditItemWholesalePrice, "Wholesale price", out wholesalePrice);
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
        SelectedReportPeriod = "This Month";
        BuildReports();
        OperationMessage = "Profit report refreshed.";
    }

    private void OpenItemSalesReport()
    {
        BuildReports();
        OperationMessage = "Item sales report refreshed.";
    }

    private void BuildReports()
    {
        var now = TimeHelper.GetPhilippineTime();
        var invoices = GetReportInvoices(now).ToList();
        var allItems = _maintenanceItems.Count > 0
            ? _maintenanceItems
            : _loadService.Context.AllItems.Count > 0
                ? _loadService.Context.AllItems
                : _currentPageItems;

        var grossSales = invoices.Sum(invoice => invoice.TotalSales);
        var tax = invoices.Sum(invoice => invoice.Tax);
        var discounts = invoices.Sum(invoice => invoice.DiscountPeso);
        var netSales = grossSales - discounts;
        var remaining = invoices.Sum(invoice => invoice.RemainingBalance);
        var invoiceCount = invoices.Count;
        var averageInvoice = invoiceCount == 0 ? 0 : grossSales / invoiceCount;
        var unitsSold = invoices.SelectMany(invoice => invoice.PurchasedItems ?? []).Sum(item => item.Quantity);
        var inventoryValue = allItems.Sum(GetInventoryValue);
        var lowStockCount = allItems.Count(item => (item.itemDetails?.OnHand ?? 0) <= Math.Max(1, item.itemDetails?.QuantityPerBox ?? 0));

        Replace(ReportKpis,
        [
            new ReportKpi("Gross Sales", FormatReportMoney(grossSales), "Before discounts", "#38DCC8"),
            new ReportKpi("Net Sales", FormatReportMoney(netSales), "After discounts", "#87E0B0"),
            new ReportKpi("Outstanding", FormatReportMoney(remaining), "Uncollected balance", "#F0B35A"),
            new ReportKpi("Invoices", invoiceCount.ToString("N0", CultureInfo.CurrentCulture), $"Avg {FormatReportMoney(averageInvoice)}", "#8DB7FF"),
            new ReportKpi("Units Sold", unitsSold.ToString("N0", CultureInfo.CurrentCulture), "Across invoice lines", "#D8A6FF"),
            new ReportKpi("Inventory Value", FormatReportMoney(inventoryValue), $"{lowStockCount:N0} low-stock SKUs", "#F07F7F")
        ]);

        ReportDateRangeText = GetReportDateRangeText(now);
        ReportGeneratedText = $"Generated {now:dd MMM yyyy h:mm tt}";
        ReportExecutiveSummary = BuildExecutiveSummary(grossSales, remaining, invoiceCount, lowStockCount);

        Replace(SalesByStatusReport, BuildBreakdown(invoices, invoice => invoice.Status, invoice => invoice.TotalSales, "No Status", "#F1C40F"));
        Replace(SalesByAgentReport, BuildBreakdown(invoices, invoice => invoice.Agent, invoice => invoice.TotalSales, "No Agent", "#38DCC8"));
        Replace(SalesByCustomerReport, BuildBreakdown(invoices, invoice => invoice.AccountName, invoice => invoice.TotalSales, "Walk-in", "#87E0B0"));
        Replace(SalesByPaymentReport, BuildBreakdown(invoices, invoice => invoice.PaymentType, invoice => invoice.TotalSales, "No Payment", "#8DB7FF"));
        Replace(MonthlySalesTrendReport, BuildMonthlyTrend(invoices));
        Replace(TopSellingItemsReport, BuildTopSellingItems(invoices));
        Replace(InventoryValuationReport, BuildInventoryValuation(allItems));
        Replace(LowStockReport, BuildLowStock(allItems));
    }

    private IEnumerable<SalesDTO> GetReportInvoices(DateTime now)
    {
        var invoices = _allInvoices.AsEnumerable();
        return SelectedReportPeriod switch
        {
            "Today" => invoices.Where(invoice => invoice.DateSold.Date == now.Date),
            "This Week" => invoices.Where(invoice => invoice.DateSold.Date >= now.Date.AddDays(-(int)now.DayOfWeek)),
            "This Month" => invoices.Where(invoice => invoice.DateSold.Year == now.Year && invoice.DateSold.Month == now.Month),
            "This Quarter" => invoices.Where(invoice => invoice.DateSold.Year == now.Year && ((invoice.DateSold.Month - 1) / 3) == ((now.Month - 1) / 3)),
            "This Year" => invoices.Where(invoice => invoice.DateSold.Year == now.Year),
            _ => invoices
        };
    }

    private string GetReportDateRangeText(DateTime now)
    {
        return SelectedReportPeriod switch
        {
            "Today" => now.ToString("dd MMM yyyy", CultureInfo.CurrentCulture),
            "This Week" => $"{now.Date.AddDays(-(int)now.DayOfWeek):dd MMM yyyy} - {now:dd MMM yyyy}",
            "This Month" => now.ToString("MMMM yyyy", CultureInfo.CurrentCulture),
            "This Quarter" => $"Q{((now.Month - 1) / 3) + 1} {now:yyyy}",
            "This Year" => now.ToString("yyyy", CultureInfo.CurrentCulture),
            _ => "All available invoices"
        };
    }

    private static string BuildExecutiveSummary(double grossSales, double remaining, int invoiceCount, int lowStockCount)
    {
        var collectionRate = grossSales <= 0 ? 100 : Math.Max(0, (grossSales - remaining) / grossSales * 100);
        return $"Sales performance covers {invoiceCount:N0} invoice(s), with a collection rate of {collectionRate:N1}% and {lowStockCount:N0} item(s) needing stock attention.";
    }

    private static IEnumerable<ReportBreakdownRow> BuildBreakdown(
        IEnumerable<SalesDTO> invoices,
        Func<SalesDTO, string?> groupSelector,
        Func<SalesDTO, double> valueSelector,
        string fallback,
        string accent)
    {
        var groups = invoices
            .GroupBy(invoice => string.IsNullOrWhiteSpace(groupSelector(invoice)) ? fallback : groupSelector(invoice)!.Trim())
            .Select(group => new { Name = group.Key, Amount = group.Sum(valueSelector), Count = group.Count() })
            .OrderByDescending(group => group.Amount)
            .Take(8)
            .ToList();
        var max = groups.Count == 0 ? 1 : groups.Max(group => group.Amount);

        return groups.Select(group => new ReportBreakdownRow(
            group.Name,
            FormatReportMoney(group.Amount),
            $"{group.Count:N0} invoice(s)",
            group.Amount <= 0 ? 4 : Math.Max(12, group.Amount / max * 260),
            accent));
    }

    private static IEnumerable<ReportBreakdownRow> BuildMonthlyTrend(IEnumerable<SalesDTO> invoices)
    {
        var groups = invoices
            .GroupBy(invoice => new DateTime(invoice.DateSold.Year, invoice.DateSold.Month, 1))
            .OrderBy(group => group.Key)
            .TakeLast(12)
            .Select(group => new { Name = group.Key.ToString("MMM yyyy", CultureInfo.CurrentCulture), Amount = group.Sum(invoice => invoice.TotalSales), Count = group.Count() })
            .ToList();
        var max = groups.Count == 0 ? 1 : groups.Max(group => group.Amount);

        return groups.Select(group => new ReportBreakdownRow(
            group.Name,
            FormatReportMoney(group.Amount),
            $"{group.Count:N0} invoice(s)",
            group.Amount <= 0 ? 4 : Math.Max(12, group.Amount / max * 260),
            "#38DCC8"));
    }

    private static IEnumerable<ReportBreakdownRow> BuildTopSellingItems(IEnumerable<SalesDTO> invoices)
    {
        var groups = invoices
            .SelectMany(invoice => invoice.PurchasedItems ?? [])
            .GroupBy(item => string.IsNullOrWhiteSpace(item.ItemName) ? "Unnamed item" : item.ItemName.Trim())
            .Select(group => new { Name = group.Key, Amount = group.Sum(item => item.TotalPrice), Quantity = group.Sum(item => item.Quantity) })
            .OrderByDescending(group => group.Amount)
            .Take(10)
            .ToList();
        var max = groups.Count == 0 ? 1 : groups.Max(group => group.Amount);

        return groups.Select(group => new ReportBreakdownRow(
            group.Name,
            FormatReportMoney(group.Amount),
            $"{group.Quantity:N0} sold",
            group.Amount <= 0 ? 4 : Math.Max(12, group.Amount / max * 260),
            "#D8A6FF"));
    }

    private static IEnumerable<InventoryValuationRow> BuildInventoryValuation(IEnumerable<Items> items)
    {
        return items
            .Where(item => item.itemDetails is not null)
            .Select(item => new InventoryValuationRow(
                item.ItemCode,
                item.ItemName,
                item.itemDetails.OnHand,
                GetAverageRetailPrice(item),
                GetInventoryValue(item),
                GetStockStatus(item)))
            .OrderByDescending(row => row.Value)
            .Take(12);
    }

    private static IEnumerable<InventoryValuationRow> BuildLowStock(IEnumerable<Items> items)
    {
        return items
            .Where(item => item.itemDetails is not null)
            .Where(item => item.itemDetails.OnHand <= Math.Max(1, item.itemDetails.QuantityPerBox))
            .Select(item => new InventoryValuationRow(
                item.ItemCode,
                item.ItemName,
                item.itemDetails.OnHand,
                GetAverageRetailPrice(item),
                GetInventoryValue(item),
                GetStockStatus(item)))
            .OrderBy(row => row.OnHand)
            .Take(12);
    }

    private static string GetStockStatus(Items item)
    {
        var details = item.itemDetails;
        if (details is null)
        {
            return "No details";
        }

        var reorderPoint = Math.Max(1, details.QuantityPerBox);
        if (details.OnHand <= 0)
        {
            return "Out";
        }

        return details.OnHand <= reorderPoint ? "Low" : "Healthy";
    }

    private static double GetInventoryValue(Items item)
    {
        return Math.Max(0, item.itemDetails?.OnHand ?? 0) * GetAverageRetailPrice(item);
    }

    private static double GetAverageRetailPrice(Items item)
    {
        var details = item.itemDetails;
        if (details is null)
        {
            return 0;
        }

        return details.RetailPrice > 0 ? details.RetailPrice : details.WholesalePrice;
    }

    private static string FormatReportMoney(double value)
    {
        return value.ToString("N2", CultureInfo.CurrentCulture);
    }

    private void ScheduleOperationMessageClear(string? message)
    {
        _operationMessageClearToken?.Cancel();
        _operationMessageClearToken?.Dispose();
        _operationMessageClearToken = null;

        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        var delay = IsErrorMessage(message) ? TimeSpan.FromSeconds(10) : TimeSpan.FromSeconds(5);
        var tokenSource = new CancellationTokenSource();
        _operationMessageClearToken = tokenSource;
        _ = ClearOperationMessageAfterDelayAsync(message, delay, tokenSource.Token);
    }

    private async Task ClearOperationMessageAfterDelayAsync(string message, TimeSpan delay, CancellationToken token)
    {
        try
        {
            await Task.Delay(delay, token);
            if (token.IsCancellationRequested)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (OperationMessage == message)
                {
                    OperationMessage = null;
                }
            });
        }
        catch (TaskCanceledException)
        {
        }
    }

    private static bool IsErrorMessage(string message)
    {
        return message.Contains("failed", StringComparison.OrdinalIgnoreCase)
            || message.Contains("unable", StringComparison.OrdinalIgnoreCase)
            || message.Contains("invalid", StringComparison.OrdinalIgnoreCase)
            || message.Contains("required", StringComparison.OrdinalIgnoreCase)
            || message.Contains("select ", StringComparison.OrdinalIgnoreCase)
            || message.Contains("please ", StringComparison.OrdinalIgnoreCase)
            || message.Contains("must ", StringComparison.OrdinalIgnoreCase);
    }

    private async Task ExportReportsExcelAsync()
    {
        try
        {
            IsBusy = true;
            BuildReports();
            var path = Path.Combine(GetReportsFolder(), $"StockPilotProERP_Business_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");

            using var workbook = new XLWorkbook();
            var overview = workbook.Worksheets.Add("Overview");
            overview.Cell(1, 1).Value = "StockPilot Pro ERP Business Report";
            overview.Cell(2, 1).Value = ReportDateRangeText;
            overview.Cell(3, 1).Value = ReportGeneratedText;
            overview.Cell(5, 1).Value = ReportExecutiveSummary;
            overview.Range("A1:D1").Merge().Style.Font.SetBold().Font.SetFontSize(18);
            overview.Range("A5:D5").Merge();

            AddKpiSheet(workbook);
            AddBreakdownSheet(workbook, "Sales by Status", SalesByStatusReport);
            AddBreakdownSheet(workbook, "Monthly Sales Trend", MonthlySalesTrendReport);
            AddBreakdownSheet(workbook, "Sales by Agent", SalesByAgentReport);
            AddBreakdownSheet(workbook, "Sales by Customer", SalesByCustomerReport);
            AddBreakdownSheet(workbook, "Sales by Payment", SalesByPaymentReport);
            AddBreakdownSheet(workbook, "Top Selling Items", TopSellingItemsReport);
            AddInventorySheet(workbook, "Inventory Valuation", InventoryValuationReport);
            AddInventorySheet(workbook, "Stock Attention", LowStockReport);

            foreach (var worksheet in workbook.Worksheets)
            {
                worksheet.Columns().AdjustToContents();
                worksheet.SheetView.FreezeRows(1);
            }

            workbook.SaveAs(path);
            OperationMessage = $"Excel report exported: {path}";
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            OperationMessage = $"Excel export failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ExportReportsPdfAsync()
    {
        try
        {
            IsBusy = true;
            BuildReports();
            var path = Path.Combine(GetReportsFolder(), $"StockPilotProERP_Business_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            var lines = BuildPdfReportLines();
            File.WriteAllBytes(path, CreateSimplePdf(lines));
            OperationMessage = $"PDF report exported: {path}";
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            OperationMessage = $"PDF export failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static string GetReportsFolder()
    {
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StockPilot Pro ERP Reports");
        Directory.CreateDirectory(folder);
        return folder;
    }

    private void AddKpiSheet(XLWorkbook workbook)
    {
        var sheet = workbook.Worksheets.Add("KPIs");
        sheet.Cell(1, 1).Value = "Metric";
        sheet.Cell(1, 2).Value = "Value";
        sheet.Cell(1, 3).Value = "Note";
        StyleHeader(sheet.Range("A1:C1"));

        var row = 2;
        foreach (var kpi in ReportKpis)
        {
            sheet.Cell(row, 1).Value = kpi.Title;
            sheet.Cell(row, 2).Value = kpi.Value;
            sheet.Cell(row, 3).Value = kpi.Caption;
            row++;
        }
    }

    private static void AddBreakdownSheet(XLWorkbook workbook, string name, IEnumerable<ReportBreakdownRow> rows)
    {
        var sheet = workbook.Worksheets.Add(name);
        sheet.Cell(1, 1).Value = "Label";
        sheet.Cell(1, 2).Value = "Value";
        sheet.Cell(1, 3).Value = "Detail";
        StyleHeader(sheet.Range("A1:C1"));

        var row = 2;
        foreach (var item in rows)
        {
            sheet.Cell(row, 1).Value = item.Label;
            sheet.Cell(row, 2).Value = item.Value;
            sheet.Cell(row, 3).Value = item.Caption;
            row++;
        }
    }

    private static void AddInventorySheet(XLWorkbook workbook, string name, IEnumerable<InventoryValuationRow> rows)
    {
        var sheet = workbook.Worksheets.Add(name);
        sheet.Cell(1, 1).Value = "Code";
        sheet.Cell(1, 2).Value = "Item";
        sheet.Cell(1, 3).Value = "On Hand";
        sheet.Cell(1, 4).Value = "Unit Value";
        sheet.Cell(1, 5).Value = "Inventory Value";
        sheet.Cell(1, 6).Value = "Status";
        StyleHeader(sheet.Range("A1:F1"));

        var row = 2;
        foreach (var item in rows)
        {
            sheet.Cell(row, 1).Value = item.Code;
            sheet.Cell(row, 2).Value = item.Name;
            sheet.Cell(row, 3).Value = item.OnHand;
            sheet.Cell(row, 4).Value = item.UnitValue;
            sheet.Cell(row, 5).Value = item.Value;
            sheet.Cell(row, 6).Value = item.Status;
            row++;
        }

        sheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
        sheet.Column(5).Style.NumberFormat.Format = "#,##0.00";
    }

    private static void StyleHeader(IXLRange range)
    {
        range.Style.Font.SetBold();
        range.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#E8F2EF"));
        range.Style.Font.SetFontColor(XLColor.FromHtml("#12312D"));
    }

    private List<string> BuildPdfReportLines()
    {
        var lines = new List<string>
        {
            "StockPilot Pro ERP Business Report",
            ReportDateRangeText,
            ReportGeneratedText,
            string.Empty,
            ReportExecutiveSummary,
            string.Empty,
            "Executive KPIs"
        };

        lines.AddRange(ReportKpis.Select(kpi => $"{kpi.Title}: {kpi.Value} ({kpi.Caption})"));
        AddPdfSection(lines, "Sales By Status", SalesByStatusReport);
        AddPdfSection(lines, "Monthly Sales Trend", MonthlySalesTrendReport);
        AddPdfSection(lines, "Sales By Agent", SalesByAgentReport);
        AddPdfSection(lines, "Sales By Customer", SalesByCustomerReport);
        AddPdfSection(lines, "Sales By Payment Method", SalesByPaymentReport);
        AddPdfSection(lines, "Top Selling Items", TopSellingItemsReport);
        AddPdfInventorySection(lines, "Inventory Valuation", InventoryValuationReport);
        AddPdfInventorySection(lines, "Stock Attention", LowStockReport);
        return lines.SelectMany(WrapPdfLine).ToList();
    }

    private static void AddPdfSection(List<string> lines, string title, IEnumerable<ReportBreakdownRow> rows)
    {
        lines.Add(string.Empty);
        lines.Add(title);
        lines.AddRange(rows.Select(row => $"{row.Label} | {row.Value} | {row.Caption}"));
    }

    private static void AddPdfInventorySection(List<string> lines, string title, IEnumerable<InventoryValuationRow> rows)
    {
        lines.Add(string.Empty);
        lines.Add(title);
        lines.AddRange(rows.Select(row => $"{row.Code} | {row.Name} | On hand {row.OnHandText} | Unit {row.UnitValueText} | Value {row.ValueText} | {row.Status}"));
    }

    private static IEnumerable<string> WrapPdfLine(string line)
    {
        const int maxLength = 112;
        if (line.Length <= maxLength)
        {
            yield return line;
            yield break;
        }

        for (var index = 0; index < line.Length; index += maxLength)
        {
            yield return line.Substring(index, Math.Min(maxLength, line.Length - index));
        }
    }

    private static byte[] CreateSimplePdf(IReadOnlyList<string> lines)
    {
        const int linesPerPage = 34;
        var pages = lines.Chunk(linesPerPage).ToList();
        if (pages.Count == 0)
        {
            pages.Add(["No report data."]);
        }

        var objects = new List<string> { string.Empty };
        var pageObjectIds = new List<int>();
        var fontObjectId = 0;

        int AddObject(string body)
        {
            objects.Add(body);
            return objects.Count - 1;
        }

        AddObject(string.Empty);
        AddObject(string.Empty);

        foreach (var pageLines in pages)
        {
            var content = BuildPdfPageContent(pageLines);
            var contentObjectId = AddObject($"<< /Length {Encoding.ASCII.GetByteCount(content)} >>\nstream\n{content}\nendstream");
            var pageObjectId = AddObject($"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 842 595] /Resources << /Font << /F1 {{FONT}} 0 R >> >> /Contents {contentObjectId} 0 R >>");
            pageObjectIds.Add(pageObjectId);
        }

        fontObjectId = AddObject("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>");
        objects[1] = "<< /Type /Catalog /Pages 2 0 R >>";
        objects[2] = $"<< /Type /Pages /Kids [{string.Join(" ", pageObjectIds.Select(id => $"{id} 0 R"))}] /Count {pageObjectIds.Count} >>";

        for (var i = 1; i < objects.Count; i++)
        {
            objects[i] = objects[i].Replace("{FONT}", fontObjectId.ToString(CultureInfo.InvariantCulture));
        }

        var builder = new StringBuilder();
        var offsets = new List<int> { 0 };
        builder.Append("%PDF-1.4\n");

        for (var i = 1; i < objects.Count; i++)
        {
            offsets.Add(Encoding.ASCII.GetByteCount(builder.ToString()));
            builder.Append(CultureInfo.InvariantCulture, $"{i} 0 obj\n{objects[i]}\nendobj\n");
        }

        var xrefOffset = Encoding.ASCII.GetByteCount(builder.ToString());
        builder.Append(CultureInfo.InvariantCulture, $"xref\n0 {objects.Count}\n0000000000 65535 f \n");

        for (var i = 1; i < objects.Count; i++)
        {
            builder.Append(CultureInfo.InvariantCulture, $"{offsets[i]:0000000000} 00000 n \n");
        }

        builder.Append(CultureInfo.InvariantCulture, $"trailer\n<< /Size {objects.Count} /Root 1 0 R >>\nstartxref\n{xrefOffset}\n%%EOF");
        return Encoding.ASCII.GetBytes(builder.ToString());
    }

    private static string BuildPdfPageContent(IEnumerable<string> lines)
    {
        var content = new StringBuilder();
        var y = 555;
        var first = true;

        foreach (var line in lines)
        {
            var fontSize = first ? 16 : 9;
            content.Append(CultureInfo.InvariantCulture, $"BT /F1 {fontSize} Tf 40 {y} Td ({EscapePdf(line)}) Tj ET\n");
            y -= first ? 24 : 15;
            first = false;
        }

        return content.ToString();
    }

    private static string EscapePdf(string value)
    {
        return value.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
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
                Cluster = string.Empty,
                isActive = true,
                RegisterDate = DateTime.Now
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
                DateLastLogin = null
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

        var retailPrice = await PromptNumberAsync("Add Item", "Retail price", 0);
        if (retailPrice is null) return;

        var wholesalePrice = await PromptNumberAsync("Add Item", "Wholesale price", 0);
        if (wholesalePrice is null) return;

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
                    RetailPrice = retailPrice.Value,
                    WholesalePrice = wholesalePrice.Value,
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
                DateLastLogin = agent.DateLastLogin
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

        var retailPrice = await PromptNumberAsync("Edit Item", "Retail price", item.itemDetails.RetailPrice);
        if (retailPrice is null) return;

        var wholesalePrice = await PromptNumberAsync("Edit Item", "Wholesale price", item.itemDetails.WholesalePrice);
        if (wholesalePrice is null) return;

        try
        {
            IsBusy = true;
            item.ItemCode = code.Trim();
            item.ItemName = name.Trim();
            item.ItemDescription = description.Trim();
            item.itemDetails.OnHand = onHand.Value;
            item.itemDetails.QuantityPerBox = qtyPerBox.Value;
            item.itemDetails.RetailPrice = retailPrice.Value;
            item.itemDetails.WholesalePrice = wholesalePrice.Value;

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

        return PricingMode == "Retail"
            ? item.itemDetails.RetailPrice
            : item.itemDetails.WholesalePrice;
    }

    private void UpdateAgentHeader()
    {
        var agent = SessionManager.AgentDetails;
        AgentName = agent?.AgentName ?? "StockPilot Pro ERP";

        var role = agent?.AgentRole ?? "User";
        AgentMeta = role;

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

    public sealed class ReportKpi(string title, string value, string caption, string accentColor)
    {
        public string Title { get; } = title;

        public string Value { get; } = value;

        public string Caption { get; } = caption;

        public string AccentColor { get; } = accentColor;
    }

    public sealed class ReportBreakdownRow(string label, string value, string caption, double barWidth, string accentColor)
    {
        public string Label { get; } = label;

        public string Value { get; } = value;

        public string Caption { get; } = caption;

        public double BarWidth { get; } = barWidth;

        public string AccentColor { get; } = accentColor;
    }

    public sealed class InventoryValuationRow(string code, string name, double onHand, double unitValue, double value, string status)
    {
        public string Code { get; } = code;

        public string Name { get; } = name;

        public double OnHand { get; } = onHand;

        public string OnHandText => OnHand.ToString("N0", CultureInfo.CurrentCulture);

        public double UnitValue { get; } = unitValue;

        public string UnitValueText => UnitValue.ToString("N2", CultureInfo.CurrentCulture);

        public double Value { get; } = value;

        public string ValueText => Value.ToString("N2", CultureInfo.CurrentCulture);

        public string Status { get; } = status;

        public string StatusColor => Status switch
        {
            "Out" => "#F07F7F",
            "Low" => "#F0B35A",
            _ => "#87E0B0"
        };
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
