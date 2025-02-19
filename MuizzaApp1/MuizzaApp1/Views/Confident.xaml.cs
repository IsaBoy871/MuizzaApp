namespace MuizzaApp1.Views;

public partial class Confident : ContentPage
{
    public Confident()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 