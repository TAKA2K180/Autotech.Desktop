using Autotech.Desktop.Core.Models;
using Autotech.Desktop.MAUI.Viewmodels;
using Microsoft.Maui.Layouts;

namespace Autotech.Desktop.MAUI.Views;

public partial class DashboardPage : ContentPage
{
    private DashboardPageViewModel ViewModel => (DashboardPageViewModel)BindingContext;
    private IDispatcherTimer? _clockTimer;
    private bool _loaded;

    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = new DashboardPageViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_loaded)
        {
            _loaded = true;
            await ViewModel.LoadAsync();
        }

        _clockTimer ??= Dispatcher.CreateTimer();
        _clockTimer.Interval = TimeSpan.FromSeconds(1);
        _clockTimer.Tick -= ClockTimer_Tick;
        _clockTimer.Tick += ClockTimer_Tick;
        _clockTimer.Start();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (SalesPanels is null)
        {
            return;
        }

        SalesPanels.Direction = width < 1050 ? FlexDirection.Column : FlexDirection.Row;

        var reservedHeight = width < 1050 ? 720 : 560;
        var listHeight = Math.Max(360, height - reservedHeight);
        ItemsList.HeightRequest = listHeight;

        var itemsViewportWidth = ItemsPanel.Width > 0 ? ItemsPanel.Width - 24 : width - 64;
        itemsViewportWidth = Math.Max(320, itemsViewportWidth);
        ItemsScroll.WidthRequest = itemsViewportWidth;
        ItemsList.WidthRequest = Math.Max(1280, itemsViewportWidth);

        var invoiceViewportWidth = InvoiceGridPanel.Width > 0 ? InvoiceGridPanel.Width - 20 : width - 64;
        invoiceViewportWidth = Math.Max(480, invoiceViewportWidth);
        InvoiceScroll.WidthRequest = invoiceViewportWidth;
        InvoiceList.WidthRequest = Math.Max(2100, invoiceViewportWidth);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _clockTimer?.Stop();
    }

    private void ClockTimer_Tick(object? sender, EventArgs e)
    {
        ViewModel.UpdateClock();
    }

    private void ItemsCollection_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Items item)
        {
            ViewModel.AddItemCommand.Execute(item);
        }

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }

}
