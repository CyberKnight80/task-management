
namespace TaskManagment.Infrastructure.Services;

public interface IAuthenticationService
{
    bool IsAuthenticated { get; }

    Task<bool> LoginAsync(string username, string password,
        CancellationToken cancellationToken = default);

    Task<bool> RegisterAsync(string username, string password,
        CancellationToken cancellationToken = default);

    void Logout();
}

