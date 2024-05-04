using System.Windows.Input;
using TaskManagement.Infrastructure.Utils;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Infrastructure.ViewModels;

public class WelcomeViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;

    private string _error;

    public WelcomeViewModel(
        IAuthenticationService authenticationService,
        INavigationService navigationService)
    {
        _authenticationService = authenticationService;
        _navigationService = navigationService;

        LogoutCommand = new RelayCommand(HandleLogoutAsync);
    }

    public string Error
    {
        get => _error;
        set => SetField(ref _error, value);
    }

    public ICommand LogoutCommand { get; }

    private async Task HandleLogoutAsync()
    {
        await _authenticationService.LogoutAsync();
        await _navigationService.GoToAsync(Route.Login, keepHistory: false);
    }
}

