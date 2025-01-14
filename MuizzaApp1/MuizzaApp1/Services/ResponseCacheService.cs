using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using System.Text.Json;
using MuizzaApp1.Models;

namespace MuizzaApp1.Services
{
    public class ResponseCacheService
    {
        private const int CacheExpirationDays = 90;
        private readonly IPreferences _preferences;
        private readonly ILogger<ResponseCacheService> _logger;

        private readonly Dictionary<string, EmotionalResponse> _commonResponses = new()
        {
            { "anxious", new EmotionalResponse(
                "I am stronger than my anxiety",
                "Hey, when you're anxious, your brain releases cortisol - it's totally normal! Your amygdala (your brain's alarm system) is just trying to protect you by making you extra alert.\n\n• Take three deep breaths - this activates your parasympathetic system\n\n• Ground yourself by naming 5 things you can see\n\n• Remember this feeling will pass - your brain is just being overprotective")
            },
            { "sad", new EmotionalResponse(
                "I have the inner strength to overcome this sadness",
                "Your brain's just a bit low on feel-good chemicals right now, which is why things might feel heavier than usual. This happens when your brain's emotional centers are processing difficult feelings.\n\n• Do one small activity you usually enjoy\n\n• Try gentle movement to boost your natural mood chemicals\n\n• Connect with someone you trust - social connection releases oxytocin")
            },
            { "stressed", new EmotionalResponse(
                "I can manage this stress and find my balance",
                "When you're stressed, your brain's in 'fight or flight' mode, making everything feel overwhelming. Your body's stress response is trying to give you energy to handle challenges.\n\n• Break big tasks into smaller, manageable steps\n\n• Take a 2-minute break to stretch or move around\n\n• Focus on just one thing at a time - this helps your brain feel more in control")
            },
            // Add more common emotions here
        };

        public ResponseCacheService(ILogger<ResponseCacheService> logger)
        {
            _preferences = Preferences.Default;
            _logger = logger;
        }

        public async Task<(string affirmation, string explanation)?> GetCachedResponse(string feeling)
        {
            var normalizedFeeling = NormalizeFeeling(feeling);

            // 1. Check pre-populated responses
            if (_commonResponses.TryGetValue(normalizedFeeling, out var commonResponse))
            {
                _logger.LogInformation("Found pre-populated response for: {feeling}", feeling);
                return (commonResponse.Affirmation, commonResponse.Explanation);
            }

            // 2. Check for similar pre-populated feeling
            var similarCommonFeeling = FindSimilarFeeling(normalizedFeeling);
            if (similarCommonFeeling != null)
            {
                _logger.LogInformation("Found similar pre-populated response for: {feeling}", feeling);
                return (_commonResponses[similarCommonFeeling].Affirmation, 
                       _commonResponses[similarCommonFeeling].Explanation);
            }

            // 3. Check user cache
            return await GetUserCachedResponse(normalizedFeeling);
        }

        private async Task<(string affirmation, string explanation)?> GetUserCachedResponse(string feeling)
        {
            string key = GetCacheKey(feeling);
            string json = _preferences.Get(key, string.Empty);

            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                var cached = JsonSerializer.Deserialize<EmotionalResponse>(json);

                // Check if cache is expired
                if (DateTime.UtcNow - cached.CachedAt > TimeSpan.FromDays(CacheExpirationDays))
                {
                    _preferences.Remove(key);
                    return null;
                }

                return (cached.Affirmation, cached.Explanation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing cached response");
                return null;
            }
        }

        public async Task CacheResponse(string feeling, string affirmation, string explanation)
        {
            try
            {
                string key = GetCacheKey(NormalizeFeeling(feeling));
                var response = new EmotionalResponse(affirmation, explanation);
                string json = JsonSerializer.Serialize(response);
                _preferences.Set(key, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error caching response");
            }
        }

        private string NormalizeFeeling(string feeling)
        {
            return feeling.ToLowerInvariant()
                         .Trim()
                         .Replace("i'm feeling ", "")
                         .Replace("i am feeling ", "")
                         .Replace("i feel ", "");
        }

        private string FindSimilarFeeling(string feeling)
        {
            return _commonResponses.Keys
                .FirstOrDefault(k => k.Contains(feeling) || feeling.Contains(k));
        }

        private string GetCacheKey(string feeling) => $"emotion_response_{feeling}";

        public void ClearAllCache()
        {
            try
            {
                _preferences.Clear();
                _logger.LogInformation("Cache cleared successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cache");
            }
        }
    }
} 