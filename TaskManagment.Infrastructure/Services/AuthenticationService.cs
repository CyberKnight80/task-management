using Microsoft.Extensions.Logging;

namespace TaskManagement.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger;
    private readonly ISecureStorageService _secureStorageService;
    private readonly ApiClientService _apiClientService;

    public AuthenticationService(ILogger<AuthenticationService> logger,
        ISecureStorageService secureStorageService,
        ApiClientService apiClientService)
    {
        _logger = logger;
        _secureStorageService = secureStorageService;
        _apiClientService = apiClientService;
    }

    public async Task<bool> AuthenticateAsync(string login, string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _apiClientService
                .LoginAsync(
                    username: login,
                    password: password,
                    cancellationToken);

            await _secureStorageService.SetAsync(SecureStorageKey.AccessToken, response.AccessToken);
            await _secureStorageService.SetAsync(SecureStorageKey.RefreshToken, response.RefreshToken);
            await _secureStorageService.SetAsync(SecureStorageKey.UserId, response.UserId.ToString());
            await _secureStorageService.SetAsync(SecureStorageKey.Username, response.Username);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to authenticate");
            return false;
        }
    }

    public async Task<bool> CheckIsAuthenticatedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _apiClientService.ValidateTokenAsync(cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to check for authentication status");
            return false;
        }
    }

    public async Task<bool> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var accessToken = await _secureStorageService.GetAsync(SecureStorageKey.AccessToken);
            var refreshToken = await _secureStorageService.GetAsync(SecureStorageKey.RefreshToken);

            var response = await _apiClientService
                .RefreshTokenAsync(
                    accessToken: accessToken,
                    refreshToken: refreshToken,
                    cancellationToken);

            await _secureStorageService.SetAsync(SecureStorageKey.AccessToken, response.AccessToken);
            await _secureStorageService.SetAsync(SecureStorageKey.RefreshToken, response.RefreshToken);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to refresh token");
            return false;
        }
    }
}

