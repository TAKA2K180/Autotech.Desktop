using Autotech.Desktop.MAUI.Viewmodels;

namespace Autotech.Desktop.MAUI.Views;

public partial class LoadingPage : ContentPage
{
    private LoadingPageViewModel ViewModel => (LoadingPageViewModel)BindingContext;
    private bool _started;

    public LoadingPage()
    {
        InitializeComponent();
        BindingContext = new LoadingPageViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_started)
        {
            return;
        }

        _started = true;
        await ViewModel.LoadAsync();
    }
}
