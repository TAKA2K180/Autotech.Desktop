using Autotech.Desktop.MAUI.Viewmodels;

namespace Autotech.Desktop.MAUI.Views;

public partial class InvoiceDetailsPage : ContentPage
{
    private readonly InvoiceDetailsPageViewModel _viewModel;

    public InvoiceDetailsPage(Guid invoiceId)
    {
        InitializeComponent();
        _viewModel = new InvoiceDetailsPageViewModel(invoiceId);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}
