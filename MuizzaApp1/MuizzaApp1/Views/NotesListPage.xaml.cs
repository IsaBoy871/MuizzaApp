using MuizzaApp1.Models;
using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views;

public partial class NotesListPage : ContentPage
{
    private readonly NotesListViewModel _viewModel;

    public NotesListPage(NotesListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        this.Opacity = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.FadeTo(1, 500, Easing.CubicOut);
        await _viewModel.LoadNotesAsync();
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await this.FadeTo(0, 500, Easing.CubicIn);
    }

    private async void OnNoteSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Note selectedNote)
        {
            // Reset selection
            ((CollectionView)sender).SelectedItem = null;
            
            // Navigate to NotesPage with the selected note
            var notesPage = Handler.MauiContext.Services.GetService<NotesPage>();
            notesPage.SetNote(selectedNote);
            await Navigation.PushAsync(notesPage);
        }
    }

    private async void OnAddNewNoteClicked(object sender, EventArgs e)
    {
        var notesPage = Handler.MauiContext.Services.GetService<NotesPage>();
        await Navigation.PushAsync(notesPage);
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            // If search text is cleared, show all notes
            _viewModel.LoadNotesAsync();
        }
        else
        {
            // Filter notes based on search text
            var searchText = e.NewTextValue.ToLower();
            var filteredNotes = _viewModel.Notes
                .Where(note => note.Content.ToLower().Contains(searchText))
                .ToList();
            
            _viewModel.Notes.Clear();
            foreach (var note in filteredNotes)
            {
                _viewModel.Notes.Add(note);
            }
        }
    }
} 