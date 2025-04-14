using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MuizzaApp1.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using MuizzaApp1.Contracts.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(string appleUserId);
    Task<User> GetUserByAppleIdAsync(string appleUserId);
    Task<User> UpdateUserNameAsync(string appleUserId, string name);
    Task UpdateSubscriptionTierAsync(string appleUserId, string subscriptionTier);
    Task<User> GetCurrentUserAsync();
    Task UpdateTrialStatusAsync(string appleUserId, bool hasTrial);
    Task<int> GetSearchBalanceAsync();
    Task<int> GetDeepDiveBalanceAsync();
    Task<int> DecrementSearchBalanceAsync();
    Task<int> DecrementDeepDiveBalanceAsync();
    Task<int> AddSearchBalanceAsync(int amount);
    Task<int> AddDeepDiveBalanceAsync(int amount);
    Task UpdateBalancesAsync(int searchBalance, int deepDiveBalance);
}

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public UserService(IConfiguration configuration)
    {
        var baseUrl = configuration["ApiSettings:BaseUrl"] 
            ?? throw new ArgumentNullException("ApiSettings:BaseUrl not found in configuration");
            
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
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
            var response = await _httpClient.PostAsJsonAsync("api/users", user, _jsonOptions);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to create user. Status: {response.StatusCode}, Error: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<User>(_jsonOptions);
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
            var updateModel = new UserUpdateRequest { Name = name };
            var response = await _httpClient.PutAsJsonAsync($"api/users/apple/{appleUserId}/name", updateModel);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to update user name. Status: {response.StatusCode}, Error: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<User>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating user name: {ex.Message}", ex);
        }
    }

    public async Task<User> GetCurrentUserAsync()
    {
        try
        {
            var appleUserId = Preferences.Get("AppleUserId", string.Empty);
            if (string.IsNullOrEmpty(appleUserId))
            {
                throw new InvalidOperationException("No user is currently signed in");
            }

            var user = await GetUserByAppleIdAsync(appleUserId);
            if (user == null)
            {
                // If user doesn't exist, create them
                user = await CreateUserAsync(appleUserId);
            }

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting current user: {ex.Message}", ex);
        }
    }

    public async Task UpdateSubscriptionTierAsync(string appleUserId, string subscriptionTier)
    {
        try
        {
            // First verify the user exists
            var user = await GetUserByAppleIdAsync(appleUserId);
            if (user == null)
            {
                throw new InvalidOperationException($"User with Apple ID {appleUserId} not found");
            }

            // Send the subscription tier as a plain string
            var content = new StringContent($"\"{subscriptionTier}\"", System.Text.Encoding.UTF8, "application/json");

            Debug.WriteLine($"Sending payload: {await content.ReadAsStringAsync()}");
            Debug.WriteLine($"User ID: {user.Id}");
            Debug.WriteLine($"Full URL: {_httpClient.BaseAddress}api/Users/{user.Id}/subscription");

            // Log request headers
            Debug.WriteLine("Request Headers:");
            foreach (var header in content.Headers)
            {
                Debug.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            var response = await _httpClient.PutAsync($"api/Users/{user.Id}/subscription", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response: {errorContent}");
                throw new HttpRequestException($"Failed to update subscription. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in UpdateSubscriptionTierAsync: {ex}");
            throw new Exception($"Error updating subscription tier: {ex.Message}", ex);
        }
    }

    public async Task UpdateTrialStatusAsync(string appleUserId, bool hasTrial)
    {
        try
        {
            var user = await GetUserByAppleIdAsync(appleUserId);
            if (user != null)
            {
                user.HasTrial = hasTrial;
                // Save changes to your data store
            }
        }
        catch (Exception ex)
        {
            // Log error
            throw;
        }
    }

    public async Task<int> GetSearchBalanceAsync()
    {
        try
        {
            var user = await GetCurrentUserAsync();
            if (user == null) throw new InvalidOperationException("No user found");

            Debug.WriteLine($"Getting search balance for user ID: {user.Id}");
            var response = await _httpClient.GetAsync($"api/users/apple/{user.AppleUserId}/search-balance");
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response from API: {error}");
                throw new HttpRequestException($"Failed to get search balance. Status: {response.StatusCode}, Error: {error}");
            }

            var balance = await response.Content.ReadFromJsonAsync<int>();
            Debug.WriteLine($"Retrieved search balance from API: {balance}");
            return balance;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting search balance: {ex.Message}");
            return 0; // Return 0 instead of throwing to prevent app crashes
        }
    }

    public async Task<int> GetDeepDiveBalanceAsync()
    {
        try
        {
            var user = await GetCurrentUserAsync();
            if (user == null) throw new InvalidOperationException("No user found");

            Debug.WriteLine($"Getting deep dive balance for user ID: {user.Id}");
            var response = await _httpClient.GetAsync($"api/users/apple/{user.AppleUserId}/deep-dive-balance");
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response from API: {error}");
                throw new HttpRequestException($"Failed to get deep dive balance. Status: {response.StatusCode}, Error: {error}");
            }

            var balance = await response.Content.ReadFromJsonAsync<int>();
            Debug.WriteLine($"Retrieved deep dive balance from API: {balance}");
            return balance;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting deep dive balance: {ex.Message}");
            return 0; // Return 0 instead of throwing to prevent app crashes
        }
    }

    public async Task<int> DecrementSearchBalanceAsync()
    {
        try
        {
            var user = await GetCurrentUserAsync();
            if (user == null) throw new InvalidOperationException("No user found");

            Debug.WriteLine($"Decrementing search balance for user ID: {user.Id}");
            var response = await _httpClient.PostAsync($"api/users/apple/{user.AppleUserId}/search-balance/decrement", null);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response from API: {error}");
                throw new HttpRequestException($"Failed to decrement search balance. Status: {response.StatusCode}, Error: {error}");
            }

            var balance = await response.Content.ReadFromJsonAsync<int>();
            Debug.WriteLine($"New search balance after decrement: {balance}");
            return balance;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error decrementing search balance: {ex.Message}");
            throw;
        }
    }

    public async Task<int> DecrementDeepDiveBalanceAsync()
    {
        try
        {
            var user = await GetCurrentUserAsync();
            if (user == null) throw new InvalidOperationException("No user found");

            Debug.WriteLine($"Decrementing deep dive balance for user ID: {user.Id}");
            var response = await _httpClient.PostAsync($"api/users/apple/{user.AppleUserId}/deep-dive-balance/decrement", null);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response from API: {error}");
                throw new HttpRequestException($"Failed to decrement deep dive balance. Status: {response.StatusCode}, Error: {error}");
            }

            var balance = await response.Content.ReadFromJsonAsync<int>();
            Debug.WriteLine($"New deep dive balance after decrement: {balance}");
            return balance;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error decrementing deep dive balance: {ex.Message}");
            throw;
        }
    }

    public async Task<int> AddSearchBalanceAsync(int amount)
    {
        try
        {
            var user = await GetCurrentUserAsync();
            if (user == null) throw new InvalidOperationException("No user found");

            Debug.WriteLine($"Adding {amount} to search balance for user ID: {user.Id}");
            
            // Create JSON content with the amount as a raw value
            var content = new StringContent(amount.ToString(), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/users/apple/{user.AppleUserId}/search-balance/add", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response from API: {error}");
                throw new HttpRequestException($"Failed to add search balance. Status: {response.StatusCode}, Error: {error}");
            }

            var balance = await response.Content.ReadFromJsonAsync<int>(_jsonOptions);
            Debug.WriteLine($"New search balance after adding: {balance}");
            return balance;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error adding to search balance: {ex.Message}");
            throw;
        }
    }

    public async Task<int> AddDeepDiveBalanceAsync(int amount)
    {
        try
        {
            var user = await GetCurrentUserAsync();
            if (user == null) throw new InvalidOperationException("No user found");

            Debug.WriteLine($"Adding {amount} to deep dive balance for user ID: {user.Id}");
            
            // Create JSON content with the amount as a raw value
            var content = new StringContent(amount.ToString(), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/users/apple/{user.AppleUserId}/deep-dive-balance/add", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response from API: {error}");
                throw new HttpRequestException($"Failed to add deep dive balance. Status: {response.StatusCode}, Error: {error}");
            }

            var balance = await response.Content.ReadFromJsonAsync<int>(_jsonOptions);
            Debug.WriteLine($"New deep dive balance after adding: {balance}");
            return balance;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error adding to deep dive balance: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateBalancesAsync(int searchBalance, int deepDiveBalance)
    {
        try
        {
            var user = await GetCurrentUserAsync();
            if (user == null) throw new InvalidOperationException("No user found");

            Debug.WriteLine($"Updating balances for user ID: {user.Id} - Search: {searchBalance}, Deep Dive: {deepDiveBalance}");
            var content = JsonContent.Create(new { searchBalance, deepDiveBalance });
            var response = await _httpClient.PutAsync($"api/users/apple/{user.AppleUserId}/balances", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response from API: {error}");
                throw new HttpRequestException($"Failed to update balances. Status: {response.StatusCode}, Error: {error}");
            }

            Debug.WriteLine("Balances updated successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating balances: {ex.Message}");
            throw;
        }
    }
}

public class UserUpdateRequest
{
    public string Name { get; set; }
}

public class SubscriptionUpdateRequest
{
    [JsonPropertyName("subscriptionTier")]
    public string SubscriptionTier { get; set; }
}