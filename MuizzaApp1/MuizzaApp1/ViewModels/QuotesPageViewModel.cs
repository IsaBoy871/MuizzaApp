using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MuizzaApp1.Models;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using MuizzaApp1.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace MuizzaApp1.ViewModels;

[ObservableObject]
public partial class QuotesPageViewModel
{
    private readonly AffirmationsService _affirmationsService;
    private readonly IServiceProvider _serviceProvider;
    
    [ObservableProperty]
    private ObservableCollection<Affirmation> affirmations;
    
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private ObservableCollection<Emotion> emotionsList;

    [ObservableProperty]
    private string feelingText;

    public Command NavigateToNotesPage { get; }

    public Command NavigateToBrainPage { get; }
    public ICommand SubmitFeelingCommand { get; }

    public QuotesPageViewModel(AffirmationsService affirmationsService, IServiceProvider serviceProvider)
    {
        Debug.WriteLine("ViewModel constructor started");
        _affirmationsService = affirmationsService;
        _serviceProvider = serviceProvider;
        Affirmations = new ObservableCollection<Affirmation>();
        InitializeEmotions();
        Debug.WriteLine($"EmotionsList count: {EmotionsList?.Count ?? 0}");
        LoadAffirmationsAsync().ConfigureAwait(false);
        NavigateToNotesPage = new Command(async () => await OnNavigateToNotesPage());
        NavigateToBrainPage =new Command(async () => await OnNavigateToBrainPage());
        SubmitFeelingCommand = new Command(async () => await OnFeelingSubmitted());
    }

    private void InitializeEmotions()
    {
        EmotionsList = new ObservableCollection<Emotion>
        {
            new Emotion { Name = "Angry", ImageSource = "angry_woman.png", Color = Color.FromArgb("#FF6B6B") },
            new Emotion { Name = "Anxious", ImageSource = "anxious_woman.png", Color = Color.FromArgb("#7d60cb") },
            new Emotion { Name = "Bored", ImageSource = "bored_woman.png", Color = Color.FromArgb("#7c82ff") },
            new Emotion { Name = "Depressed", ImageSource = "depressed_woman.png", Color = Color.FromArgb("#89888d") },
            new Emotion { Name = "Stressed", ImageSource = "stressed.png", Color = Color.FromArgb("#FFA94D") },
            new Emotion { Name = "Overwhelmed", ImageSource = "overwhelmed.png", Color = Color.FromArgb("#F783AC") },
            new Emotion { Name = "Insecure", ImageSource = "insecure.png", Color = Color.FromArgb("#8E6B4E") },
            new Emotion { Name = "Lonely", ImageSource = "lonely.png", Color = Color.FromArgb("#7950F2") },
        };
    }

    public async Task LoadAffirmationsAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            var loadedAffirmations = await _affirmationsService.GetAffirmationsAsync();
            
            Affirmations.Clear();
            foreach (var affirmation in loadedAffirmations)
            {
                Affirmations.Add(affirmation);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading affirmations: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task OnNavigateToNotesPage()
    {
        var notesPage = _serviceProvider.GetService<NotesPage>();
        await Shell.Current.Navigation.PushAsync(notesPage);
    }

    private async Task OnNavigateToBrainPage()
    {

        var brainPage = _serviceProvider.GetService<BrainPage>();
        await Shell.Current.Navigation.PushAsync(brainPage);
    }

    private async Task OnFeelingSubmitted()
    {
        if (!string.IsNullOrWhiteSpace(FeelingText))
        {
            await Application.Current.MainPage.Navigation.PushAsync(new EmotionalResponsePage(FeelingText));
            FeelingText = string.Empty; // Clear the entry
        }
    }
}