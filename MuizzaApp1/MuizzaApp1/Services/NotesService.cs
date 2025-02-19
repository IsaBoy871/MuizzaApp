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

        public NotesService(IConfiguration configuration)
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
            
            Console.WriteLine($"Platform: {DeviceInfo.Platform}");
            Console.WriteLine($"Base Address: {_httpClient.BaseAddress}");
        }

        public async Task<bool> SaveNoteAsync(string content)
        {
            try
            {
                var noteContent = new { Content = content };
                Console.WriteLine($"Attempting to save note to: {_httpClient.BaseAddress}api/Notes");
                
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                using var request = new HttpRequestMessage(HttpMethod.Post, "api/Notes")
                {
                    Content = JsonContent.Create(noteContent)
                };

                Console.WriteLine("Sending request...");
                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine($"Response status: {response.StatusCode}");
                Console.WriteLine($"Response content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                Console.WriteLine($"Exception message: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                return false;
            }
        }

        public async Task<List<Note>> GetNotesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Notes");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Note>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting notes: {ex.Message}");
                return new List<Note>();
            }
        }

        public async Task<bool> UpdateNoteAsync(Note note)
        {
            try
            {
                Console.WriteLine($"Attempting to update note with ID: {note.Id}");
                
                var response = await _httpClient.PutAsJsonAsync($"api/Notes/{note.Id}", note);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine($"Response status: {response.StatusCode}");
                Console.WriteLine($"Response content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                Console.WriteLine($"Exception message: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                return false;
            }
        }
    }
}