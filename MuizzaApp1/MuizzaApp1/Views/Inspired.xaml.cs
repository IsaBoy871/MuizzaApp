namespace MuizzaApp1.Views;

public partial class Inspired : ContentPage
{
    public Inspired()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 