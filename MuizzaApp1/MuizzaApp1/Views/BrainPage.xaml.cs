using Microsoft.Maui.Controls;
using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views;

public partial class BrainPage : ContentPage
{
    public BrainPage(BrainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
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
} 