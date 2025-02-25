using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using SQLite;
using MuizzaApp1.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using MuizzaApp1.Views;
using MuizzaApp1.Contracts.Services;

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

                try
                {
                    // Get required services
                    var navigationService = IPlatformApplication.Current.Services.GetRequiredService<INavigationService>();
                    Console.WriteLine("[App] Navigation service retrieved");
                    
                    MainPage = IPlatformApplication.Current.Services.GetRequiredService<AppShell>();
                    Console.WriteLine("[App] MainPage set to AppShell");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[App ERROR] Service resolution failed: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[App ERROR] Constructor failed: {ex.Message}");
                Console.WriteLine($"[App ERROR] Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            try
            {
                Debug.WriteLine("[App] Creating Window");
                var window = base.CreateWindow(activationState);
                Debug.WriteLine("[App] Window created successfully");
                
                // Ensure window is created with proper initialization
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