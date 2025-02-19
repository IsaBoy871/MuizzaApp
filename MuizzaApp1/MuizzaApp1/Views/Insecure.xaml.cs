namespace MuizzaApp1.Views;

public partial class Insecure : ContentPage
{
    public Insecure()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 