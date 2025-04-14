using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Collections.Generic;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui.Devices;

#if IOS
using PassKit;
using Foundation;
#endif

namespace MuizzaApp1.ViewModels
{
    public partial class PremiumStatusViewModel : BaseViewModel
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IUserService _userService;

        [ObservableProperty]
        private int searchBalance;

        [ObservableProperty]
        private int deepDiveBalance;

        [ObservableProperty]
        private bool isRefreshing;

        private readonly Dictionary<string, (decimal amount, string description)> _packPrices = new()
        {
            { "starter", (0.80m, "Starter Pack - 10 searches & 4 deep dives") },
            { "popular", (4.99m, "Popular Pack - 50 searches & 20 deep dives") },
            { "premium", (40.00m, "Premium Pack - 1000 searches & 400 deep dives") }
        };

        public ICommand PurchaseCommand { get; }
        public ICommand RechargeCommand { get; }
        public ICommand InitiateApplePayCommand { get; }
        public ICommand NavigateToPremiumCommand { get; }

        public bool IsNotPremiumUser => !IsPremiumUser;

        public PremiumStatusViewModel(ISubscriptionService subscriptionService, IUserService userService) : base(subscriptionService)
        {
            _subscriptionService = subscriptionService;
            _userService = userService;
            
            // Remove ConfigureAwait(false) and let OnAppearing handle initial loading
            CheckPremiumStatus();

            NavigateToPremiumCommand = new AsyncRelayCommand(async () =>
            {
                await Shell.Current.GoToAsync("PremiumOnboard");
            });

            PurchaseCommand = new AsyncRelayCommand<string>(async (amount) =>
            {
                if (int.TryParse(amount, out int searches))
                {
                    // Map searches to deep dives and price
                    (int deepDives, decimal price) = searches switch
                    {
                        10 => (4, 0.8m),
                        50 => (20, 4.99m),
                        1000 => (400, 40.00m),
                        _ => (0, 0m)
                    };

                    if (price > 0)
                    {
                        try 
                        {
                            Debug.WriteLine($"Navigating with parameters: searches={searches}, deepDives={deepDives}, price={price}");
                            await Shell.Current.GoToAsync($"PaywallPage?searches={searches}&deepDives={deepDives}&price={price}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Navigation error: {ex}");
                            await Shell.Current.DisplayAlert(
                                "Error",
                                "Unable to process purchase. Please try again.",
                                "OK");
                        }
                    }
                }
            });

            RechargeCommand = new AsyncRelayCommand(async () =>
            {
                await Shell.Current.DisplayAlert(
                    "Recharge CatNips",
                    "Choose an amount to add to your balance!",
                    "OK");
            });

            // Subscribe to navigation events
            MessagingCenter.Subscribe<PaywallViewModel>(this, "PurchaseComplete", async (sender) =>
            {
                await LoadBalancesAsync();
            });

            InitiateApplePayCommand = new AsyncRelayCommand<string>(InitiateApplePayAsync);
        }

        private async Task LoadBalancesAsync()
        {
            try
            {
                Debug.WriteLine("[0:] Starting LoadBalancesAsync");

                // Just load the current balances without resetting
                SearchBalance = await _userService.GetSearchBalanceAsync();
                DeepDiveBalance = await _userService.GetDeepDiveBalanceAsync();

                Debug.WriteLine($"[0:] Retrieved from API - Search: {SearchBalance}, Deep Dive: {DeepDiveBalance}");
                Debug.WriteLine($"[0:] Set to properties - Search: {SearchBalance}, Deep Dive: {DeepDiveBalance}");
                Debug.WriteLine($"[0:] After LoadBalancesAsync - Search: {SearchBalance}, Deep Dive: {DeepDiveBalance}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[0:] Error in LoadBalancesAsync: {ex}");
            }
        }

