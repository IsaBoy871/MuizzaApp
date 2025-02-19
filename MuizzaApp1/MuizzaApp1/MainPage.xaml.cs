using MuizzaApp1.ViewModels;

namespace MuizzaApp1;

public partial class MainPage : ContentPage
{
    readonly MainPageViewModel ViewModel;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;
        this.Opacity = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.FadeTo(1, 500, Easing.CubicOut);
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await this.FadeTo(0, 500, Easing.CubicIn);
    }
}