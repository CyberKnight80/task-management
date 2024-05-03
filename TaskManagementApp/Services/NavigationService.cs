using TaskManagement.Infrastructure.Services;

namespace TaskManagementApp.Services;

public class NavigationService : INavigationService
{
    public Task GoToAsync(Route route, bool keepHistory = true)
    {
        var prefix = keepHistory ? "/" : "//";
        var routeS = $"{prefix}{route.MapRouteToPath()}";
        return Shell.Current.GoToAsync(routeS);
    }

    public Task GoBackAsync() => 
        Shell.Current.GoToAsync("..");
}

internal static class RouteExtensions
{
    public static string MapRouteToPath(this Route route) => route switch
    {
        Route.Back => "..",
        Route.Login => "login",
        Route.Register => "register",
        Route.Welcome => "welcome",
        _ => throw new NotSupportedException($"Route {route} is not supported")
    };
}

