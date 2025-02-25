using Microsoft.Maui.Controls;
using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views;

public partial class PremiumOnboard : ContentPage
{
    private readonly PremiumOnboardViewModel _viewModel;

    public PremiumOnboard(PremiumOnboardViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("QuotesPage");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Reset the view model state when the page appears
        _viewModel.IsMonthlySelected = true;
        _viewModel.IsProcessingPayment = false;
    }
} 