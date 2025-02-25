using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MuizzaApp1.Contracts.Services;
using System.Windows.Input;

namespace MuizzaApp1.ViewModels
{
    public partial class PremiumOnboardViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IStripeService _stripeService;

        [ObservableProperty]
        private bool isMonthlySelected = true;

        [ObservableProperty]
        private bool isProcessingPayment;

        public ICommand SelectMonthlyCommand { get; }
        public ICommand SelectYearlyCommand { get; }
        public ICommand StartTrialCommand { get; }

        public PremiumOnboardViewModel(IUserService userService, IStripeService stripeService)
        {
            _userService = userService;
            _stripeService = stripeService;

            SelectMonthlyCommand = new Command(() => IsMonthlySelected = true);
            SelectYearlyCommand = new Command(() => IsMonthlySelected = false);
            StartTrialCommand = new Command(async () => await ProcessSubscription());
        }

        private async Task ProcessSubscription()
        {
            if (IsProcessingPayment) return;

            try
            {
                IsProcessingPayment = true;

                // Get current user
                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Please sign in again.", "OK");
                    await Shell.Current.GoToAsync("MainPage");
                    return;
                }

                decimal amount = IsMonthlySelected ? 6.99m : 41.94m;
                string plan = IsMonthlySelected ? "Monthly" : "Yearly";

                var paymentResult = await _stripeService.ProcessPaymentAsync(
                    amount, 
                    $"Muizza Premium - {plan}"
                );

                if (paymentResult.IsSuccessful)
                {
                    await _userService.UpdateSubscriptionTierAsync(currentUser.AppleUserId, "Premium");
                    await Shell.Current.DisplayAlert("Success", "Welcome to Muizza Premium!", "OK");
                    await Shell.Current.GoToAsync("QuotesPage");
                }
                else
                {
                    await Shell.Current.DisplayAlert(
                        "Error",
                        paymentResult.Error ?? "Payment failed. Please try again.",
                        "OK"
                    );
                }
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
        }
    }
} 