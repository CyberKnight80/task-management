using TaskManagementApp.Pages;
using TaskManagment.Infrastructure.Services;

namespace TaskManagementApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    private void RegisterRoutes()
    {
        Routing.RegisterRoute(Route.Login.ToRouteString(), typeof(LoginPage));
        Routing.RegisterRoute(Route.Register.ToRouteString(), typeof(RegisterPage));
        Routing.RegisterRoute(Route.Welcome.ToRouteString(), typeof(WelcomePage));
    }
}