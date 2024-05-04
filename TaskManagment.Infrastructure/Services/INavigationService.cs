
namespace TaskManagement.Infrastructure.Services;

public interface INavigationService
{
    Task GoToAsync(Route route, IDictionary<string, object>? parameters = null,
        bool keepHistory = true);

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

