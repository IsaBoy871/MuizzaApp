using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using SQLite;

namespace MuizzaApp1
{
    public partial class App : Application
    {
        private readonly AffirmationDatabase _database;

        public App(AffirmationDatabase database)
        {
            try
            {
                InitializeComponent();
                _database = database;
                
                // Switch to using Task.Run instead of MainThread
                Task.Run(async () =>
                {
                    try
                    {
                        await InitializeDatabaseAsync();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Fatal error during initialization: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Constructor error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            try
            {
                return new Window(new AppShell());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Window creation error: {ex.Message}");
                throw;
            }
        }

        private async Task InitializeDatabaseAsync()
        {
            try
            {
                var affirmations = await _database.GetAffirmationsAsync();
                System.Diagnostics.Debug.WriteLine($"Found {affirmations?.Count ?? 0} existing affirmations");
                
                if (!affirmations?.Any() ?? true)
                {
                    System.Diagnostics.Debug.WriteLine("Seeding database...");
                    await _database.SeedDatabaseAsync();
                }
            }
            catch (SQLiteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQLite error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}