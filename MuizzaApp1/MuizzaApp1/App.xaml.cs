using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using SQLite;
using MuizzaApp1.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using MuizzaApp1.Views;
using MuizzaApp1.Contracts.Services;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace MuizzaApp1
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                Console.WriteLine("[App] Constructor starting");
                InitializeComponent();
                Console.WriteLine("[App] InitializeComponent completed");
                
                // Initialize notifications
                InitializeNotifications();
                
                // Get required services first
                var navigationService = IPlatformApplication.Current.Services.GetRequiredService<INavigationService>();
                var appShell = IPlatformApplication.Current.Services.GetRequiredService<AppShell>();
                
                // Set MainPage to AppShell
                MainPage = appShell;

                // Check if this is first launch
                bool hasCompletedOnboarding = Preferences.Default.Get("HasCompletedOnboarding", false);
                Console.WriteLine($"[App] Has completed onboarding: {hasCompletedOnboarding}");

                if (hasCompletedOnboarding)
                {
                    // Wait for shell to be ready before navigating
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            await Task.Delay(100);  // Give shell time to initialize
                            Console.WriteLine("[App] Attempting navigation to QuotesPage");
                            await Shell.Current.GoToAsync("//QuotesPage");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[App ERROR] Navigation failed: {ex.Message}");
                            Console.WriteLine($"[App ERROR] Stack trace: {ex.StackTrace}");
                        }
                    });
                }
                else
                {
                    Console.WriteLine("[App] Starting with MainPage for onboarding");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[App ERROR] Constructor failed: {ex.Message}");
                Console.WriteLine($"[App ERROR] Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private async void InitializeNotifications()
        {
#if IOS
            try
            {
                var notificationCenter = LocalNotificationCenter.Current;
                
                // Subscribe to notification events
                notificationCenter.NotificationActionTapped += OnNotificationActionTapped;

                // Request permission
                var settings = await notificationCenter.RequestNotificationPermission();
                Debug.WriteLine($"[App] Notification settings: {settings}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[App] Error initializing notifications: {ex.Message}");
            }
#endif
        }

        private void OnNotificationActionTapped(NotificationActionEventArgs e)
        {
            Debug.WriteLine($"[App] Notification tapped: {e.Request.NotificationId}");
            
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Navigate to BrainPage when notification is tapped
                await Shell.Current.GoToAsync("///BrainPage");
            });
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            try
            {
                Debug.WriteLine("[App] Creating Window");
                var window = base.CreateWindow(activationState);
                Debug.WriteLine("[App] Window created successfully");
                
                if (window != null)
                {
                    window.Page = MainPage;
                }
                
                return window;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[App ERROR] Window creation failed: {ex.Message}");
                Debug.WriteLine($"[App ERROR] Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        protected override async void OnStart()
        {
            base.OnStart();
            
            /*
#if DEBUG
            try
            {
                Debug.WriteLine("Starting test training process...");
                
                var trainingService = IPlatformApplication.Current.Services.GetService<AITrainingService>();
                if (trainingService == null)
                {
                    Debug.WriteLine("ERROR: Training service is null!");
                    return;
                }

                Debug.WriteLine("Running test training...");
                await trainingService.RunTestTraining();
                Debug.WriteLine("Test training completed successfully!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR during training: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
#endif
            */
        }
    }
}

//    var cacheService = IPlatformApplication.Current.Services.GetService<ResponseCacheService>();
        //    cacheService?.ClearAllCache();
        //    // Remove this code after running once