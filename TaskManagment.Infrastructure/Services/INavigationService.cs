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

