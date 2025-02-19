using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using SQLite;
using MuizzaApp1.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using MuizzaApp1.Views;

namespace MuizzaApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Create and set MainPage before any other initialization
            MainPage = new AppShell();

            // Register routes after MainPage is set
            Routing.RegisterRoute(nameof(GetStarted2), typeof(GetStarted2));
            Routing.RegisterRoute(nameof(GetStarted3), typeof(GetStarted3));
            Routing.RegisterRoute(nameof(GetStarted4), typeof(GetStarted4));
            Routing.RegisterRoute(nameof(QuotesPage), typeof(QuotesPage));
            Routing.RegisterRoute(nameof(BrainPage), typeof(BrainPage));
            Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
            Routing.RegisterRoute(nameof(NotesListPage), typeof(NotesListPage));
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);
            
            // Ensure window is created with proper initialization
            if (window != null)
            {
                window.Page = MainPage;
            }
            
            return window;
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