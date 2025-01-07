using MuizzaApp1.ViewModels;

namespace MuizzaApp1;

public partial class GetStarted3 : ContentPage
{
    readonly GetStarted3PageViewModel ViewModel;

    public GetStarted3(GetStarted3PageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;
    }
}