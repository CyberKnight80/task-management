using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using TaskManagment.Infrastructure.DataContracts;

namespace TaskManagment.Infrastructure.Services;

public class ApiClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string AuthEndpoint = "http://10.0.2.2:5223/api/auth";
    //private const string AuthEndpoint = "http://localhost:5223/api/auth"; 

    public ApiClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TokenResponse> LoginAsync(string username, string password)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.PostAsJsonAsync($"{AuthEndpoint}/login", new { username, password });

        if (response.IsSuccessStatusCode)
        {
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            return tokenResponse;
        }
        else
        {
            // Обработка ошибок или неудачного статуса ответа
            throw new HttpRequestException($"Error logging in: {response.ReasonPhrase}");
        }
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var jsonContent = JsonConvert.SerializeObject(
            new RegisterModel() { Password = password, Username = username });
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync($"{AuthEndpoint}/register", content);

        return response.IsSuccessStatusCode;
    }
}

