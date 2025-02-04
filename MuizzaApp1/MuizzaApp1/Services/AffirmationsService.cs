using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MuizzaApp1.Models;

public interface IAffirmationsService
{
    Task<List<Affirmation>> GetAffirmationsAsync();
}

public class AffirmationsService : IAffirmationsService
{
    private readonly HttpClient _httpClient;

    public AffirmationsService(IConfiguration configuration)
    {
        var baseUrl = configuration["ApiSettings:BaseUrl"] 
            ?? throw new ArgumentNullException("ApiSettings:BaseUrl not found in configuration");
            
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public async Task<List<Affirmation>> GetAffirmationsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/affirmations");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Affirmation>>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching affirmations: {ex.Message}");
            throw;
        }
    }
}

// Add this class to handle SSL bypass
public class HttpsClientHandler : HttpClientHandler
{
    public HttpsClientHandler() : base()
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    }
} 