        [RelayCommand]
        private async Task RefreshBalancesAsync()
        {
            try
            {
                IsRefreshing = true;
                await LoadBalancesAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public async void OnAppearing()
        {
            Debug.WriteLine("PremiumStatusPage OnAppearing called");
            await CheckPremiumStatus();
            Debug.WriteLine($"After CheckPremiumStatus - IsPremiumUser: {IsPremiumUser}");
            await LoadBalancesAsync();
            Debug.WriteLine($"After LoadBalancesAsync - Search: {SearchBalance}, Deep Dive: {DeepDiveBalance}");
        }

        public void Cleanup()
        {
            MessagingCenter.Unsubscribe<PaywallViewModel>(this, "PurchaseComplete");
        }

        private async Task CheckPremiumStatus()
        {
            Debug.WriteLine("Starting CheckPremiumStatus in PremiumStatusViewModel");
            var isPremiumBefore = IsPremiumUser;
            await CheckSubscriptionStatus();
            Debug.WriteLine($"CheckPremiumStatus - Before: {isPremiumBefore}, After: {IsPremiumUser}");
            OnPropertyChanged(nameof(IsPremiumUser));
            OnPropertyChanged(nameof(IsNotPremiumUser));
        }

        private async Task InitiateApplePayAsync(string packType)
        {
#if IOS
            try
            {
                Debug.WriteLine($"Starting Apple Pay process for pack: {packType}");
                
                if (!_packPrices.ContainsKey(packType))
                {
                    await Shell.Current.DisplayAlert("Error", "Invalid pack selected", "OK");
                    return;
                }

                var (amount, description) = _packPrices[packType];

                // Check if Apple Pay is available
                if (!PKPaymentAuthorizationController.CanMakePayments)
                {
                    await Shell.Current.DisplayAlert("Error", "Apple Pay is not available on this device", "OK");
                    return;
                }

                // Get current user
                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Please sign in again to make a purchase.", "OK");
                    return;
                }

                Debug.WriteLine("Creating payment request");
                // Create payment request
                var paymentRequest = new PKPaymentRequest
                {
                    MerchantIdentifier = "merchant.com.isaadeel.muizzaapp",
                    CountryCode = "GB",
                    CurrencyCode = "GBP",
                    SupportedNetworks = new[] { PKPaymentNetwork.Visa, PKPaymentNetwork.MasterCard, PKPaymentNetwork.Amex },
                    MerchantCapabilities = PKMerchantCapability.ThreeDS | PKMerchantCapability.Credit | PKMerchantCapability.Debit,
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
                var tcs = new TaskCompletionSource<(bool success, PKPayment payment)>();
                paymentController.Delegate = new CustomPaymentAuthorizationDelegate(tcs);

                await paymentController.PresentAsync();
                var result = await tcs.Task;
                bool success = result.success;
                PKPayment payment = result.payment;

                if (success && payment != null)
                {
                    try
                    {
                        await ProcessSuccessfulPayment(packType);
                        await Shell.Current.DisplayAlert("Success", "Payment completed successfully!", "OK");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error processing successful payment: {ex}");
                        await Shell.Current.DisplayAlert("Error", "Payment was authorized but failed to process. Please contact support.", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Payment Failed", "The payment was not completed. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Apple Pay error: {ex}");
                await Shell.Current.DisplayAlert("Error", "There was an error processing your payment. Please try again.", "OK");
            }
#else
            await Shell.Current.DisplayAlert("Not Available", "Apple Pay is only available on iOS devices.", "OK");
#endif
        }

#if IOS
        private class CustomPaymentAuthorizationDelegate : PKPaymentAuthorizationControllerDelegate
        {
            private readonly TaskCompletionSource<(bool success, PKPayment payment)> _tcs;

            public CustomPaymentAuthorizationDelegate(TaskCompletionSource<(bool success, PKPayment payment)> tcs)
            {
                _tcs = tcs;
            }

            public override void DidAuthorizePayment(PKPaymentAuthorizationController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
            {
                try
                {
                    // Validate the payment
                    if (payment?.Token == null)
                    {
                        completion(PKPaymentAuthorizationStatus.Failure);
                        _tcs.SetResult((false, null));
                        return;
                    }

                    completion(PKPaymentAuthorizationStatus.Success);
                    _tcs.SetResult((true, payment));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Payment authorization error: {ex}");
                    completion(PKPaymentAuthorizationStatus.Failure);
                    _tcs.SetResult((false, null));
                }
            }

            public override void DidFinish(PKPaymentAuthorizationController controller)
            {
                controller.DismissAsync();
                if (!_tcs.Task.IsCompleted)
                {
                    _tcs.SetResult((false, null));
                }
            }
        }
#endif

        private async Task ProcessSuccessfulPayment(string packType)
        {
            try
            {
                Debug.WriteLine($"Processing successful payment for pack: {packType}");
                
                switch (packType)
                {
                    case "starter":
                        await _userService.AddSearchBalanceAsync(10);
                        await _userService.AddDeepDiveBalanceAsync(4);
                        break;
                    case "popular":
                        await _userService.AddSearchBalanceAsync(50);
                        await _userService.AddDeepDiveBalanceAsync(20);
                        break;
                    case "premium":
                        await _userService.AddSearchBalanceAsync(1000);
                        await _userService.AddDeepDiveBalanceAsync(400);
                        break;
                    default:
                        throw new ArgumentException($"Invalid pack type: {packType}");
                }

                await LoadBalancesAsync(); // Refresh balances after successful payment
                
                Debug.WriteLine($"Payment processed successfully. New balances - Search: {SearchBalance}, Deep Dive: {DeepDiveBalance}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error processing payment: {ex}");
                throw; // Rethrow to be handled by the caller
            }
        }
    }
} 