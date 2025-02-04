using MuizzaApp1.ViewModels;

namespace MuizzaApp1;

public partial class QuotesPage : ContentPage
{
    private string currentAffirmation;
    public string CurrentAffirmation
    {
        get => currentAffirmation;
        set
        {
            currentAffirmation = value;
            OnPropertyChanged();
        }
    }

    private QuotesPageViewModel ViewModel => BindingContext as QuotesPageViewModel;

    public QuotesPage(QuotesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        // Set initial affirmation
        CurrentAffirmation = "I am just a test quote. To show what it looks like.";
    }

    private void OnFeelingEntered(object sender, EventArgs e)
    {
        if (BindingContext is QuotesPageViewModel viewModel)
        {
            viewModel.SubmitFeelingCommand.Execute(null);
        }
    }

    private void OnPositionChanged(object sender, PositionChangedEventArgs e)
    {
        ViewModel?.PositionChangedCommand.Execute(e.CurrentPosition);
    }

    private void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        if (BindingContext is QuotesPageViewModel viewModel)
        {
            viewModel.OnScrolled(e);
        }
    }
}