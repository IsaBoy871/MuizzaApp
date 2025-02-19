namespace MuizzaApp1.Views;

public partial class Restless : ContentPage
{
    public Restless()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 