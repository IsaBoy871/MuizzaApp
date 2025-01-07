namespace MuizzaApp1;

public partial class QuotesPage : ContentPage
{
    private readonly AffirmationDatabase _database;

    public QuotesPage(AffirmationDatabase database)
    {
        InitializeComponent();
        _database = database;
        LoadAffirmations();
    }

    private async void LoadAffirmations()
    {
        var affirmations = await _database.GetAffirmationsAsync();
        AffirmationsList.ItemsSource = affirmations;
    }
}