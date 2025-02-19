namespace MuizzaApp1.Views;

public partial class Hopeful : ContentPage
{
    public Hopeful()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 