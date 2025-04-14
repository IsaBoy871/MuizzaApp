using MuizzaApp1.ViewModels;
using MuizzaApp1.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace MuizzaApp1;

public partial class EmotionalResponsePage : ContentPage
{
    private readonly EmotionalResponseViewModel _viewModel;

    public EmotionalResponsePage(
        string feeling, 
        IOpenAIService openAIService = null, 
        ResponseCacheService cacheService = null,
        ILogger<EmotionalResponseViewModel> logger = null,
        ISubscriptionService subscriptionService = null,
        IPaymentService paymentService = null,
        IUserService userService = null)
    {
        InitializeComponent();
        
        Debug.WriteLine("EmotionalResponsePage constructor started");
        
        var services = Application.Current.Handler.MauiContext.Services;
        openAIService ??= services.GetService<IOpenAIService>();
        cacheService ??= services.GetService<ResponseCacheService>();
        logger ??= services.GetService<ILogger<EmotionalResponseViewModel>>();
        subscriptionService ??= services.GetService<ISubscriptionService>();
        paymentService ??= services.GetService<IPaymentService>();
        userService ??= services.GetService<IUserService>();
        
        _viewModel = new EmotionalResponseViewModel(
            openAIService, 
            cacheService, 
            logger,
            subscriptionService,
            paymentService,
            userService);
        
        Debug.WriteLine("Setting BindingContext");
        BindingContext = _viewModel;
        Debug.WriteLine($"InitiateApplePayCommand is null: {_viewModel.InitiateApplePayCommand == null}");
        
        // Store the feeling before generating response
        _viewModel.CurrentFeeling = feeling;
        
        // Generate response
        MainThread.BeginInvokeOnMainThread(async () => 
            await _viewModel.GenerateResponse(feeling));
            
        Debug.WriteLine("EmotionalResponsePage constructor completed");
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///QuotesPage");
    }

    protected override bool OnBackButtonPressed()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync("///QuotesPage");
        });
        return true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is EmotionalResponseViewModel viewModel)
        {
            viewModel.UpdateSearchBalance();
        }
    }
} 