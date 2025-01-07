namespace MuizzaApp1;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("GetStarted2", typeof(GetStarted2));
    }
}