using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using MuizzaApp1.Models;
using System.Text.Json;
using MuizzaApp1.Exceptions;

namespace MuizzaApp1.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<OpenAIService> _logger;
        private readonly ResponseCacheService _cacheService;
        private const string MODEL = "gpt-3.5-turbo";
        private const int MAX_TOKENS = 150;
        private const int DailyFreeLimit = 50;

        // Track API usage
        private readonly Dictionary<string, int> _dailyUsage = new();
        private readonly SemaphoreSlim _throttler = new(1);
        private DateTime _lastRequestTime = DateTime.MinValue;
        private const int MinRequestIntervalMs = 1000; // 1 second between requests

        public OpenAIService(
            IConfiguration configuration,
            ILogger<OpenAIService> logger,
            ResponseCacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];

            if (string.IsNullOrEmpty(_apiKey))
            {
                _logger.LogError("OpenAI API key is missing from configuration");
                throw new InvalidOperationException("OpenAI API key is not configured");
            }

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<(string affirmation, string explanation)> GenerateResponse(string feeling)
        {
            try
            {
                // 1. Try cache first
                var cached = await _cacheService.GetCachedResponse(feeling);
                if (cached.HasValue)
                {
                    _logger.LogInformation("Cache hit for feeling: {feeling}", feeling);
                    return cached.Value;
                }

                // 2. Check and update daily usage
                if (!await CheckDailyLimit())
                {
                    _logger.LogWarning("Daily API limit reached");
                    return GetFallbackResponse();
                }

                // 3. Apply rate limiting
                await ApplyRateLimit();

                // 4. Make API call
                var response = await MakeApiCall(feeling);

                // 5. Cache the response
                await _cacheService.CacheResponse(feeling, response.affirmation, response.explanation);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating response for feeling: {feeling}", feeling);
                return GetFallbackResponse();
            }
        }

        private async Task<bool> CheckDailyLimit()
        {
            var today = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
            
            await _throttler.WaitAsync();
            try
            {
                if (!_dailyUsage.ContainsKey(today))
                {
                    _dailyUsage.Clear(); // Reset old dates
                    _dailyUsage[today] = 0;
                }

                if (_dailyUsage[today] >= DailyFreeLimit)
                    return false;

                _dailyUsage[today]++;
                return true;
            }
            finally
            {
                _throttler.Release();
            }
        }

        private async Task ApplyRateLimit()
        {
            await _throttler.WaitAsync();
            try
            {
                var timeSinceLastRequest = DateTime.UtcNow - _lastRequestTime;
                if (timeSinceLastRequest.TotalMilliseconds < MinRequestIntervalMs)
                {
                    await Task.Delay(MinRequestIntervalMs - (int)timeSinceLastRequest.TotalMilliseconds);
                }
                _lastRequestTime = DateTime.UtcNow;
            }
            finally
            {
                _throttler.Release();
            }
        }

        private async Task<(string affirmation, string explanation)> MakeApiCall(string feeling)
        {
            var request = new
            {
                model = MODEL,
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = @"You are a friendly AI counselor who explains brain science in simple terms. 

                        1. AFFIRMATION: One empowering 'I' statement (10-15 words)
                        2. EXPLANATION: Two parts:
                           - First paragraph: 2-3 sentences (60-80 words) explaining what's happening 
                             in the brain using everyday language. Avoid technical terms - explain 
                             like you're talking to a friend.
                           - Then 3 bullet points (10-12 words each) with clear, friendly advice.
                             IMPORTANT: Each bullet point MUST be separated by TWO newlines.

                        Keep tone warm and conversational throughout.

                        Format exactly like this:
                        AFFIRMATION: I [empowering statement]
                        EXPLANATION: [Friendly explanation about what's happening in your brain]

                        • [Friendly advice with gentle encouragement]


                        • [Friendly advice with gentle encouragement]


                        • [Friendly advice with gentle encouragement]

                        Example:
                        EXPLANATION: Hey, when you're feeling this way, your brain is like a protective friend who's being a bit too careful. It's releasing chemicals that make you feel alert and on edge, kind of like an overactive alarm system. This is your mind trying to keep you safe, even though it might not feel great right now.

                        • Take three deep breaths and remind yourself you're safe


                        • Step outside for a moment to give your mind space


                        • Write down one small thing you can do right now"
                    },
                    new
                    {
                        role = "user",
                        content = $"I'm feeling: {feeling}"
                    }
                },
                max_tokens = MAX_TOKENS,
                temperature = 0.7
            };

            var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
            var content = result.choices[0].message.content;

            var affirmation = content.Split("AFFIRMATION:")[1].Split("EXPLANATION:")[0].Trim();
            var explanation = content.Split("EXPLANATION:")[1].Trim();

            return (affirmation, explanation);
        }

        private (string affirmation, string explanation) GetFallbackResponse()
        {
            return ("I acknowledge how I'm feeling",
                   "Hey, we're experiencing high demand right now. Try expressing how you feel in simpler terms, or check back in a little while. Remember, your feelings are valid and it's okay to take time to process them.");
        }

        public async Task<int> GetRemainingSearches()
        {
            var today = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
            
            await _throttler.WaitAsync();
            try
            {
                if (!_dailyUsage.ContainsKey(today))
                {
                    return DailyFreeLimit;
                }

                return Math.Max(0, DailyFreeLimit - _dailyUsage[today]);
            }
            finally
            {
                _throttler.Release();
            }
        }
    }
}

// Custom exceptions
public class OpenAIException : Exception
{
    public OpenAIException(string message) : base(message) { }
}

public class RateLimitExceededException : OpenAIException
{
    public RateLimitExceededException(string message) : base(message) { }
}

public class UnauthorizedException : OpenAIException
{
    public UnauthorizedException(string message) : base(message) { }
} 