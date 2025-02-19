using Microsoft.Maui.Controls;
using MuizzaApp1.ViewModels;

namespace MuizzaApp1;

public partial class GetStartedNamePage : ContentPage
{
    public GetStartedNamePage(GetStartedNameViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 