using Microsoft.Extensions.Logging;
using MuizzaApp1.ViewModels;
using CommunityToolkit.Maui;
using MuizzaApp1.Contracts.Services;
using MuizzaApp1.Services;

namespace MuizzaApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fredoka.ttf", "fredoka");
                });

            builder = ConfigureServices(builder);
            builder.UseMauiApp<App>().UseMauiCommunityToolkit();
            builder.Services.AddSingleton<AffirmationDatabase>();

            return builder.Build();
        }

        /// <summary>
        /// Configure DI Services
        /// </summary>
        /// <param name="builder"></param>
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
