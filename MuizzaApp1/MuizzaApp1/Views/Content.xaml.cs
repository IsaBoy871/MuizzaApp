namespace MuizzaApp1.Views;

public partial class Content : ContentPage
{
    public Content()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 