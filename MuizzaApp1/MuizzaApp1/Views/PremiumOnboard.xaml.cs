namespace MuizzaApp1.Views;

public partial class PremiumOnboard : ContentPage
{
    public PremiumOnboard()
    {
        InitializeComponent();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 