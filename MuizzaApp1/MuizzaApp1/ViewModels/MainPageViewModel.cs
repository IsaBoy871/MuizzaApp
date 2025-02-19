using MuizzaApp1.Contracts.Services;
using MuizzaApp1.ViewModels.Base;
using System.Threading.Tasks;

namespace MuizzaApp1.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IAppleAuthService _appleAuthService;
        private readonly IUserService _userService;

        public Command SignInWithAppleCommand { get; }

        public MainPageViewModel(
            INavigationService navigationService, 
            IAppleAuthService appleAuthService,
            IUserService userService)
        {
            _navigationService = navigationService;
            _appleAuthService = appleAuthService;
            _userService = userService;
            
            SignInWithAppleCommand = new Command(async () => await SignInWithAppleAsync());
        }

        private async Task SignInWithAppleAsync()
        {
            try
            {
                var result = await _appleAuthService.AuthenticateAsync();
                
                if (result.IsSuccessful)
                {
                    // Check if user exists
                    var existingUser = await _userService.GetUserByAppleIdAsync(result.UserId);
                    
                    if (existingUser == null)
                    {
                        // Create new user with just the Apple User ID
                        await _userService.CreateUserAsync(result.UserId);
                    }

                    // Store user ID in preferences
                    Preferences.Set("AppleUserId", result.UserId);
                    
                    // Navigate to next page after successful sign in
                    await _navigationService.NavigateToPage<GetStarted2>();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Authentication Failed", 
                        "Unable to sign in with Apple. Please try again.",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred during sign in.";
                if (ex is HttpRequestException httpEx)
                {
                    errorMessage = httpEx.Message;
                }
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error", 
                    errorMessage,
                    "OK");
            }
        }
    }
}