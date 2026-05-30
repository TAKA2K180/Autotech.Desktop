using System.Windows.Input;
using Autotech.Desktop.BusinessLayer.Services;
using Autotech.Desktop.Core.Models;

namespace Autotech.Desktop.MAUI.Viewmodels;

public sealed class LoginPageViewModel : ViewModelBase
{
    private readonly LoginServices _loginServices = new();
    private string? _username;
    private string? _password;
    private string? _errorMessage;
    private bool _isBusy;

    public LoginPageViewModel()
    {
        LoginCommand = new Command(async () => await SignInAsync(), () => !IsBusy);
        VersionText = $"Version {ReadVersion()}";
    }

    public ICommand LoginCommand { get; }

    public string VersionText { get; }

    public string? Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string? Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            if (SetProperty(ref _errorMessage, value))
            {
                OnPropertyChanged(nameof(HasError));
            }
        }
    }

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
            {
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }
    }

    public async Task SignInAsync()
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter both username and password.";
            return;
        }

        IsBusy = true;

        try
        {
            var loginSuccessful = await _loginServices.LoginAsync(Username.Trim(), Password);
            LoginHelper.isLoggedIn = loginSuccessful;

            if (!loginSuccessful)
            {
                ErrorMessage = "Invalid username or password. Please try again.";
                return;
            }

            Username = string.Empty;
            Password = string.Empty;
            await Shell.Current.GoToAsync("//LoadingPage");
        }
        catch (Exception ex)
        {
            ErrorMessage = "Unable to sign in. Please check your connection and try again.";
            System.Diagnostics.Debug.WriteLine(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static string ReadVersion()
    {
        try
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("version.txt").GetAwaiter().GetResult();
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd().Trim();
        }
        catch
        {
            return AppInfo.Current.VersionString;
        }
    }
}
