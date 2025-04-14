using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MuizzaApp1.Contracts.Services;
using MuizzaApp1.Models;
using System.Windows.Input;
#if IOS
using Foundation;
using PassKit;
#endif

namespace MuizzaApp1.ViewModels
{
    public partial class PremiumOnboardViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IInAppPurchaseService _inAppPurchaseService;

        [ObservableProperty]
        private bool isMonthlySelected = true;

        [ObservableProperty]
        private bool isProcessingPayment;

        public ICommand SelectMonthlyCommand { get; }
        public ICommand SelectYearlyCommand { get; }
        public ICommand StartTrialCommand { get; }
        public ICommand InitiateApplePayCommand { get; }

        public PremiumOnboardViewModel(IUserService userService, IInAppPurchaseService inAppPurchaseService)
        {
            _userService = userService;
            _inAppPurchaseService = inAppPurchaseService;

            SelectMonthlyCommand = new Command(() => IsMonthlySelected = true);
            SelectYearlyCommand = new Command(() => IsMonthlySelected = false);
            StartTrialCommand = new Command(async () => await ProcessSubscription());
            InitiateApplePayCommand = new AsyncRelayCommand<string>(async (packType) =>
            {
                if (IsProcessingPayment) return;

                try
                {
                    IsProcessingPayment = true;

#if IOS
                    if (!PKPaymentAuthorizationController.CanMakePayments)
                    {
                        await Shell.Current.DisplayAlert("Error", "Apple Pay is not available on this device", "OK");
                        return;
                    }

                    var currentUser = await _userService.GetCurrentUserAsync();
                    if (currentUser == null)
                    {
                        await Shell.Current.DisplayAlert("Error", "Please sign in again.", "OK");
                        return;
                    }

                    decimal amount = IsMonthlySelected ? 6.99m : 48.00m;
                    string plan = IsMonthlySelected ? "Monthly" : "Yearly";
                    string productId = IsMonthlySelected ? "com.muizza.subscription.monthly" : "com.muizza.subscription.yearly";

                    var result = await _inAppPurchaseService.PurchaseAsync(productId);

                    if (result.Success)
                    {
                        await _userService.UpdateSubscriptionTierAsync(currentUser.AppleUserId, "Premium");
                        Preferences.Default.Set("HasCompletedOnboarding", true);
                        await Shell.Current.DisplayAlert("Success", "Welcome to Muizza Premium!", "OK");
                        await Shell.Current.GoToAsync("QuotesPage");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert(
                            "Error",
                            result.ErrorMessage ?? "Payment failed. Please try again.",
                            "OK"
                        );
                    }
#else
                    await Shell.Current.DisplayAlert(
                        "Not Available",
                        "Apple Pay is only available on iOS devices.",
                        "OK"
                    );
#endif
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", "An error occurred. Please try again.", "OK");
                    System.Diagnostics.Debug.WriteLine($"Payment error: {ex}");
                }
                finally
                {
                    IsProcessingPayment = false;
                }
            });
        }

        private async Task ProcessSubscription()
        {
            try
            {
                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Please sign in again.", "OK");
                    return;
                }

                await _userService.UpdateTrialStatusAsync(currentUser.AppleUserId, true);
                Preferences.Default.Set("HasCompletedOnboarding", true);
                await Shell.Current.DisplayAlert("Success", "Welcome to Muizza Premium Trial!", "OK");
                await Shell.Current.GoToAsync("QuotesPage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "An error occurred. Please try again.", "OK");
                System.Diagnostics.Debug.WriteLine($"Subscription error: {ex}");
            }
        }
    }
} 