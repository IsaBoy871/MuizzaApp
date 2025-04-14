using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views;

public partial class Anxious : ContentPage
{
    private readonly IListChoiceService _listChoiceService;
    readonly AnxiousViewModel ViewModel;

    public Anxious(IListChoiceService listChoiceService, AnxiousViewModel viewModel)
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