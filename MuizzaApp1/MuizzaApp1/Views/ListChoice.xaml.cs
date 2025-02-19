using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views;

[QueryProperty(nameof(Emotion), "emotion")]
[QueryProperty(nameof(Choice), "choice")]
public partial class ListChoice : ContentPage
{
    private readonly IListChoiceService _listChoiceService;
    private string _emotion;
    private string _choice;

    public string Emotion
    {
        get => _emotion;
        set
        {
            _emotion = value;
            UpdateViewModel();
        }
    }

    public string Choice
    {
        get => _choice;
        set
        {
            _choice = value;
            UpdateViewModel();
        }
    }

    public ListChoice(IListChoiceService listChoiceService)
    {
        InitializeComponent();
        _listChoiceService = listChoiceService;
    }

    private void UpdateViewModel()
    {
        if (!string.IsNullOrEmpty(Emotion) && !string.IsNullOrEmpty(Choice))
        {
            BindingContext = new ListChoiceViewModel(_listChoiceService, Emotion, Choice);
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 