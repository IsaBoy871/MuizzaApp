using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MuizzaApp1.Contracts.Services;
using MuizzaApp1.Models;
using System.Collections.Generic;
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Microsoft.Maui.ApplicationModel;

#if IOS
using PassKit;
using UIKit;
using Foundation;
#endif

namespace MuizzaApp1.ViewModels
{
    [QueryProperty(nameof(SearchAmount), "searches")]
    [QueryProperty(nameof(DeepDiveAmount), "deepDives")]
    [QueryProperty(nameof(Amount), "price")]
    public partial class PaywallViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private decimal amount;

        [ObservableProperty]
        private int searchAmount;

        [ObservableProperty]
        private int deepDiveAmount;

        [ObservableProperty]
        private bool isProcessing;

        [ObservableProperty]
        private bool isApplePayAvailable;

        private readonly IUserService _userService;
        private readonly IInAppPurchaseService _inAppPurchaseService;
        private readonly INavigationService _navigationService;

        public IAsyncRelayCommand ApplePayCommand { get; }

        public PaywallViewModel(
            ISubscriptionService subscriptionService,
            IUserService userService,
            IInAppPurchaseService inAppPurchaseService,
            INavigationService navigationService) : base(subscriptionService)
        {
            _userService = userService;
            _inAppPurchaseService = inAppPurchaseService;
            _navigationService = navigationService;

            // Check if Apple Pay is available
            IsApplePayAvailable = DeviceInfo.Platform == DevicePlatform.iOS;

            ApplePayCommand = new AsyncRelayCommand(ProcessApplePayAsync, CanProcessApplePay);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Debug.WriteLine($"Received query parameters: {string.Join(", ", query.Select(kv => $"{kv.Key}={kv.Value}"))}");
            
            if (query.TryGetValue("searches", out var searches) && 
                query.TryGetValue("deepDives", out var deepDives) &&
                query.TryGetValue("price", out var price))
            {
                try
                {
                    Debug.WriteLine($"Converting parameters: searches={searches}, deepDives={deepDives}, price={price}");
                    
                    SearchAmount = Convert.ToInt32(searches);
                    DeepDiveAmount = Convert.ToInt32(deepDives);
                    Amount = Convert.ToDecimal(price);
                    Title = $"Purchase {SearchAmount} searches & {DeepDiveAmount} deep dives";

                    Debug.WriteLine($"Parameters converted successfully: SearchAmount={SearchAmount}, DeepDiveAmount={DeepDiveAmount}, Amount={Amount}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Parameter conversion error: {ex}");
                    Shell.Current.DisplayAlert(
                        "Error",
                        "Invalid purchase details. Please try again.",
                        "OK");
                    Shell.Current.Navigation.PopAsync();
                }
            }
            else
            {
                Debug.WriteLine($"Missing required parameters. Available keys: {string.Join(", ", query.Keys)}");
                Shell.Current.DisplayAlert(
                    "Error",
                    "Invalid purchase details. Please try again.",
                    "OK");
                Shell.Current.Navigation.PopAsync();
            }
        }

        private bool CanProcessApplePay()
        {
            return IsApplePayAvailable && !IsProcessing;
        }

        private async Task ProcessApplePayAsync()
        {
            if (!IsApplePayAvailable)
            {
                await Shell.Current.DisplayAlert(
                    "Not Available",
                    "Apple Pay is only available on iOS devices.",
                    "OK");
                return;
            }

            try
            {
                IsProcessing = true;

                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Please sign in again.", "OK");
                    return;
                }

                string productId = Amount switch
                {
                    6.99m => "com.muizza.subscription.monthly",
                    48.00m => "com.muizza.subscription.yearly",
                    _ => throw new ArgumentException($"Invalid amount: {Amount}")
                };

                var result = await _inAppPurchaseService.PurchaseAsync(productId);

                if (result.Success)
                {
                    await _userService.UpdateSubscriptionTierAsync(currentUser.AppleUserId, "Premium");
                    await Shell.Current.DisplayAlert("Success", "Payment completed successfully!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert(
                        "Error",
                        result.ErrorMessage ?? "Payment failed. Please try again.",
                        "OK"
                    );
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Payment error: {ex}");
                await Shell.Current.DisplayAlert(
                    "Error",
                    "An error occurred. Please try again.",
                    "OK"
                );
            }
            finally
            {
                IsProcessing = false;
            }
        }
    }
} 