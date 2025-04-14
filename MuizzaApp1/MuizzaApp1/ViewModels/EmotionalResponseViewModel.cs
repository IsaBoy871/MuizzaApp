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
using Microsoft.Maui.Storage;
using System.Diagnostics;
#if IOS
using PassKit;
using Foundation;
#endif

namespace MuizzaApp1.ViewModels
{
    public partial class EmotionalResponseViewModel : BaseViewModel
    {
        private readonly IOpenAIService _openAIService;
        private readonly ResponseCacheService _cacheService;
        private readonly ILogger<EmotionalResponseViewModel> _logger;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private string _currentFeeling;

        private readonly Dictionary<string, (decimal amount, string description, int searches, int deepDives)> _packPrices = new()
        {
            { "starter", (0.80m, "Starter Pack - 10 CatNips", 10, 4) },
            { "popular", (4.99m, "Popular Pack - 50 CatNips", 50, 20) },
            { "premium", (40.00m, "Premium Pack - 1000 CatNips", 1000, 400) }
        };

        [ObservableProperty]
        private bool isNotPremium;

        [ObservableProperty]
        private int searchBalance;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private string affirmation;

        [ObservableProperty]
        private string explanation;

        [ObservableProperty]
        private bool hasResponse;

        [ObservableProperty]
        private string currentFeeling;

        [ObservableProperty]
        private int remainingSearches;

        public ICommand DeepDiveCommand { get; }
        public ICommand SubscribeToPremiumCommand { get; }
        public ICommand InitiateApplePayCommand { get; }

        public EmotionalResponseViewModel(
            IOpenAIService openAIService, 
            ResponseCacheService cacheService, 
            ILogger<EmotionalResponseViewModel> logger,
            ISubscriptionService subscriptionService,
            IPaymentService paymentService,
            IUserService userService) : base(subscriptionService)
        {
            _openAIService = openAIService;
            _cacheService = cacheService;
            _logger = logger;
            _paymentService = paymentService;
            _userService = userService;
            
            LoadSearchBalanceAsync().ConfigureAwait(false);
            DeepDiveCommand = new AsyncRelayCommand(NavigateToDeepDive);
            SubscribeToPremiumCommand = new AsyncRelayCommand(NavigateToPremium);
            InitiateApplePayCommand = new AsyncRelayCommand<string>(InitiateApplePayAsync);
            HasResponse = false;
            CheckPremiumStatus();
        }

        private async Task NavigateToPremium()
        {
            await Shell.Current.GoToAsync("///PremiumOnboard");
        }

        private async Task CheckPremiumStatus()
        {
            IsNotPremium = !await _subscriptionService.IsPremiumUser();
        }

        private async Task LoadSearchBalanceAsync()
        {
            try
            {
                var balance = await _userService.GetSearchBalanceAsync();
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    SearchBalance = balance;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load search balance");
                // Fallback to local preferences if API call fails
                SearchBalance = Preferences.Get("SearchBalance", 0);
            }
        }

        public async Task UpdateSearchBalance()
        {
            await LoadSearchBalanceAsync();
        }

        private async Task NavigateToDeepDive()
        {
            if (string.IsNullOrEmpty(_currentFeeling)) return;

            await Shell.Current.GoToAsync($"AdvisorSelectionPage?feeling={Uri.EscapeDataString(_currentFeeling)}");
        }

