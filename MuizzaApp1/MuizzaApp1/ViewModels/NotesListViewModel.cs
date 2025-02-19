using CommunityToolkit.Mvvm.ComponentModel;
using MuizzaApp1.Models;
using MuizzaApp1.Services;
using MuizzaApp1.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace MuizzaApp1.ViewModels;

public partial class NotesListViewModel : ObservableObject
{
    private readonly NotesService _notesService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ObservableCollection<Note> notes;

    public ICommand NavigateToQuotesPage { get; }
    public ICommand NavigateToNotesPage { get; }
    public ICommand NavigateToBrainPage { get; }

    public NotesListViewModel(NotesService notesService, IServiceProvider serviceProvider)
    {
        _notesService = notesService;
        _serviceProvider = serviceProvider;
        Notes = new ObservableCollection<Note>();

        NavigateToQuotesPage = new Command(async () => await Shell.Current.GoToAsync(nameof(QuotesPage), false));
        NavigateToNotesPage = new Command(async () => await Shell.Current.GoToAsync(nameof(NotesPage), false));
        NavigateToBrainPage = new Command(async () => await Shell.Current.GoToAsync(nameof(BrainPage), false));
    }

    public async Task LoadNotesAsync()
    {
        try
        {
            var notesList = await _notesService.GetNotesAsync();
            Notes.Clear();
            foreach (var note in notesList)
            {
                Notes.Add(note);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading notes: {ex.Message}");
            // Handle error appropriately
        }
    }
} 