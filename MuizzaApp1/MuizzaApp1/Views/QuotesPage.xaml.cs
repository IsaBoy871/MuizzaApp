using MuizzaApp1.ViewModels;

namespace MuizzaApp1;

public partial class QuotesPage : ContentPage
{
    private string currentAffirmation;
    private QuotesPageViewModel ViewModel => BindingContext as QuotesPageViewModel;
    private bool _isFirstAppearance = true;
    private Image _mainBackdrop;
    private Image _catBackdrop;
    private bool _isPreloaded;

    public string CurrentAffirmation
    {
        get => currentAffirmation;
        set
        {
            currentAffirmation = value;
            OnPropertyChanged();
        }
    }

    public QuotesPage(QuotesPageViewModel viewModel)
    {
        // Set background color immediately to prevent white flash
        BackgroundColor = Color.FromArgb("#f6e5cb");
        
        InitializeComponent();
        BindingContext = viewModel;

        // Start preloading immediately
        MainThread.BeginInvokeOnMainThread(async () => await PreloadAsync());
    }

    private async Task PreloadAsync()
    {
        if (_isPreloaded) return;
        _isPreloaded = true;

        try
        {
            // Store references
            _mainBackdrop = BackdropImage;
            _catBackdrop = CatBackdropImage;

            // Set initial states
            _mainBackdrop.Opacity = 0;
            _catBackdrop.Opacity = 0;
            EmotionsScrollView.Opacity = 0;
            NavigationBar.Opacity = 0;

            // Start loading data in background
            if (ViewModel != null && !ViewModel.IsInitialized)
            {
                await ViewModel.LoadInitialAffirmationsAsync();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Preload error: {ex.Message}");
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        
        // Ensure preload has started
        if (!_isPreloaded)
        {
            MainThread.BeginInvokeOnMainThread(async () => await PreloadAsync());
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isFirstAppearance)
        {
            _isFirstAppearance = false;

            // Ensure preload is complete
            if (!_isPreloaded)
            {
                await PreloadAsync();
            }

            // Show elements with minimal delay
            await Task.WhenAll(
                _mainBackdrop.FadeTo(1, 100),
                _catBackdrop.FadeTo(1, 100)
            );

            EmotionsScrollView.Opacity = 1;
            NavigationBar.Opacity = 1;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        this.Opacity = 0;
        this.Scale = 0.9;
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