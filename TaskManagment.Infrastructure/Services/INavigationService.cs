namespace TaskManagment.Infrastructure.Services;

public interface INavigationService
{
    Task GoToAsync(Route route, bool keepHistory = true);
    Task GoBackAsync();
}

public enum Route
{
    Root,
    Back,
    Login,
    Register,
    Welcome,
}

public static class RouteExtensions
{
    public static string ToRouteString(this Route route) => route switch
    {
        Route.Back => "..",
        Route.Login => "login",
        Route.Register => "register",
        Route.Welcome => "welcome",
        _ => throw new NotSupportedException($"Route {route} is not supported")
    };

}

