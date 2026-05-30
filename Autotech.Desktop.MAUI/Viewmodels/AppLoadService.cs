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
        // Load first page into CurrentPageItems
        Context.CurrentPageItems.AddRange(await _itemServices.GetPaginatedItemsAsync(Context.CurrentItemPage, PageSize));

        // Also load the full items list for searches that should span all items
        try
        {
            var all = await _itemServices.GetAllItemsAsync();
            if (all is not null)
            {
                Context.AllItems.AddRange(all);
            }
        }
        catch
        {
            // If full items cannot be loaded, fall back to paged items only
        }

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
        var pageItems = await _itemServices.GetPaginatedItemsAsync(page, PageSize);
        Context.CurrentPageItems.AddRange(pageItems);

        // Ensure AllItems accumulates items as pages are loaded (don't replace)
        if (pageItems is not null)
        {
            foreach (var item in pageItems)
            {
                if (!Context.AllItems.Contains(item))
                {
                    Context.AllItems.Add(item);
                }
            }
        }
    }
}
