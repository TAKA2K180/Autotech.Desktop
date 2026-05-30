using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.BusinessLayer.Services;

namespace Autotech.Desktop.MAUI.Viewmodels;

public sealed class AppLoadService
{
    public const int PageSize = 25;

    private readonly ItemServices _itemServices = new();
    private readonly AccountService _accountService = new();
    private readonly SalesService _salesService = new();

    public static AppLoadService Current { get; } = new();

    public AppLoadContext Context { get; } = new();

    public async Task LoadInitialDataAsync(IProgress<string>? progress = null)
    {
        Context.Clear();

        progress?.Report("Loading inventory...");
        Context.CurrentPageItems.AddRange(await _itemServices.GetPaginatedItemsAsync(Context.CurrentItemPage, PageSize));

        progress?.Report("Loading accounts...");
        var locationId = SessionManager.AgentDetails?.LocationId ?? Guid.Empty;
        var accounts = locationId == Guid.Empty
            ? await _accountService.GetAllAccountsAsync()
            : await _accountService.GetAccountsByLocationIdAsync(locationId);
        Context.Accounts.AddRange(accounts);

        progress?.Report("Loading invoices...");
        Context.Invoices.AddRange(await _salesService.GetAllInvoicesAsync());

        Context.HasLoaded = true;
    }

    public async Task LoadItemPageAsync(int page)
    {
        Context.CurrentItemPage = page;
        Context.CurrentPageItems.Clear();
        Context.CurrentPageItems.AddRange(await _itemServices.GetPaginatedItemsAsync(page, PageSize));
    }
}
