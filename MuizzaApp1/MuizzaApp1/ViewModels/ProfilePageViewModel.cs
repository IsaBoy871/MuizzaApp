using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace MuizzaApp1.ViewModels
{
    public partial class ProfilePageViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private bool dailyNotificationsEnabled;

        [ObservableProperty]
        private bool weeklyNotificationsEnabled;

        private int _selectedTimeIndex;
        public int SelectedTimeIndex
        {
            get => _selectedTimeIndex;
            set
            {
                if (_selectedTimeIndex != value)
                {
                    _selectedTimeIndex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedTime));
                }
            }
        }

        public string SelectedTime => AvailableTimes[SelectedTimeIndex];

        public List<string> AvailableTimes { get; } = new List<string>
        {
            "4:00 AM",
            "5:00 AM",
            "6:00 AM",
            "7:00 AM",
            "8:00 AM",
            "9:00 AM"
        };

        public ICommand SaveNameCommand { get; }
        public ICommand SignOutCommand { get; }

        public ProfilePageViewModel(IUserService userService)
        {
            _userService = userService;

            // Load user data
            LoadUserData();

            // Initialize commands
            SaveNameCommand = new Command(async () => await SaveUserName());
            SignOutCommand = new Command(async () => await SignOut());

            // Subscribe to property changes for notifications
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(DailyNotificationsEnabled) ||
                    e.PropertyName == nameof(WeeklyNotificationsEnabled) ||
                    e.PropertyName == nameof(SelectedTimeIndex))
                {
                    SaveNotificationSettings();
                }
            };
        }

        private async void LoadUserData()
        {
            try
            {
                var appleUserId = Preferences.Get("AppleUserId", string.Empty);
                if (!string.IsNullOrEmpty(appleUserId))
                {
                    var user = await _userService.GetUserByAppleIdAsync(appleUserId);
                    if (user != null)
                    {
                        UserName = user.Name;
                    }
                }

                // Load notification settings
                DailyNotificationsEnabled = Preferences.Get("DailyNotifications", true);
                WeeklyNotificationsEnabled = Preferences.Get("WeeklyNotifications", true);
                SelectedTimeIndex = Preferences.Get("SelectedTimeIndex", 0); // Default to 4:00 AM
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load profile data", "OK");
                System.Diagnostics.Debug.WriteLine($"Error loading profile: {ex.Message}");
            }
        }

        private async Task SaveUserName()
        {
            try
            {
                var appleUserId = Preferences.Get("AppleUserId", string.Empty);
                if (!string.IsNullOrEmpty(appleUserId))
                {
                    await _userService.UpdateUserNameAsync(appleUserId, UserName);
                    await Shell.Current.DisplayAlert("Success", "Name updated successfully", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to update name", "OK");
                System.Diagnostics.Debug.WriteLine($"Error saving name: {ex.Message}");
            }
        }

        private void SaveNotificationSettings()
        {
            Preferences.Set("DailyNotifications", DailyNotificationsEnabled);
            Preferences.Set("WeeklyNotifications", WeeklyNotificationsEnabled);
            Preferences.Set("SelectedTimeIndex", SelectedTimeIndex);
        }

        private async Task SignOut()
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Sign Out",
                "Are you sure you want to sign out?",
                "Yes",
                "No"
            );

            if (confirm)
            {
                // Clear user data
                Preferences.Clear();
                
                // Navigate to login/onboarding
                await Shell.Current.GoToAsync("///MainPage");
            }
        }
    }
} 