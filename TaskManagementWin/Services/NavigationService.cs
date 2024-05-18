using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using TaskManagementWin.Pages;
using TaskManagement.Infrastructure.Services;
using System.Collections.Generic;
using TaskManagement.Infrastructure.ViewModels;

namespace TaskManagementWin.Services;

internal class NavigationService : INavigationService
{
    public NavigationService()
    {
    }

    private NavigationWindow NavigationWindow => App.Current.MainWindow as NavigationWindow ?? 
        throw new NotSupportedException("Main window should be navigation window");

    public Task GoBackAsync()
    {
        if (NavigationWindow.CanGoBack)
        {
            NavigationWindow.GoBack();
        }

        return Task.CompletedTask;
    }

    public Task GoToAsync(Route route, IDictionary<string, object>? parameters = null, bool keepHistory = true)
    {
        if (!keepHistory)
        {
            // don't work - there is still exist the back button
            while (NavigationWindow.CanGoBack)
            {
                NavigationWindow.RemoveBackEntry();
            }
        }

        NavigationWindow.Navigate(MapRouteToPage(route, parameters));

        return Task.CompletedTask;
    }

#pragma warning disable CS8603 // Possible null reference return.
    private Page MapRouteToPage(Route route, IDictionary<string, object>? parameters = null) => route switch
    {
        Route.Login => new LoginPage(),
        Route.Register => new RegisterPage(),
        Route.Welcome => new WelcomePage(),
        Route.Teams => new TeamsPage(),
        Route.TeamDetails => new TeamDetailsPage(int.Parse(parameters[TeamDetailsViewModel.TeamIdQueryKey].ToString())),
        _ => throw new NotSupportedException(),
    };
#pragma warning restore CS8603 // Possible null reference return.
}
