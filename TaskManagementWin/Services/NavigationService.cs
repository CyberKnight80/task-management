using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using TaskManagementWin.Pages;
using TaskManagement.Infrastructure.Services;

namespace TaskManagementWin.Services;

internal class NavigationService : INavigationService
{
    private readonly System.Windows.Navigation.NavigationService _navigationService;

    public NavigationService(NavigationWindow navigationWindow)
    {
        _navigationService = navigationWindow.NavigationService;
    }

    public Task GoBackAsync()
    {
        if (_navigationService.CanGoBack)
        {
            _navigationService.GoBack();
        }

        return Task.CompletedTask;
    }

    public Task GoToAsync(Route route, IDictionary<string, object> parameters, bool keepHistory = true)
    {
        if (!keepHistory)
        {
            // don't work - there is still exist the back button
            while (_navigationService.CanGoBack)
            {
                _navigationService.RemoveBackEntry();
            }
        }

        _navigationService.Navigate(MapRouteToPage(route, parameters));

        return Task.CompletedTask;
    }

#pragma warning disable CS8603 // Possible null reference return.
    private Page MapRouteToPage(Route route, IDictionary<string, object> parameters) => route switch
    {
        Route.Login => new LoginPage(),
        Route.Register => new RegisterPage(),
        Route.Welcome => new WelcomePage(),
        _ => throw new NotSupportedException(),
    };
#pragma warning restore CS8603 // Possible null reference return.
}
