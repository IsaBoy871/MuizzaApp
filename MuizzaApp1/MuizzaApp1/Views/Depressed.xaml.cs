namespace MuizzaApp1.Views;

public partial class Depressed : ContentPage
{
    public Depressed()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 