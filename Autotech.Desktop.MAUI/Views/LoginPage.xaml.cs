using Autotech.Desktop.MAUI.Viewmodels;

namespace Autotech.Desktop.MAUI.Views
{
    public partial class LoginPage : ContentPage
    {
        private LoginPageViewModel ViewModel => (LoginPageViewModel)BindingContext;

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginPageViewModel();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (LoginLayout is null || SignInPanel is null)
        {
            return;
        }

        if (width < 760)
        {
            LoginLayout.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new(GridLength.Star)
            };
            LoginLayout.RowDefinitions = new RowDefinitionCollection
            {
                new(GridLength.Auto),
                new(GridLength.Auto)
            };
            SignInPanel.SetValue(Grid.RowProperty, 1);
            SignInPanel.SetValue(Grid.ColumnProperty, 0);
        }
        else
        {
            LoginLayout.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new(new GridLength(1.15, GridUnitType.Star)),
                new(new GridLength(0.85, GridUnitType.Star))
            };
            LoginLayout.RowDefinitions = new RowDefinitionCollection
            {
                new(GridLength.Auto)
            };
            SignInPanel.SetValue(Grid.RowProperty, 0);
            SignInPanel.SetValue(Grid.ColumnProperty, 1);
        }
    }

    private async void PasswordEntry_Completed(object? sender, EventArgs e)
        {
            await ViewModel.SignInAsync();
        }
    }
}
