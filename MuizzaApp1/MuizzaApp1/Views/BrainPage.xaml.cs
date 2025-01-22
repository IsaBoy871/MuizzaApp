using Microsoft.Maui.Controls;

namespace MuizzaApp1.Views;

public partial class BrainPage : ContentPage
{
    public BrainPage()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override bool OnBackButtonPressed()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync("..");
        });
        return true;
    }

    private async void OnQuotesClicked(object sender, EventArgs e)
    {
        var quotesPage = Handler.MauiContext.Services.GetService<QuotesPage>();
        await Shell.Current.Navigation.PushAsync(quotesPage);
    }
} 