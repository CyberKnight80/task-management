namespace TaskManagment.Infrastructure.Services;

public interface IAuthenticationService
{
    Task<bool> AuthenticateAsync(string login, string password,
        CancellationToken cancellationToken = default);

    Task<bool> CheckIsAuthenticatedAsync(CancellationToken cancellationToken = default);

    Task<bool> RefreshTokenAsync(CancellationToken cancellationToken = default);
}

