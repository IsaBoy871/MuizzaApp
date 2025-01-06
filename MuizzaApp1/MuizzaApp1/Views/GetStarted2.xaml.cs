using MuizzaApp1.ViewModels;

namespace MuizzaApp1;

public partial class GetStarted2 : ContentPage
{
    readonly GetStarted2PageViewModel ViewModel;

    public GetStarted2(GetStarted2PageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;
    }
}