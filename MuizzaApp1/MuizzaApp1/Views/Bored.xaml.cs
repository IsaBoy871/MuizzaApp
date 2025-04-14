using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views;

public partial class Bored : ContentPage
{
    private readonly IListChoiceService _listChoiceService;
    readonly BoredViewModel ViewModel;

    public Bored(IListChoiceService listChoiceService, BoredViewModel viewModel)
    {
        InitializeComponent();
        _listChoiceService = listChoiceService;
        BindingContext = ViewModel = viewModel;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 