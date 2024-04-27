using TaskManagment.Infrastructure.Services;

namespace TaskManagementApp;

public partial class App : Application
{
    private readonly INavigationService _navigationService;

    public App(
        INavigationService navigationService)
    {
        InitializeComponent();

        _navigationService = navigationService;
        MainPage = new AppShell();
    }

    protected override async void OnStart()
    {
        base.OnStart();

        var isAuthenticated = false;

        if (isAuthenticated)
        {
            await _navigationService.GoToAsync(Route.Welcome, keepHistory: false);
        }
    }
}

