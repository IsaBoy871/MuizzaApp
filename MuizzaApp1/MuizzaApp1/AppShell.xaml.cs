using MuizzaApp1.Views;

namespace MuizzaApp1;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(GetStarted2), typeof(GetStarted2));
        Routing.RegisterRoute(nameof(QuotesPage), typeof(QuotesPage));
        // Register other pages as needed
    }
}