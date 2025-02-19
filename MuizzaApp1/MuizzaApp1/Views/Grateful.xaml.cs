namespace MuizzaApp1.Views;

public partial class Grateful : ContentPage
{
    public Grateful()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 