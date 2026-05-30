using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.Core.Models;

namespace Autotech.Desktop.MAUI.Viewmodels;

public sealed class AppLoadContext
{
    public int CurrentItemPage { get; set; } = 1;

    public List<Items> CurrentPageItems { get; } = new();

    public List<Accounts> Accounts { get; } = new();

    public List<SalesDTO> Invoices { get; } = new();

    public bool HasLoaded { get; set; }

    public void Clear()
    {
        CurrentItemPage = 1;
        CurrentPageItems.Clear();
        Accounts.Clear();
        Invoices.Clear();
        HasLoaded = false;
    }
}
