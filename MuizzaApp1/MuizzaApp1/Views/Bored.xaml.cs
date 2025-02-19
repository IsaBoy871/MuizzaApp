namespace MuizzaApp1.Views;

public partial class Bored : ContentPage
{
    public Bored()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 