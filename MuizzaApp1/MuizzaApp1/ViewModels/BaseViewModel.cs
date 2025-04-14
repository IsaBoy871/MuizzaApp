using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MuizzaApp1.Contracts.Services;
using System.Diagnostics;

namespace MuizzaApp1.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly ISubscriptionService _subscriptionService;
        
        [ObservableProperty]
        private bool isPremiumUser;

        protected BaseViewModel(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public async Task CheckSubscriptionStatus()
        {
            try
            {
                IsPremiumUser = await _subscriptionService.IsPremiumUser();
                Debug.WriteLine($"CheckSubscriptionStatus - IsPremiumUser: {IsPremiumUser}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking subscription status: {ex.Message}");
                IsPremiumUser = false;
            }
        }

        protected async Task<bool> ValidatePremiumFeature()
        {
            if (await _subscriptionService.IsPremiumUser())
                return true;

            var upgrade = await Shell.Current.DisplayAlert(
                "Premium Feature", 
                "This feature is only available to premium subscribers. Would you like to upgrade?",
                "Upgrade",
                "Maybe Later");

            if (upgrade)
            {
                await Shell.Current.GoToAsync("PremiumOnboard");
            }
            return false;
        }

        public virtual void Cleanup()
        {
            // Base implementation - can be empty
        }
    }
} 