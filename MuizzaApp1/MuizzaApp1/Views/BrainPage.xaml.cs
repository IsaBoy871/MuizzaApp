using Microsoft.Maui.Controls;
using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views;

public partial class BrainPage : ContentPage
{
    private BrainPageViewModel ViewModel => BindingContext as BrainPageViewModel;

    public BrainPage(BrainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override bool OnBackButtonPressed()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync("..");
        });
        return true;
    }

    private async void OnNotificationClicked(object sender, EventArgs e)
    {
        if (ViewModel != null)
        {
            await ViewModel.SaveDailyEntryAsync();
        }
    }

    private async Task ShowCompletedState()
    {
        // Shrink notification icon
        await Task.WhenAll(
            NotificationIcon.ScaleTo(0, 250, Easing.CubicOut),
            NotificationIcon.FadeTo(0, 250, Easing.CubicOut)
        );
        NotificationIcon.IsVisible = false;

        // Show and expand complete icon
        CompleteIcon.IsVisible = true;
        CompleteIcon.Scale = 0;
        CompleteIcon.Opacity = 1;
        CompleteIcon.HeightRequest = 50;
        CompleteIcon.WidthRequest = 50;
        await CompleteIcon.ScaleTo(1, 250, Easing.CubicOut);

        // Show popup with animation
        PopupOverlay.Opacity = 0;
        PopupOverlay.IsVisible = true;
        await PopupOverlay.FadeTo(1, 250, Easing.CubicOut);
    }

    private async void OnStartDayClicked(object sender, EventArgs e)
    {
        // Hide popup with animation
        await PopupOverlay.FadeTo(0, 250, Easing.CubicOut);
        PopupOverlay.IsVisible = false;
    }

    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        // The ViewModel will automatically set IsCompleted to false when text changes
        // due to our property setters
    }
} 