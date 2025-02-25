using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using MuizzaApp1.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Windows.Input;

namespace MuizzaApp1.ViewModels
{
    public partial class EmotionalResponseViewModel : BaseViewModel
    {
        private readonly IOpenAIService _openAIService;
        private readonly ResponseCacheService _cacheService;
        private readonly ILogger<EmotionalResponseViewModel> _logger;
        private bool _isLoading;
        private string _errorMessage;
        private string _affirmation;
        private string _explanation;
        private int _remainingSearches;
        private string _currentFeeling;

        [ObservableProperty]
        private bool hasResponse;

        [ObservableProperty]
        private string currentFeeling;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public string Affirmation
        {
            get => _affirmation;
            set => SetProperty(ref _affirmation, value);
        }

        public string Explanation
        {
            get => _explanation;
            set => SetProperty(ref _explanation, value);
        }

        public int RemainingSearches
        {
            get => _remainingSearches;
            set => SetProperty(ref _remainingSearches, value);
        }

        public ICommand DeepDiveCommand { get; }

        public EmotionalResponseViewModel(
            IOpenAIService openAIService, 
            ResponseCacheService cacheService, 
            ILogger<EmotionalResponseViewModel> logger,
            ISubscriptionService subscriptionService) : base(subscriptionService)
        {
            _openAIService = openAIService;
            _cacheService = cacheService;
            _logger = logger;
            
            DeepDiveCommand = new AsyncRelayCommand(NavigateToDeepDive);
            HasResponse = false;
        }

        private async Task NavigateToDeepDive()
        {
            if (string.IsNullOrEmpty(_currentFeeling)) return;

            await Shell.Current.GoToAsync($"AdvisorSelectionPage?feeling={Uri.EscapeDataString(_currentFeeling)}");
        }

        public async Task GenerateResponse(string feeling)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                _currentFeeling = feeling;  // Store the current feeling
                HasResponse = false;

                if (!await _subscriptionService.CanMakeSearch())
                {
                    var isPremium = await _subscriptionService.IsPremiumUser();
                    ErrorMessage = isPremium 
                        ? "You've reached your daily limit of 20 searches. Please try again tomorrow."
                        : "You've reached your daily free search. Upgrade to Premium for 20 searches per day!";
                    
                    if (!isPremium)
                    {
                        await MainThread.InvokeOnMainThreadAsync(async () =>
                        {
                            await Shell.Current.GoToAsync("PremiumOnboard");
                        });
                    }
                    return;
                }

                var (affirmation, explanation) = await _openAIService.GenerateResponse(feeling);
                
                if (!string.IsNullOrEmpty(affirmation) && !string.IsNullOrEmpty(explanation))
                {
                    await _subscriptionService.IncrementSearchCount();
                    RemainingSearches = await _subscriptionService.GetRemainingSearches();
                    
                    Affirmation = affirmation;
                    Explanation = explanation;
                    HasResponse = true;  // Enable the Deep Dive button
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
                RateLimitExceededException => "You've reached your daily limit. Try again tomorrow or upgrade to Premium for more searches!",
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