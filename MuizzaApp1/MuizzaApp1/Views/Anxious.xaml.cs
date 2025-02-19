namespace MuizzaApp1.Views;

public partial class Anxious : ContentPage
{
    public Anxious()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 