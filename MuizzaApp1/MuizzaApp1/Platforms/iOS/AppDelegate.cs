using Foundation;
using Microsoft.Maui;
using System.Diagnostics;
using UIKit;

namespace MuizzaApp1
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp()
        {
            try
            {
                Debug.WriteLine("[AppDelegate] Starting CreateMauiApp");
                var builder = MauiApp.CreateBuilder();
                Debug.WriteLine("[AppDelegate] Builder created");
                
                var app = MauiProgram.CreateMauiApp();
                Debug.WriteLine("[AppDelegate] MauiApp created successfully");
                return app;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AppDelegate ERROR] Failed to create MauiApp: {ex.Message}");
                Debug.WriteLine($"[AppDelegate ERROR] Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            try
            {
                Debug.WriteLine("[AppDelegate] FinishedLaunching called");
                var result = base.FinishedLaunching(application, launchOptions);
                Debug.WriteLine("[AppDelegate] FinishedLaunching completed successfully");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AppDelegate ERROR] FinishedLaunching failed: {ex.Message}");
                Debug.WriteLine($"[AppDelegate ERROR] Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
