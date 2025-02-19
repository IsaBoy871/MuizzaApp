namespace MuizzaApp1.Views;

public partial class Motivated : ContentPage
{
    public Motivated()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 