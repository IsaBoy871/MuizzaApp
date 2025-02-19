using MuizzaApp1.ViewModels;
using MuizzaApp1.Services;
using System.Net.Http;
using System.Diagnostics;
using MuizzaApp1.Models;

namespace MuizzaApp1.Views;

public partial class NotesPage : ContentPage
{
    private readonly NotesService _notesService;
    private readonly NotesPageViewModel _viewModel;
    private bool _isSaving = false;
    private Note _currentNote;

    public NotesPage(NotesService notesService, NotesPageViewModel viewModel)
    {
        InitializeComponent();
        _notesService = notesService;
        _viewModel = viewModel;
        BindingContext = viewModel;
        this.Opacity = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.FadeTo(1, 500, Easing.CubicOut);
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await this.FadeTo(0, 500, Easing.CubicIn);
    }

    public void SetNote(Note note)
    {
        _currentNote = note;
        editor.Text = note.Content;
    }

    private async void OnNotesClicked(object sender, EventArgs e)
    {
        var notesListPage = Handler.MauiContext.Services.GetService<NotesListPage>();
        await Navigation.PushAsync(notesListPage);
    }

    private async void OnDoneClicked(object sender, EventArgs e)
    {
        if (_isSaving) return;
        
        try
        {
            _isSaving = true;
            
            if (string.IsNullOrWhiteSpace(editor.Text))
            {
                await Navigation.PopAsync();
                return;
            }

            bool success;
            if (_currentNote != null)
            {
                _currentNote.Content = editor.Text;
                success = await _notesService.UpdateNoteAsync(_currentNote);
            }
            else
            {
                success = await _notesService.SaveNoteAsync(editor.Text);
            }
            
            if (success)
            {
                await DisplayAlert("Success", "Note saved successfully!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to save note. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving note: {ex.Message}");
            await DisplayAlert("Error", "An unexpected error occurred.", "OK");
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async void OnQuotesClicked(object sender, EventArgs e)
    {
        var quotesPage = Handler.MauiContext.Services.GetService<QuotesPage>();
        await Shell.Current.Navigation.PushAsync(quotesPage);
    }

    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        // Editor text changed event handler remains empty as we no longer need to manage lines
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