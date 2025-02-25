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
        private User _currentUser;

        public Command NavigateCommand => new(async () => await GetStarted());

        public GetStartedNameViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
            LoadCurrentUserAsync();
        }

        private async Task LoadCurrentUserAsync()
        {
            try
            {
                var appleUserId = Preferences.Get("AppleUserId", string.Empty);
                if (!string.IsNullOrEmpty(appleUserId))
                {
                    _currentUser = await _userService.GetUserByAppleIdAsync(appleUserId);
                    if (_currentUser != null && !string.IsNullOrEmpty(_currentUser.Name))
                    {
                        Name = _currentUser.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading user: {ex.Message}");
            }
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
                if (_currentUser == null)
                {
                    await Shell.Current.DisplayAlert("Error", "User not found. Please sign in again.", "OK");
                    await Shell.Current.GoToAsync("///MainPage");
                    return;
                }

                // Update the user's name in the database
                await _userService.UpdateUserNameAsync(_currentUser.AppleUserId, Name.Trim());

                // Navigate to PremiumOnboard
                await Shell.Current.GoToAsync("PremiumOnboard");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetStarted: {ex}");
                await Shell.Current.DisplayAlert("Error", "Failed to save your name. Please try again.", "OK");
            }
        }
    }
} 