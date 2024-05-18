using System.Threading.Tasks;
using System.Windows.Input;
using TaskManagement.Infrastructure.Services;
using TaskManagement.Infrastructure.Utils;
using TaskManagement.Infrastructure.ViewModels;

namespace TaskManagementWin.ViewModels;

internal class MainMenuViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;

    public MainMenuViewModel(
        INavigationService navigationService,
        IAuthenticationService authenticationService)
    {
        _navigationService = navigationService;
        _authenticationService = authenticationService;

        LogoutCommand = new RelayCommand(HandleLogoutCommandAsync);
        GoToHomePageCommand = new RelayCommand(HandleGoToHomePageCommandAsync);
        GoToTeamsPageCommand = new RelayCommand(HandleGoToTeamsPageCommandAsync);
    }

    public ICommand LogoutCommand { get; }
    public ICommand GoToHomePageCommand { get; }
    public ICommand GoToTeamsPageCommand { get; }

    private async Task HandleLogoutCommandAsync()
    {
        await _authenticationService.LogoutAsync();
        await _navigationService.GoToAsync(Route.Login, keepHistory: false);
    }

    private Task HandleGoToHomePageCommandAsync() => 
        _navigationService.GoToAsync(Route.Welcome);

    private Task HandleGoToTeamsPageCommandAsync() =>
        _navigationService.GoToAsync(Route.Teams);
}
