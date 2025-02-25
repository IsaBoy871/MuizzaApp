using UIKit;
using System.Diagnostics;
using Foundation;

namespace MuizzaApp1;

public class Program
{
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("[iOS Main] Application starting...");
            
            // Set up a global exception handler
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var exception = e.ExceptionObject as Exception;
                Console.WriteLine($"[CRASH] Unhandled Exception: {exception?.Message}");
                Console.WriteLine($"[CRASH] Stack Trace: {exception?.StackTrace}");
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Console.WriteLine($"[CRASH] Unobserved Task Exception: {e.Exception.Message}");
                Console.WriteLine($"[CRASH] Stack Trace: {e.Exception.StackTrace}");
            };
            
            Console.WriteLine("[iOS Main] About to call UIApplication.Main");
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FATAL ERROR] {ex.GetType().Name}: {ex.Message}");
            Console.WriteLine($"[FATAL ERROR] Stack trace: {ex.StackTrace}");
            throw;
        }
    }
}
