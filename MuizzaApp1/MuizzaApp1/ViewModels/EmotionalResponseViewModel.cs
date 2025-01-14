using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using MuizzaApp1.Services;
using Microsoft.Extensions.Logging;

namespace MuizzaApp1.ViewModels
{
    public class EmotionalResponseViewModel : INotifyPropertyChanged
    {
        private readonly IOpenAIService _openAIService;
        private readonly ResponseCacheService _cacheService;
        private readonly ILogger<EmotionalResponseViewModel> _logger;
        private bool _isLoading;
        private string _errorMessage;
        private string _affirmation;
        private string _explanation;
        private int _remainingSearches;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public string Affirmation
        {
            get => _affirmation;
            set
            {
                _affirmation = value;
                OnPropertyChanged();
            }
        }

        public string Explanation
        {
            get => _explanation;
            set
            {
                _explanation = value;
                OnPropertyChanged();
            }
        }

        public int RemainingSearches
        {
            get => _remainingSearches;
            set
            {
                _remainingSearches = value;
                OnPropertyChanged();
            }
        }

        public EmotionalResponseViewModel(IOpenAIService openAIService, ResponseCacheService cacheService, ILogger<EmotionalResponseViewModel> logger)
        {
            _openAIService = openAIService;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task GenerateResponse(string feeling)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var (affirmation, explanation) = await _openAIService.GenerateResponse(feeling);
                
                Affirmation = affirmation;
                Explanation = explanation;

                // Update remaining searches if we got a valid response
                if (!string.IsNullOrEmpty(affirmation) && !string.IsNullOrEmpty(explanation))
                {
                    RemainingSearches = await _openAIService.GetRemainingSearches();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating response");
                ErrorMessage = GetUserFriendlyError(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private string GetUserFriendlyError(Exception ex)
        {
            return ex switch
            {
                RateLimitExceededException => "You've reached your daily limit. Try again tomorrow or use simpler feelings that might be in our quick-response list!",
                HttpRequestException => "Unable to connect. Please check your internet connection.",
                _ => "Something went wrong. Please try again later."
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 