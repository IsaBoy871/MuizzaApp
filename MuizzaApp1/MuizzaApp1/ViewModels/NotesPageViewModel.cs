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
    public ICommand NavigateToBrainPage { get; }

    public NotesPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        NavigateToQuotesPage = new Command(async () => await Shell.Current.GoToAsync(nameof(QuotesPage), false));
        NavigateToBrainPage = new Command(async () => await Shell.Current.GoToAsync(nameof(BrainPage), false));
    }
} 