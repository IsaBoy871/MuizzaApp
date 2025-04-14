using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MuizzaApp1.Models;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Configuration;

namespace MuizzaApp1.Services
{
    public class NotesService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClientHandler _handler;
        private readonly string _baseUrl;
        private readonly ISubscriptionService _subscriptionService;

        public NotesService(IConfiguration configuration, ISubscriptionService subscriptionService)
        {
            _handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(_handler);
            _subscriptionService = subscriptionService;
            
            _baseUrl = configuration["ApiSettings:BaseUrl"] 
                ?? throw new ArgumentNullException("ApiSettings:BaseUrl not found in configuration");
            
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            Console.WriteLine($"Platform: {DeviceInfo.Platform}");
            Console.WriteLine($"Base Address: {_httpClient.BaseAddress}");
        }

        public async Task<bool> SaveNoteAsync(string content)
        {
            try
            {
                var appleUserId = Preferences.Get("AppleUserId", string.Empty);
                if (string.IsNullOrEmpty(appleUserId))
                    return false;
                
                // Check if user has reached the note limit
                var notes = await GetNotesAsync();
                var subscriptionTier = await _subscriptionService.GetSubscriptionTier();
                
                if (subscriptionTier != "Premium" && notes.Count >= 20)
                {
                    return false;
                }
                
                var noteContent = new { Content = content, UserId = appleUserId };
                
                using var request = new HttpRequestMessage(HttpMethod.Post, "api/Notes")
                {
                    Content = JsonContent.Create(noteContent)
                };
                
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving note: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Note>> GetNotesAsync()
        {
            try
            {
                var appleUserId = Preferences.Get("AppleUserId", string.Empty);
                if (string.IsNullOrEmpty(appleUserId))
                    return new List<Note>();
            
                var response = await _httpClient.GetAsync($"api/Notes?userId={appleUserId}");
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Note>>();
                }
                
                return new List<Note>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting notes: {ex.Message}");
                return new List<Note>();
            }
        }

        public async Task<bool> UpdateNoteAsync(Note note)
        {
            try
            {
                var appleUserId = Preferences.Get("AppleUserId", string.Empty);
                if (string.IsNullOrEmpty(appleUserId))
                    return false;
                    
                var noteContent = new { Content = note.Content, UserId = appleUserId };
                
                using var request = new HttpRequestMessage(HttpMethod.Put, $"api/Notes/{note.Id}")
                {
                    Content = JsonContent.Create(noteContent)
                };
                
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating note: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteNoteAsync(int noteId)
        {
            try
            {
                var appleUserId = Preferences.Get("AppleUserId", string.Empty);
                if (string.IsNullOrEmpty(appleUserId))
                    return false;
                    
                var response = await _httpClient.DeleteAsync($"api/Notes/{noteId}?userId={appleUserId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting note: {ex.Message}");
                return false;
            }
        }
    }
}