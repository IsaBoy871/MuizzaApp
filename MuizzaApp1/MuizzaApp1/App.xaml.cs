using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using SQLite;
using MuizzaApp1.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MuizzaApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
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