using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public interface IUserService
{
    Task<User> CreateUserAsync(string appleUserId);
    Task<User> GetUserByAppleIdAsync(string appleUserId);
    Task<User> UpdateUserNameAsync(string appleUserId, string name);
}

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(IConfiguration configuration)
    {
        var baseUrl = configuration["ApiSettings:BaseUrl"] 
            ?? throw new ArgumentNullException("ApiSettings:BaseUrl not found in configuration");
            
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public async Task<User> CreateUserAsync(string appleUserId)
    {
        if (string.IsNullOrEmpty(appleUserId))
        {
            throw new ArgumentException("AppleUserId cannot be null or empty");
        }

        var user = new User
        {
            AppleUserId = appleUserId,
            SubscriptionTier = "Free",
            CreatedAt = DateTime.UtcNow
        };

        try 
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", user);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to create user. Status: {response.StatusCode}, Error: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<User>();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Error creating user: {ex.Message}", ex);
        }
    }

    public async Task<User> GetUserByAppleIdAsync(string appleUserId)
    {
        var response = await _httpClient.GetAsync($"api/users/apple/{appleUserId}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
            
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>();
    }

    public async Task<User> UpdateUserNameAsync(string appleUserId, string name)
    {
        try 
        {
            var response = await _httpClient.PutAsJsonAsync($"api/users/apple/{appleUserId}/name", 
                new { name = name });
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to update user name. Status: {response.StatusCode}, Error: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<User>();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Error updating user name: {ex.Message}", ex);
        }
    }
} 