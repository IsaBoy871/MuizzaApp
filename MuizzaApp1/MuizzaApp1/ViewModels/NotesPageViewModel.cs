using CommunityToolkit.Mvvm.ComponentModel;
using MuizzaApp1.Views;
using System.Windows.Input;

namespace MuizzaApp1.ViewModels;

[ObservableObject]
public partial class NotesPageViewModel
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private bool isSaving;

    public ICommand NavigateToQuotesPage { get; }
    public ICommand NavigateToNotesPage { get; }
    public ICommand NavigateToBrainPage { get; }

    public NotesPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        NavigateToQuotesPage = new Command(async () => await Shell.Current.GoToAsync(nameof(QuotesPage)));
        NavigateToNotesPage = new Command(async () => await Shell.Current.GoToAsync(nameof(NotesPage)));
        NavigateToBrainPage = new Command(async () => await Shell.Current.GoToAsync(nameof(BrainPage)));
    }
} 