        private async Task InitiateApplePayAsync(string packType)
        {
            Debug.WriteLine($"InitiateApplePayAsync called with packType: {packType}");
#if IOS
            try
            {
                _logger.LogInformation($"Starting Apple Pay process for pack: {packType}");
                IsLoading = true;

                if (!_packPrices.ContainsKey(packType))
                {
                    Debug.WriteLine($"Invalid pack type: {packType}");
                    await Shell.Current.DisplayAlert("Error", "Invalid pack selected", "OK");
                    return;
                }

                var (amount, description, searches, deepDives) = _packPrices[packType];
                Debug.WriteLine($"Pack details - Amount: {amount}, Description: {description}, Searches: {searches}, Deep Dives: {deepDives}");

                // Check if Apple Pay is available
                if (!PKPaymentAuthorizationController.CanMakePayments)
                {
                    Debug.WriteLine("Apple Pay is not available on this device");
                    await Shell.Current.DisplayAlert("Error", "Apple Pay is not available on this device", "OK");
                    return;
                }

                Debug.WriteLine("Creating payment request");
                // Create payment request
                var paymentRequest = new PKPaymentRequest
                {
                    MerchantIdentifier = "merchant.com.isaadeel.muizzaapp",
                    CountryCode = "GB",
                    CurrencyCode = "GBP",
                    SupportedNetworks = new[] { PKPaymentNetwork.Visa, PKPaymentNetwork.MasterCard },
                    MerchantCapabilities = PKMerchantCapability.ThreeDS,
                    PaymentSummaryItems = new[]
                    {
                        PKPaymentSummaryItem.Create(description, new NSDecimalNumber(amount.ToString("F2")))
                    }
                };

                // Create and present payment controller
                var paymentController = new PKPaymentAuthorizationController(paymentRequest);
                if (paymentController == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Unable to create payment controller", "OK");
                    return;
                }

                // Handle payment authorization
                var tcs = new TaskCompletionSource<bool>();
                paymentController.Delegate = new CustomPaymentAuthorizationDelegate(tcs, ProcessSuccessfulPayment, packType);

                await paymentController.PresentAsync();
                var result = await tcs.Task;

                if (!result)
                {
                    await Shell.Current.DisplayAlert("Payment Failed", "The payment was not completed.", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Apple Pay payment");
                await Shell.Current.DisplayAlert("Error", "Something went wrong with the payment. Please try again.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
#else
            await Shell.Current.DisplayAlert("Not Available", "Apple Pay is only available on iOS devices.", "OK");
#endif
        }

        private async Task ProcessSuccessfulPayment(string packType)
        {
            if (_packPrices.TryGetValue(packType, out var packDetails))
            {
                var (_, _, searches, deepDives) = packDetails;
                
                try {
                    // Update balances through API
                    await _userService.AddSearchBalanceAsync(searches);
                    await _userService.AddDeepDiveBalanceAsync(deepDives);
                    
                    // Refresh the UI
                    await LoadSearchBalanceAsync();
                    
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Shell.Current.DisplayAlert(
                            "Success!",
                            $"Added {searches} searches and {deepDives} deep dives to your balance!",
                            "OK"
                        );
                        
                        // Navigate back to QuotesPage
                        await Shell.Current.GoToAsync("///QuotesPage");
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update balances after payment");
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Shell.Current.DisplayAlert("Error", "Payment processed but failed to update balances. Please contact support.", "OK");
                    });
                }
            }
        }

#if IOS
        private class CustomPaymentAuthorizationDelegate : PKPaymentAuthorizationControllerDelegate
        {
            private readonly TaskCompletionSource<bool> _tcs;
            private readonly Func<string, Task> _processPaymentFunc;
            private readonly string _packType;

            public CustomPaymentAuthorizationDelegate(TaskCompletionSource<bool> tcs, Func<string, Task> processPaymentFunc, string packType)
            {
                _tcs = tcs;
                _processPaymentFunc = processPaymentFunc;
                _packType = packType;
            }

            public override async void DidAuthorizePayment(PKPaymentAuthorizationController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
            {
                try
                {
                    await _processPaymentFunc(_packType);
                    completion(PKPaymentAuthorizationStatus.Success);
                    _tcs.SetResult(true);
                }
                catch (Exception)
                {
                    completion(PKPaymentAuthorizationStatus.Failure);
                    _tcs.SetResult(false);
                }
            }

            public override void DidFinish(PKPaymentAuthorizationController controller)
            {
                controller.DismissAsync();
                if (!_tcs.Task.IsCompleted)
                {
                    _tcs.SetResult(false);
                }
            }
        }
#endif

        public async Task GenerateResponse(string feeling)
        {
            try
            {
                Debug.WriteLine($"Starting GenerateResponse for feeling: {feeling}");
                IsLoading = true;
                ErrorMessage = null;
                _currentFeeling = feeling;  // Store the current feeling
                HasResponse = false;

                // Get real-time balance from API
                await LoadSearchBalanceAsync();
                Debug.WriteLine($"Current search balance: {SearchBalance}");

                // Check if user has enough CatNips
                if (SearchBalance <= 0)
                {
                    Debug.WriteLine("No search balance available");
                    ErrorMessage = "You don't have enough Search CatNips. Please purchase more!";
                    return;
                }

                if (!await _subscriptionService.CanMakeSearch())
                {
                    var isPremium = await _subscriptionService.IsPremiumUser();
                    Debug.WriteLine($"Cannot make search. IsPremium: {isPremium}");
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

                Debug.WriteLine("Generating response from OpenAI");
                var (affirmation, explanation) = await _openAIService.GenerateResponse(feeling);
                Debug.WriteLine($"Received response - Affirmation: {!string.IsNullOrEmpty(affirmation)}, Explanation: {!string.IsNullOrEmpty(explanation)}");
                
                if (!string.IsNullOrEmpty(affirmation) && !string.IsNullOrEmpty(explanation))
                {
                    // Decrement SearchBalance through API
                    Debug.WriteLine("Decrementing search balance");
                    await _userService.DecrementSearchBalanceAsync();
                    await LoadSearchBalanceAsync(); // Refresh the balance
                    
                    await _subscriptionService.IncrementSearchCount();
                    RemainingSearches = await _subscriptionService.GetRemainingSearches();
                    
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Affirmation = affirmation;
                        Explanation = explanation;
                        HasResponse = true;  // Enable the Deep Dive button
                        Debug.WriteLine($"UI updated - HasResponse: {HasResponse}, Affirmation length: {affirmation?.Length}, Explanation length: {explanation?.Length}");
                        OnPropertyChanged(nameof(HasResponse));
                        OnPropertyChanged(nameof(Affirmation));
                        OnPropertyChanged(nameof(Explanation));
                    });
                }
                else
                {
                    Debug.WriteLine("Response was empty");
                    ErrorMessage = "Unable to generate a response. Please try again.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GenerateResponse: {ex}");
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