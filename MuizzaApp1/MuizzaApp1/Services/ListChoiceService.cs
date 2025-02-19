using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MuizzaApp1.Models;

public interface IListChoiceService
{
    Task<ListChoice> GetListChoiceByTitleAsync(string title);
}

public class ListChoiceService : IListChoiceService
{
    private readonly HttpClient _httpClient;
    private readonly HttpClientHandler _handler;

    public ListChoiceService(IConfiguration configuration)
    {
        _handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        _httpClient = new HttpClient(_handler);
        
        var baseUrl = configuration["ApiSettings:BaseUrl"] 
            ?? throw new ArgumentNullException("ApiSettings:BaseUrl not found in configuration");
        
        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<ListChoice> GetListChoiceByTitleAsync(string title)
    {
        try
        {
            var requestUrl = $"api/ListChoice/title/{Uri.EscapeDataString(title)}";
            Console.WriteLine($"Making request to: {_httpClient.BaseAddress}{requestUrl}");
            
            var response = await _httpClient.GetAsync(requestUrl);
            Console.WriteLine($"Response status: {response.StatusCode}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error response content: {errorContent}");
                throw new HttpRequestException($"API returned {response.StatusCode}: {errorContent}");
            }
            
            var result = await response.Content.ReadFromJsonAsync<ListChoice>();
            Console.WriteLine($"Successfully retrieved ListChoice data: {result?.Title}");
            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching ListChoice: {ex.Message}");
            Console.WriteLine($"Exception type: {ex.GetType().Name}");
            Console.WriteLine($"Exception message: {ex.Message}");
            Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            throw;
        }
    }
} 