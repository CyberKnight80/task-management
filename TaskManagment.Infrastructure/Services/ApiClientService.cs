using System.Net.Http.Json;
using System.Text.Json;
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

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content
                .ReadFromJsonAsync<RefreshTokenResponse>(
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
                $"Refresh token fails: {response.ReasonPhrase}", null,
                response.StatusCode);
        }
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
}

