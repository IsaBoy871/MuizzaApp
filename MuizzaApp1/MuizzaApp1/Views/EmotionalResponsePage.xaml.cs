using MuizzaApp1.ViewModels;
using MuizzaApp1.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace MuizzaApp1;

public partial class EmotionalResponsePage : ContentPage
{
    private readonly EmotionalResponseViewModel _viewModel;

    public EmotionalResponsePage(
        string feeling, 
        IOpenAIService openAIService = null, 
        ResponseCacheService cacheService = null,
        ILogger<EmotionalResponseViewModel> logger = null,
        ISubscriptionService subscriptionService = null)
    {
        InitializeComponent();
        
        var services = Application.Current.Handler.MauiContext.Services;
        openAIService ??= services.GetService<IOpenAIService>();
        cacheService ??= services.GetService<ResponseCacheService>();
        logger ??= services.GetService<ILogger<EmotionalResponseViewModel>>();
        subscriptionService ??= services.GetService<ISubscriptionService>();
        
        _viewModel = new EmotionalResponseViewModel(
            openAIService, 
            cacheService, 
            logger,
            subscriptionService);
        
        BindingContext = _viewModel;
        
        // Store the feeling before generating response
        _viewModel.CurrentFeeling = feeling;
        
        // Generate response
        MainThread.BeginInvokeOnMainThread(async () => 
            await _viewModel.GenerateResponse(feeling));
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("QuotesPage");
    }

    protected override bool OnBackButtonPressed()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync("QuotesPage");
        });
        return true;
    }
} 