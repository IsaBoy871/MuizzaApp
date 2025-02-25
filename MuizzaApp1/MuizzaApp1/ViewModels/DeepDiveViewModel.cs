using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using MuizzaApp1.Services;

namespace MuizzaApp1.ViewModels
{
    public partial class DeepDiveViewModel : BaseViewModel
    {
        private readonly IOpenAIService _openAIService;
        private readonly ILogger<DeepDiveViewModel> _logger;
        private readonly string _feeling;
        private readonly string _advisorSystemPrompt;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string detailedResponse;

        [ObservableProperty]
        private string feeling;

        public DeepDiveViewModel(
            IOpenAIService openAIService,
            ILogger<DeepDiveViewModel> logger,
            ISubscriptionService subscriptionService,
            string feeling,
            string advisorSystemPrompt) : base(subscriptionService)
        {
            _openAIService = openAIService;
            _logger = logger;
            _feeling = feeling;
            _advisorSystemPrompt = advisorSystemPrompt;
            Feeling = feeling;
        }

        public async Task InitializeAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("InitializeAsync started");
                IsLoading = true;
                System.Diagnostics.Debug.WriteLine("Calling OpenAI service");
                DetailedResponse = await _openAIService.GenerateDeepDiveResponse(_feeling, _advisorSystemPrompt);
                System.Diagnostics.Debug.WriteLine($"Response received: {DetailedResponse?.Substring(0, Math.Min(100, DetailedResponse?.Length ?? 0))}...");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InitializeAsync: {ex}");
                _logger.LogError(ex, "Error generating deep dive response");
                DetailedResponse = "We're having trouble generating a detailed response right now. Please try again later.";
            }
            finally
            {
                IsLoading = false;
                System.Diagnostics.Debug.WriteLine("InitializeAsync completed");
            }
        }
    }
} 