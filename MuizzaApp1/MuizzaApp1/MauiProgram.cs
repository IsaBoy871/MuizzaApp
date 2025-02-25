using Microsoft.Extensions.Logging;
using MuizzaApp1.ViewModels;
using CommunityToolkit.Maui;
using MuizzaApp1.Contracts.Services;
using MuizzaApp1.Services;
using Microsoft.Maui.Handlers;
using MuizzaApp1.Views;
using Microsoft.Extensions.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.Maui.Storage;

namespace MuizzaApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            try
            {
                Debug.WriteLine("[MauiProgram] Starting CreateMauiApp");
                
                var builder = MauiApp.CreateBuilder();
                Debug.WriteLine("[MauiProgram] Builder created");
                
                // Configure logging first
                builder.Services.AddLogging(logging =>
                {
                    logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Trace);
                });

                builder.UseMauiApp<App>()
                    .ConfigureFonts(fonts =>
                    {
                        Debug.WriteLine("[MauiProgram] Configuring fonts");
                        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                        fonts.AddFont("fredoka.ttf", "fredoka");
                    })
                    .UseMauiCommunityToolkit();

                // Load configuration
                try
                {
                    var assembly = typeof(MauiProgram).Assembly;
                    var stream = assembly.GetManifestResourceStream("MuizzaApp1.appsettings.json");
                    
                    if (stream != null)
                    {
                        var config = new ConfigurationBuilder()
                            .AddJsonStream(stream)
                            .Build();

                        builder.Configuration.AddConfiguration(config);
                    }
                    else
                    {
                        Debug.WriteLine("Warning: appsettings.json not found in embedded resources");
                    }

                    var resourceNames = assembly.GetManifestResourceNames();
                    foreach (var name in resourceNames)
                    {
                        Debug.WriteLine($"Found resource: {name}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Warning: Could not load appsettings.json: {ex.Message}");
                }

                // Add this right after ConfigureFonts but before registering services
                builder.Services.AddHttpClient("API", client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler();
#if DEBUG
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
#endif
                    return handler;
                });

                // Register HttpClient and Services first
                builder.Services.AddHttpClient<AffirmationsService>(client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
                })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpsClientHandler());

                builder.Services.AddSingleton<INavigationService, NavigationService>();
                builder.Services.AddSingleton<NotesService>();

                // Register ViewModels
                builder.Services.AddTransient<QuotesPageViewModel>();
                builder.Services.AddSingleton<MainPageViewModel>();
                builder.Services.AddSingleton<GetStarted2PageViewModel>();
                builder.Services.AddSingleton<GetStarted3PageViewModel>();

                // Register Pages
                builder.Services.AddTransient<QuotesPage>();
                builder.Services.AddTransient<NotesPage>();
                builder.Services.AddSingleton<AppShell>();
                builder.Services.AddSingleton<MainPage>();
                builder.Services.AddSingleton<GetStarted2>();
                builder.Services.AddSingleton<GetStarted3>();

                // Register pages and viewmodels
                builder.Services.AddTransient<EmotionalResponsePage>();
                builder.Services.AddTransient<EmotionalResponseViewModel>();
                builder.Services.AddSingleton<ResponseCacheService>();
                builder.Services.AddSingleton<IOpenAIService, OpenAIService>();

                builder.Services.AddSingleton<AITrainingService>();

                // Register HttpClient if not already registered
                builder.Services.AddHttpClient();

                builder.Services.AddTransient<BrainPage>();
                builder.Services.AddTransient<BrainPageViewModel>();

                // Register services
                builder.Services.AddSingleton<AffirmationsService>();  // Concrete implementation
                builder.Services.AddSingleton<IAffirmationsService>(sp => sp.GetRequiredService<AffirmationsService>()); // Interface mapping
                
                builder.Services.AddTransient<NotesListPage>();
                builder.Services.AddTransient<NotesListViewModel>();

                builder.Services.AddTransient<NotesPageViewModel>();

                builder.Services.AddSingleton<IAppleAuthService, AppleAuthService>();

                // Register GetStarted4 and its ViewModel
                builder.Services.AddTransient<GetStarted4>();
                builder.Services.AddTransient<GetStarted4PageViewModel>();

                // Register GetStartedName and its ViewModel
                builder.Services.AddTransient<GetStartedNamePage>();
                builder.Services.AddTransient<GetStartedNameViewModel>();

                builder.Services.AddSingleton<IListChoiceService, ListChoiceService>();

                // Add this after your existing service registrations
                builder.Services.AddTransient<AngryViewModel>();
                builder.Services.AddTransient<Angry>();
                builder.Services.AddTransient<ListChoice>();
                builder.Services.AddTransient<ListChoiceViewModel>();
                builder.Services.AddTransient<PremiumOnboard>();
                builder.Services.AddTransient<PremiumOnboardViewModel>();
                builder.Services.AddTransient<DeepDivePage>();
                builder.Services.AddTransient<DeepDiveViewModel>();

                builder.Services.AddSingleton<IUserService>(sp =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();
                    var baseUrl = config["ApiSettings:BaseUrl"] ?? "https://affirmations-api-cdgsf0azgqhrf2cx.uksouth-01.azurewebsites.net/";
                    return new UserService(new ConfigurationBuilder()
                        .AddInMemoryCollection(new Dictionary<string, string>
                        {
                            ["ApiSettings:BaseUrl"] = baseUrl
                        })
                        .Build());
                });

                builder.Services.AddSingleton<IStripeService, StripeService>();

                builder.Services.AddSingleton<ISubscriptionService, SubscriptionService>();

                // Add IPreferences service
                builder.Services.AddSingleton<IPreferences>(Preferences.Default);

                Debug.WriteLine("[MauiProgram] Building MauiApp");
                var app = builder.Build();
                
                var logger = app.Services.GetRequiredService<ILogger<MauiApp>>();
                logger.LogInformation("Application built successfully");
                
                return app;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MauiProgram ERROR] Failed to create MauiApp: {ex.Message}");
                Debug.WriteLine($"[MauiProgram ERROR] Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Configure DI Services
        /// </summary>
        /// <param name = "builder"></param>
        /// <returns></returns>
    }
}