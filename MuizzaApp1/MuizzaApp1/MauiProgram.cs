using Microsoft.Extensions.Logging;
using MuizzaApp1.ViewModels;
using CommunityToolkit.Maui;
using MuizzaApp1.Contracts.Services;
using MuizzaApp1.Services;
using Microsoft.Maui.Handlers;
using MuizzaApp1.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace MuizzaApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            
            builder.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
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

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddDebug();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
            });

            // Add this line to register the AITrainingService
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

            return builder.Build();
        }

        /// <summary>
        /// Configure DI Services
        /// </summary>
        /// <param name = "builder"></param>
        /// <returns></returns>
        internal static MauiAppBuilder ConfigureServices(MauiAppBuilder builder)
        {
            // View Model and Pages
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<GetStarted2>();
            builder.Services.AddSingleton<GetStarted2PageViewModel>();
            builder.Services.AddSingleton<GetStarted3>();
            builder.Services.AddSingleton<GetStarted3PageViewModel>();
            builder.Services.AddSingleton<QuotesPage>();
            //builder.Services.AddTransient<PetDetailsViewModel>();
            //builder.Services.AddSingleton<FavouritePetsPage>();
            //builder.Services.AddTransient<FavouritePetsViewModel>();
            //builder.Services.AddSingleton<PetsLocationPage>();
            //builder.Services.AddTransient<PetsLocationViewModel>();
            //builder.Services.AddSingleton<MaintenancePage>();
            //builder.Services.AddTransient<MaintenanceViewModel>();
            // Services
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            return builder;
        }
    }
}