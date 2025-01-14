using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MuizzaApp1.Models;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace MuizzaApp1.Services
{
    public class NotesService
    {
        private readonly HttpClient _httpClient;

        public NotesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            var baseAddress = DeviceInfo.Platform == DevicePlatform.Android 
                ? "https://10.0.2.2:7273/"
                : "https://localhost:7273/";
            _httpClient = new HttpClient(new HttpsClientHandler())
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<bool> SaveNoteAsync(string content)
        {
            try
            {
                var noteContent = new { Content = content };
                Console.WriteLine($"Attempting to save note to: {_httpClient.BaseAddress}api/Notes");
                
                var response = await _httpClient.PostAsJsonAsync("api/Notes", noteContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response status: {response.StatusCode}");
                Console.WriteLine($"Response content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                Console.WriteLine($"Exception message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
    }
}