using MuizzaApp1.Views;
using MuizzaApp1.ViewModels;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using System.Diagnostics;
using MuizzaApp1.Contracts.Services;

namespace MuizzaApp1;

public partial class AppShell : Shell
{
    private bool isNavigating = false;
    private Dictionary<string, Page> pageCache = new();
    private IServiceProvider _services;
    private readonly INavigationService _navigationService;

    public AppShell(INavigationService navigationService)
    {
        try
        {
            Console.WriteLine("[AppShell] Constructor starting");
            _navigationService = navigationService;
            
            InitializeComponent();
            Console.WriteLine("[AppShell] InitializeComponent completed");

            // Register routes for navigation
            Routing.RegisterRoute(nameof(GetStarted2), typeof(GetStarted2));
            Routing.RegisterRoute(nameof(GetStarted3), typeof(GetStarted3));
            Routing.RegisterRoute(nameof(GetStarted4), typeof(GetStarted4));
            Routing.RegisterRoute(nameof(QuotesPage), typeof(QuotesPage));
            Routing.RegisterRoute(nameof(BrainPage), typeof(BrainPage));
            Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
            Routing.RegisterRoute(nameof(NotesListPage), typeof(NotesListPage));
            Routing.RegisterRoute(nameof(Angry), typeof(Angry));
            Routing.RegisterRoute(nameof(Anxious), typeof(Anxious));
            Routing.RegisterRoute(nameof(Bored), typeof(Bored));
            Routing.RegisterRoute(nameof(Depressed), typeof(Depressed));
            Routing.RegisterRoute(nameof(Inspired), typeof(Inspired));
            Routing.RegisterRoute(nameof(Grateful), typeof(Grateful));
            Routing.RegisterRoute(nameof(Restless), typeof(Restless));
            Routing.RegisterRoute(nameof(Insecure), typeof(Insecure));
            Routing.RegisterRoute(nameof(Hopeful), typeof(Hopeful));
            Routing.RegisterRoute(nameof(Content), typeof(Content));
            Routing.RegisterRoute(nameof(Confident), typeof(Confident));
            Routing.RegisterRoute(nameof(Motivated), typeof(Motivated));
            Routing.RegisterRoute(nameof(ListChoice), typeof(ListChoice));
            Routing.RegisterRoute(nameof(PremiumOnboard), typeof(PremiumOnboard));
            Routing.RegisterRoute("DeepDivePage", typeof(DeepDivePage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("AdvisorSelectionPage", typeof(AdvisorSelectionPage));
            Console.WriteLine("[AppShell] MainPage route registered");
            // Disable default Shell transitions
            Shell.SetNavBarIsVisible(this, false);
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);

            // Handle navigation with smooth transition
            this.Navigating += async (s, e) =>
            {
                if (isNavigating) return;
                e.Cancel();
                isNavigating = true;

                try
                {
                    var destination = e.Target.Location.OriginalString;
                    
                    // Pre-warm the destination page if it's QuotesPage
                    if (destination == nameof(QuotesPage) && !pageCache.ContainsKey(destination))
                    {
                        await PreloadQuotesPage();
                    }
                    
                    // Execute transition
                    var currentPage = this.CurrentPage;
                    await Task.WhenAll(
                        currentPage.FadeTo(0, 200, Easing.CubicInOut),
                        currentPage.ScaleTo(0.9, 200, Easing.CubicInOut)
                    );
                    
                    await Shell.Current.GoToAsync(destination, false);
                    var newPage = this.CurrentPage;
                    newPage.Opacity = 0;
                    newPage.Scale = 0.9;
                    
                    await Task.WhenAll(
                        newPage.FadeTo(1, 200, Easing.CubicInOut),
                        newPage.ScaleTo(1, 200, Easing.CubicInOut)
                    );
                }
                finally
                {
                    isNavigating = false;
                }
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AppShell ERROR] Constructor failed: {ex.Message}");
            Console.WriteLine($"[AppShell ERROR] Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        
        if (Handler != null)
        {
            _services = Handler.MauiContext.Services;
            
            // Preload QuotesPage immediately when app starts
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await PreloadQuotesPage();
                
                // Preload other pages after QuotesPage
                PreloadPages(new[] { 
                    nameof(NotesPage), 
                    nameof(BrainPage), 
                    nameof(NotesListPage) 
                });
            });
        }
    }

    private async Task PreloadQuotesPage()
    {
        if (_services == null || pageCache.ContainsKey(nameof(QuotesPage))) return;

        try
        {
            var quotesPage = _services.GetService<QuotesPage>();
            if (quotesPage != null)
            {
                // Force layout calculation before caching
                quotesPage.Layout(new Rect(0, 0, Window.Width, Window.Height));
                
                // Start loading data immediately
                if (quotesPage.BindingContext is QuotesPageViewModel viewModel)
                {
                    await viewModel.LoadInitialAffirmationsAsync();
                }
                
                // Cache the page
                pageCache[nameof(QuotesPage)] = quotesPage;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error preloading QuotesPage: {ex.Message}");
        }
    }

    private void PreloadPages(string[] pageNames)
    {
        if (_services == null) return;

        foreach (var pageName in pageNames)
        {
            try
            {
                var pageType = Type.GetType($"MuizzaApp1.Views.{pageName}");
                if (pageType != null && !pageCache.ContainsKey(pageName))
                {
                    pageCache[pageName] = (Page)_services.GetService(pageType);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error preloading {pageName}: {ex.Message}");
            }
        }
    }
}