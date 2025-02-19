using MuizzaApp1.ViewModels;

namespace MuizzaApp1;

public partial class GetStarted4 : ContentPage
{
    readonly GetStarted4PageViewModel ViewModel;

    public GetStarted4(GetStarted4PageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;
        
        // Set initial opacity to 0
        this.Opacity = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Animate to full opacity
        await this.FadeTo(1, 500, Easing.CubicOut);
    }
} 