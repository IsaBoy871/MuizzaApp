using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MuizzaApp1.Models;

public class AffirmationsService
{
    private readonly HttpClient _httpClient;

    public AffirmationsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var baseAddress = DeviceInfo.Platform == DevicePlatform.Android 
            ? "https://10.0.2.2:7273/"
            : "https://localhost:7273/";
        _httpClient.BaseAddress = new Uri(baseAddress);
    }

    public async Task<List<Affirmation>> GetAffirmationsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/affirmations");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Affirmation>>() ?? new List<Affirmation>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching affirmations: {ex.Message}");
            return new List<Affirmation>();
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