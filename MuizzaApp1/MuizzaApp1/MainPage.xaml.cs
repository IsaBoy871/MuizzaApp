using MuizzaApp1.ViewModels;

namespace MuizzaApp1;

public partial class MainPage : ContentPage
{
    readonly MainPageViewModel ViewModel;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;
    }
}