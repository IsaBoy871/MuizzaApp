using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Storage;
using MuizzaApp1.Contracts.Services;

namespace MuizzaApp1.ViewModels
{
    public partial class GetStartedNameViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;

        public Command NavigateCommand => new(async () => await GetStarted());

        public GetStartedNameViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
        }
        
        [RelayCommand]
        private async Task GetStarted()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Oops!", "Please enter your name first!", "OK");
                return;
            }

            try
            {
                // Get the AppleUserId from preferences
                var appleUserId = Preferences.Get("AppleUserId", string.Empty);
                if (string.IsNullOrEmpty(appleUserId))
                {
                    await Shell.Current.DisplayAlert("Error", "User not found. Please sign in again.", "OK");
                    return;
                }

                // Update the user's name in the database
                await _userService.UpdateUserNameAsync(appleUserId, Name.Trim());

                // Save name to preferences
                Preferences.Set("UserName", Name.Trim());

                // Navigate to main app
                await Shell.Current.GoToAsync("QuotesPage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to save your name. Please try again.", "OK");
            }
        }
    }
} 