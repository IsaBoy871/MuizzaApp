using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views;

public partial class Depressed : ContentPage
{
    private readonly IListChoiceService _listChoiceService;
    readonly DepressedViewModel ViewModel;

    public Depressed(IListChoiceService listChoiceService, DepressedViewModel viewModel)
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