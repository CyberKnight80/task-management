using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using Microsoft.Extensions.Logging;
using TaskManagement.Infrastructure.DataContracts;

namespace TaskManagement.Infrastructure.Services;

public class ApiClientService
{
    public const string AutorizedHttpClient = "AutorizedHttpClient";

    private readonly ILogger<ApiClientService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _serverAddress;

    public ApiClientService(ILogger<ApiClientService> logger,
        IHttpClientFactory httpClientFactory,
        string serverAddress)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _serverAddress = $"{serverAddress}/api";

    }

    #region Auth

    public async Task<LoginResponse> LoginAsync(string username, string password,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClientFactory
            .CreateClient()
            .PostAsJsonAsync($"{_serverAddress}/auth/login", new LoginRequest
                {
                    Username = username,
                    Password = password
                },
                cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content
                .ReadFromJsonAsync<LoginResponse>(
                    cancellationToken: cancellationToken);

            if (responseContent == null)
            {
                throw new JsonException("Response has unknown format");
            }

            return responseContent;
        }
        else
        {
            throw new HttpRequestException(
                $"Error logging in: {response.ReasonPhrase}", null,
                response.StatusCode);
        }
    }

    public async Task<bool> RegisterAsync(string username, string password,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClientFactory
            .CreateClient()
            .PostAsJsonAsync($"{_serverAddress}/auth/register", new RegisterRequest
                {
                    Username = username,
                    Password = password
                },
                cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Error logging in: {response.ReasonPhrase}", null,
                response.StatusCode);
        }

        return true;
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(string accessToken,
        string refreshToken, CancellationToken cancellationToken = default)
    {
        var response = await _httpClientFactory
            .CreateClient()
            .PostAsJsonAsync($"{_serverAddress}/auth/refresh-token", new RefreshTokenRequest
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                },
                cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Refresh token fails: {response.ReasonPhrase}", null,
                response.StatusCode);
        }

        var responseContent = await response.Content
            .ReadFromJsonAsync<RefreshTokenResponse>(
                cancellationToken: cancellationToken);

        if (responseContent == null)
        {
            throw new JsonException("Response has unknown format");
        }

        return responseContent;
    }

    public async Task<bool> ValidateTokenAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClientFactory
            .CreateClient(AutorizedHttpClient)
            .GetAsync($"{_serverAddress}/auth/validate-token", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Validate token fails: {response.ReasonPhrase}", null,
                response.StatusCode);
        }

        return true;
    }

    public async Task<bool> LogoutAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClientFactory
            .CreateClient(AutorizedHttpClient)
            .GetAsync($"{_serverAddress}/auth/logout", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Logout fails: {response.ReasonPhrase}", null,
                response.StatusCode);
        }

        return true;
    }

    #endregion

    #region Teams

    public async Task<IEnumerable<TeamEntity>> GetTeamsAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClientFactory
            .CreateClient(AutorizedHttpClient)
            .GetAsync($"{_serverAddress}/teams", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Get teams request fails: {response.ReasonPhrase}", null,
                response.StatusCode);
        }

        var responseContent = await response.Content
            .ReadFromJsonAsync<IEnumerable<TeamEntity>>(
                cancellationToken: cancellationToken);

        if (responseContent == null)
        {
            throw new JsonException("Response has unknown format");
        }

        return responseContent;
    }

    public async Task<GetTeamResponse> GetTeamAsync(int teamId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClientFactory
            .CreateClient(AutorizedHttpClient)
            .GetAsync($"{_serverAddress}/teams/{teamId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Get team request fails: {response.ReasonPhrase}", null,
                response.StatusCode);
        }

        return await ParseGetTeamResponseAsync(response, cancellationToken);
    }

    public async Task<GetTeamResponse> AddUserToTeamAsync(int teamId,
        int userId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClientFactory
            .CreateClient(AutorizedHttpClient)
            .PostAsync($"{_serverAddress}/teams/{teamId}/users/{userId}",
                null, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Add user to team request fails: {response.ReasonPhrase}",
                null, response.StatusCode);
        }

        return await ParseGetTeamResponseAsync(response, cancellationToken);
    }

    public async Task<GetTeamResponse> RemoveUserFromTeamAsync(int teamId,
        int userId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClientFactory
            .CreateClient(AutorizedHttpClient)
            .DeleteAsync($"{_serverAddress}/teams/{teamId}/users/{userId}",
                cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Remove user from team request fails: {response.ReasonPhrase}",
                null, response.StatusCode);
        }

        return await ParseGetTeamResponseAsync(response, cancellationToken);
    }

    private async Task<GetTeamResponse> ParseGetTeamResponseAsync(
        HttpResponseMessage? response,
        CancellationToken cancellationToken = default)
    {
        if (response is null)
        {
            throw new ArgumentNullException(nameof(response));
        }

        var responseContent = await response.Content
            .ReadFromJsonAsync<GetTeamResponse>(
                cancellationToken: cancellationToken);

        if (responseContent == null)
        {
            throw new JsonException("Response has unknown format");
        }

        return responseContent;
    }

    #endregion
}

