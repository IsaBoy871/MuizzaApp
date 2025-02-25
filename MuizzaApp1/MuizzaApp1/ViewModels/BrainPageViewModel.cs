using CommunityToolkit.Mvvm.ComponentModel;
using MuizzaApp1.Views;
using System.Windows.Input;

namespace MuizzaApp1.ViewModels;

[ObservableObject]
public partial class BrainPageViewModel
{
    private readonly IServiceProvider _serviceProvider;

    private readonly IUserService _userService;

    [ObservableProperty]
    private string welcomeMessage = "Hey,"; // Default value while loading

    public ICommand NavigateToQuotesPage { get; }
    public ICommand NavigateToNotesPage { get; }
    public ICommand NavigateToBrainPage { get; }

    public BrainPageViewModel(IServiceProvider serviceProvider, IUserService userService)
    {
        _serviceProvider = serviceProvider;
        _userService = userService;
        LoadUserNameAsync();
        
        NavigateToQuotesPage = new Command(async () => await Shell.Current.GoToAsync(nameof(QuotesPage), false));
        NavigateToNotesPage = new Command(async () => await Shell.Current.GoToAsync(nameof(NotesPage), false));
        NavigateToBrainPage = new Command(async () => await Shell.Current.GoToAsync(nameof(BrainPage), false));
    }

    private async Task LoadUserNameAsync()
    {
        try
        {
            var appleUserId = Preferences.Get("AppleUserId", string.Empty);
            if (!string.IsNullOrEmpty(appleUserId))
            {
                var user = await _userService.GetUserByAppleIdAsync(appleUserId);
                if (user != null && !string.IsNullOrEmpty(user.Name))
                {
                    WelcomeMessage = $"Hey, {user.Name}";
                }
            }
        }
        catch (Exception ex)
        {
            // Handle error silently or log it
            System.Diagnostics.Debug.WriteLine($"Error loading user name: {ex.Message}");
        }
    }
} 