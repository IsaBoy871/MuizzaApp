using MuizzaApp1.ViewModels;
using MuizzaApp1.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace MuizzaApp1;

public partial class EmotionalResponsePage : ContentPage
{
    public EmotionalResponsePage(
        string feeling, 
        IOpenAIService openAIService = null, 
        ResponseCacheService cacheService = null,
        ILogger<EmotionalResponseViewModel> logger = null)
    {
        InitializeComponent();
        
        var services = Application.Current.Handler.MauiContext.Services;
        openAIService ??= services.GetService<IOpenAIService>();
        cacheService ??= services.GetService<ResponseCacheService>();
        logger ??= services.GetService<ILogger<EmotionalResponseViewModel>>();
        
        BindingContext = new EmotionalResponseViewModel(openAIService, cacheService, logger);
        
        Task.Run(async () => await ((EmotionalResponseViewModel)BindingContext).GenerateResponse(feeling));
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