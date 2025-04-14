using System;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MuizzaApp1.Models;
using MuizzaApp1.Contracts.Services;
using MuizzaApp1.Views;
using System.Windows.Input;
using System.Diagnostics;
using Plugin.LocalNotification;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;

namespace MuizzaApp1.ViewModels;

[ObservableObject]
public partial class BrainPageViewModel
{
    private readonly IPreferencesService _preferencesService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserService _userService;
    private const string DAILY_ENTRY_KEY = "current_daily_entry";
    private const string WELCOME_MESSAGE_KEY = "welcome_message";

    [ObservableProperty]
    private string welcomeMessage = "Hey, friend"; // Default value

    private string _affirmation;
    public string Affirmation
    {
        get => _affirmation;
        set
        {
            if (SetProperty(ref _affirmation, value))
            {
                IsCompleted = false;
            }
        }
    }

    private string _intention;
    public string Intention
    {
        get => _intention;
        set
        {
            if (SetProperty(ref _intention, value))
            {
                IsCompleted = false;
            }
        }
    }

    private string _gratitude;
    public string Gratitude
    {
        get => _gratitude;
        set
        {
            if (SetProperty(ref _gratitude, value))
            {
                IsCompleted = false;
            }
        }
    }

    [ObservableProperty]
    private bool isCompleted;

    public BrainPageViewModel(
        IPreferencesService preferencesService, 
        IUserService userService,
        IServiceProvider serviceProvider)
    {
        _preferencesService = preferencesService;
        _userService = userService;
        _serviceProvider = serviceProvider;
        LoadCurrentEntry();
        InitializeNotifications();
        InitializeAsync();
    }

    private async void InitializeNotifications()
    {
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            var isGranted = await LocalNotificationCenter.Current.RequestNotificationPermission();
            Debug.WriteLine($"[BrainPage] Notification permission granted: {isGranted}");
        }
    }

    private async void InitializeAsync()
    {
        try
        {
            var user = await _userService.GetCurrentUserAsync();
            WelcomeMessage = $"Hey, {user.Name}";
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[BrainPage] Failed to get user name: {ex.Message}");
            // Keep the default welcome message
        }
    }

    private void LoadCurrentEntry()
    {
        var entryJson = _preferencesService.Get(DAILY_ENTRY_KEY, string.Empty);
        if (!string.IsNullOrEmpty(entryJson))
        {
            try
            {
                var entry = JsonSerializer.Deserialize<DailyEntry>(entryJson);
                if (entry != null && entry.ExpiresAt > DateTime.Now)
                {
                    _affirmation = entry.Affirmation;
                    _intention = entry.Intention;
                    _gratitude = entry.Gratitude;
                    OnPropertyChanged(nameof(Affirmation));
                    OnPropertyChanged(nameof(Intention));
                    OnPropertyChanged(nameof(Gratitude));
                    IsCompleted = true;
                }
                else
                {
                    ClearCurrentEntry();
                }
            }
            catch
            {
                ClearCurrentEntry();
            }
        }
    }

    private void ClearCurrentEntry()
    {
        Affirmation = string.Empty;
        Intention = string.Empty;
        Gratitude = string.Empty;
        IsCompleted = false;
        _preferencesService.Remove(DAILY_ENTRY_KEY);
    }

    [RelayCommand]
    public async Task SaveDailyEntryAsync()
    {
        if (string.IsNullOrWhiteSpace(Affirmation) || 
            string.IsNullOrWhiteSpace(Intention) || 
            string.IsNullOrWhiteSpace(Gratitude))
        {
            return;
        }

        var entry = new DailyEntry
        {
            Affirmation = Affirmation,
            Intention = Intention,
            Gratitude = Gratitude,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Today.AddDays(1)
        };

        var entryJson = JsonSerializer.Serialize(entry);
        _preferencesService.Set(DAILY_ENTRY_KEY, entryJson);
        IsCompleted = true;

        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            try
            {
                var notificationCenter = LocalNotificationCenter.Current;
                
                // Cancel all previous notifications
                notificationCenter.Cancel(50);  // Test notification
                notificationCenter.Cancel(100); // Affirmation notification
                notificationCenter.Cancel(200); // Intention notification
                notificationCenter.Cancel(300); // Gratitude notification
                Debug.WriteLine("[BrainPage] Attempting to cancel previous notifications");

                // Send an immediate test notification
                var testRequest = new NotificationRequest
                {
                    NotificationId = 50,
                    Title = "✨ You Classy Cat!",
                    Description = "Your daily reflections have been saved. You'll receive reminders throughout the day.",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(2),
                        RepeatType = NotificationRepeat.No
                    }
                };
                notificationCenter.Show(testRequest);
                Debug.WriteLine("[BrainPage] Sent test notification");

                // Schedule notifications for specific times
                var notifications = new[]
                {
                    new
                    {
                        Id = 100,
                        Title = "Your Affirmation",
                        Content = entry.Affirmation,
                        Hour = 10
                    },
                    new
                    {
                        Id = 200,
                        Title = "Your Intention",
                        Content = entry.Intention,
                        Hour = 14
                    },
                    new
                    {
                        Id = 300,
                        Title = "Your Gratitude",
                        Content = entry.Gratitude,
                        Hour = 18
                    }
                };

                foreach (var notification in notifications)
                {
                    var notifyTime = DateTime.Today.AddHours(notification.Hour);
                    
                    // If the time has passed today, schedule for tomorrow
                    if (notifyTime < DateTime.Now)
                    {
                        notifyTime = notifyTime.AddDays(1);
                    }

                    var request = new NotificationRequest
                    {
                        NotificationId = notification.Id,
                        Title = notification.Title,
                        Description = notification.Content,
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = notifyTime,
                            RepeatType = NotificationRepeat.Daily
                        }
                    };

                    notificationCenter.Show(request);
                    Debug.WriteLine($"[BrainPage] Scheduled {notification.Title} notification for {notifyTime}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BrainPage] Failed to handle notifications: {ex.Message}");
                Debug.WriteLine($"[BrainPage] Stack trace: {ex.StackTrace}");
            }
        }
    }

    [RelayCommand]
    private async Task NavigateToQuotesPageAsync()
    {
        try
        {
            await Shell.Current.GoToAsync($"//{nameof(QuotesPage)}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Navigation error: {ex}");
            await Shell.Current.DisplayAlert("Error", "Unable to navigate to Quotes page", "OK");
        }
    }

    [RelayCommand]
    private async Task NavigateToNotesPageAsync()
    {
        try
        {
            var notesPage = _serviceProvider.GetService<NotesPage>();
            await Shell.Current.Navigation.PushAsync(notesPage);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Navigation error: {ex}");
            await Shell.Current.DisplayAlert("Error", "Unable to navigate to Notes page", "OK");
        }
    }
} 