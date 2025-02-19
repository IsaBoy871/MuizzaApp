using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views;

public partial class Angry : ContentPage
{
    private readonly IListChoiceService _listChoiceService;
    readonly AngryViewModel ViewModel;

    public Angry(IListChoiceService listChoiceService, AngryViewModel viewModel)
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