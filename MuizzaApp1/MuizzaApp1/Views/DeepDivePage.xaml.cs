using Microsoft.Extensions.Logging;
using MuizzaApp1.ViewModels;

namespace MuizzaApp1;

[QueryProperty(nameof(Feeling), "feeling")]
[QueryProperty(nameof(AdvisorSystemPrompt), "advisorSystemPrompt")]
public partial class DeepDivePage : ContentPage
{
    private string _feeling;
    private string _advisorSystemPrompt;
    private readonly IOpenAIService _openAIService;
    private readonly ILogger<DeepDiveViewModel> _logger;
    private readonly ISubscriptionService _subscriptionService;
    private DeepDiveViewModel _viewModel;

    public string Feeling
    {
        get => _feeling;
        set
        {
            System.Diagnostics.Debug.WriteLine($"Setting Feeling: {value}");
            _feeling = value;
            LoadContent();
        }
    }

    public string AdvisorSystemPrompt
    {
        get => _advisorSystemPrompt;
        set
        {
            System.Diagnostics.Debug.WriteLine($"Setting AdvisorSystemPrompt: {value}");
            _advisorSystemPrompt = value;
            LoadContent();
        }
    }

    public DeepDivePage(
        IOpenAIService openAIService,
        ILogger<DeepDiveViewModel> logger,
        ISubscriptionService subscriptionService)
    {
        System.Diagnostics.Debug.WriteLine("DeepDivePage constructor called");
        InitializeComponent();
        _openAIService = openAIService;
        _logger = logger;
        _subscriptionService = subscriptionService;
    }

    private async void LoadContent()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("LoadContent called");
            System.Diagnostics.Debug.WriteLine($"Feeling: {_feeling}");
            System.Diagnostics.Debug.WriteLine($"AdvisorSystemPrompt: {_advisorSystemPrompt}");

            if (!string.IsNullOrEmpty(_feeling) && !string.IsNullOrEmpty(_advisorSystemPrompt))
            {
                System.Diagnostics.Debug.WriteLine("Creating ViewModel");
                _viewModel = new DeepDiveViewModel(
                    _openAIService, 
                    _logger, 
                    _subscriptionService, 
                    _feeling,
                    _advisorSystemPrompt);
                
                System.Diagnostics.Debug.WriteLine("Setting BindingContext");
                BindingContext = _viewModel;
                
                System.Diagnostics.Debug.WriteLine("Initializing ViewModel");
                await _viewModel.InitializeAsync();
                System.Diagnostics.Debug.WriteLine("ViewModel Initialized");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Feeling or AdvisorSystemPrompt is empty");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in LoadContent: {ex}");
            _logger?.LogError(ex, "Error in LoadContent");
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
} 