using MuizzaApp1.ViewModels;
using MuizzaApp1.Services;
using System.Net.Http;
using System.Diagnostics;

namespace MuizzaApp1.Views;

public partial class NotesPage : ContentPage
{
    private readonly NotesService _notesService;

    public NotesPage(NotesService notesService)
    {
        InitializeComponent();
        _notesService = notesService;
        
        // Initialize lines
        for (int i = 0; i < 20; i++)
        {
            linesContainer.Children.Add(new BoxView
            {
                HeightRequest = 1,
                Color = Colors.Black,
                Opacity = 1
            });
        }
    }

    private async void OnDoneClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Done button clicked");

        if (!string.IsNullOrWhiteSpace(editor.Text))
        {
            try
            {
                Debug.WriteLine($"Attempting to save note with content: {editor.Text}");
                bool success = await _notesService.SaveNoteAsync(editor.Text);

                if (success)
                {
                    await DisplayAlert("Success", "Note saved successfully!", "OK");
                    Debug.WriteLine("Note saved successfully");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to save note. Please try again.", "OK");
                    Debug.WriteLine("Failed to save note");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to save note. Please try again.", "OK");
                Debug.WriteLine($"Error saving note: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return;
            }
        }
        else
        {
            Debug.WriteLine("Editor text is empty");
        }

        try
        {
            Debug.WriteLine("Attempting to navigate back");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Navigation error: {ex.Message}");
            await Shell.Current.GoToAsync("..");
        }
    }

    private async void OnQuotesClicked(object sender, EventArgs e)
{
    var quotesPage = Handler.MauiContext.Services.GetService<QuotesPage>();
    await Shell.Current.Navigation.PushAsync(quotesPage);
}

    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        // Calculate how many lines we need based on editor height
        var editorHeight = editor.Height;
        var lineSpacing = 30; // Same as in XAML
        var numberOfLines = (int)(editorHeight / lineSpacing) + 5; // Add extra lines for scrolling

        // Only add more lines if we need them
        while (linesContainer.Children.Count < numberOfLines)
        {
            linesContainer.Children.Add(new BoxView
            {
                HeightRequest = 1,
                Color = Colors.Black,
                Opacity = 1
            });
        }
    }

    private void OnDonePressed(object sender, EventArgs e)
    {
        Debug.WriteLine("Done button pressed");
        if (sender is Button button)
        {
            button.Opacity = 0.5;
            // Reset opacity after a short delay
            Dispatcher.DispatchAsync(async () => 
            {
                await Task.Delay(100);
                button.Opacity = 1;
            });
        }
    }
} 