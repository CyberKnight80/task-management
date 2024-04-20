using TaskManagment.Infrastructure.Services;

namespace TaskManagementApp.Services;

public class NavigationService : INavigationService
{
    public Task GoToAsync(Route route, bool keepHistory = true)
    {
        var prefix = keepHistory ? "/" : "//";
        var routeS = $"{prefix}{route.ToRouteString()}";
        return Shell.Current.GoToAsync(routeS);
    }
        

    public Task GoBackAsync() => 
        Shell.Current.GoToAsync("..");
}

