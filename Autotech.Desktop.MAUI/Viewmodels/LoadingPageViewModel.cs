using System.Windows.Input;

namespace Autotech.Desktop.MAUI.Viewmodels;

public sealed class LoadingPageViewModel : ViewModelBase
{
    private string _statusText = "Preparing your workspace...";
    private string? _errorMessage;
    private bool _hasError;
    private bool _isLoading = true;

    public LoadingPageViewModel()
    {
        RetryCommand = new Command(async () => await LoadAsync(), () => !IsLoading);
    }

    public ICommand RetryCommand { get; }

    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }

    public bool HasError
    {
        get => _hasError;
        private set => SetProperty(ref _hasError, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (SetProperty(ref _isLoading, value))
            {
                ((Command)RetryCommand).ChangeCanExecute();
                OnPropertyChanged(nameof(CanRetry));
            }
        }
    }

    public bool CanRetry => !IsLoading;

    public async Task LoadAsync()
    {
        HasError = false;
        ErrorMessage = null;
        IsLoading = true;
        StatusText = "Preparing your workspace...";

        try
        {
            var progress = new Progress<string>(message => StatusText = message);
            await AppLoadService.Current.LoadInitialDataAsync(progress);
            StatusText = "Opening dashboard...";
            await Shell.Current.GoToAsync("//DashboardPage");
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "Could not load the workspace. Please check your connection and try again.";
            StatusText = "Loading stopped";
            System.Diagnostics.Debug.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
