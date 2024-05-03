using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TaskManagment.Infrastructure.DataContracts;

namespace TaskManagment.Infrastructure.Services;

public class ApiClientService
{
    public const string AutorizedHttpClient = "AutorizedHttpClient";
    private const string AuthEndpoint = "http://10.0.2.2:5223/api/auth";
    //private const string AuthEndpoint = "http://localhost:5223/api/auth"; 

    private readonly ILogger<ApiClientService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiClientService(ILogger<ApiClientService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<LoginResponse> LoginAsync(string username, string password,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClientFactory
            .CreateClient()
            .PostAsJsonAsync($"{AuthEndpoint}/login", new LoginRequest
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
            .PostAsJsonAsync($"{AuthEndpoint}/register", new RegisterRequest
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
            .PostAsJsonAsync($"{AuthEndpoint}/refresh-token", new RefreshTokenRequest
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
            .GetAsync($"{AuthEndpoint}/validate-token", cancellationToken);

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
            .GetAsync($"{AuthEndpoint}/logout", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Logout fails: {response.ReasonPhrase}", null,
                response.StatusCode);
        }

        return true;
    }
}

