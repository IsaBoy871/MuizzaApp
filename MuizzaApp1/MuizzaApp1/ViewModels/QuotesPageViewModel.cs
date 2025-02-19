using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MuizzaApp1.Models;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using MuizzaApp1.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Linq;
using System.Collections.Generic;
using System;

namespace MuizzaApp1.ViewModels;

[ObservableObject]
public partial class QuotesPageViewModel
{
    private readonly IAffirmationsService _affirmationsService;
    private readonly IServiceProvider _serviceProvider;
    private List<Affirmation> _allAffirmations;
    private Random _random = new Random();
    private const int BatchSize = 3;
    private const int MaxVisibleItems = 10; // Reduced from 15
    private int _currentIndex = 0;
    
    [ObservableProperty]
    private ObservableCollection<Affirmation> affirmations;
    
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private ObservableCollection<Emotion> emotionsList;

    [ObservableProperty]
    private string feelingText;

    [ObservableProperty]
    private string currentAffirmation;

    [ObservableProperty]
    private int currentPosition;

    private bool _isLoadingMore;

    [ObservableProperty]
    private bool isInitialized;

    public Command NavigateToNotesPage { get; }

    public Command NavigateToBrainPage { get; }
    public ICommand SubmitFeelingCommand { get; }

    public QuotesPageViewModel(IAffirmationsService affirmationsService, IServiceProvider serviceProvider)
    {
        Debug.WriteLine("ViewModel constructor started");
        _affirmationsService = affirmationsService;
        _serviceProvider = serviceProvider;
        Affirmations = new ObservableCollection<Affirmation>();
        _allAffirmations = new List<Affirmation>();
        InitializeEmotions();
        
        // Initialize commands
        NavigateToNotesPage = new Command(async () => await OnNavigateToNotesPage());
        NavigateToBrainPage = new Command(async () => await OnNavigateToBrainPage());
        SubmitFeelingCommand = new Command(async () => await OnFeelingSubmitted());
        
        // Remove any test data initialization
        MainThread.BeginInvokeOnMainThread(async () => await LoadInitialAffirmationsAsync());
        Debug.WriteLine("ViewModel constructor completed");
    }

    private void InitializeEmotions()
    {
        EmotionsList = new ObservableCollection<Emotion>
        {
            new Emotion { Name = "Angry", ImageSource = "angry_woman.png", Color = Color.FromArgb("#FF6B6B") },
            new Emotion { Name = "Anxious", ImageSource = "anxious_woman.png", Color = Color.FromArgb("#7d60cb") },
            new Emotion { Name = "Bored", ImageSource = "bored_woman.png", Color = Color.FromArgb("#7c82ff") },
            new Emotion { Name = "Depressed", ImageSource = "depressed_woman.png", Color = Color.FromArgb("#89888d") },
            new Emotion { Name = "Inspired", ImageSource = "inspired.png", Color = Color.FromArgb("#ff6fb0") },
            new Emotion { Name = "Grateful", ImageSource = "grateful.png", Color = Color.FromArgb("#82b28d") },
            new Emotion { Name = "Restless", ImageSource = "restless.png", Color = Color.FromArgb("#cda2a2") },
            new Emotion { Name = "Insecure", ImageSource = "insecure.png", Color = Color.FromArgb("#9378ff") },
            new Emotion { Name = "Hopeful", ImageSource = "hopeful.png", Color = Color.FromArgb("#ff6fb0") },
            new Emotion { Name = "Content", ImageSource = "content.png", Color = Color.FromArgb("#ebd69a") },
            new Emotion { Name = "Confident", ImageSource = "confident.png", Color = Color.FromArgb("#ffcb8d") },
            new Emotion { Name = "Motivated", ImageSource = "motivated.png", Color = Color.FromArgb("#8aeaff") },
        };
    }

    public async Task LoadInitialAffirmationsAsync()
    {
        if (IsInitialized) return;
        
        try
        {
            IsLoading = true;
            _allAffirmations = await Task.Run(() => _affirmationsService.GetAffirmationsAsync());
            
            // Shuffle the list
            _allAffirmations = _allAffirmations.OrderBy(x => _random.Next()).ToList();
            
            // Load initial batch
            await LoadNextBatchAsync();
            
            IsInitialized = true;
            Debug.WriteLine($"Loaded initial batch: {Affirmations.Count} affirmations");
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

    private async Task LoadNextBatchAsync()
    {
        try
        {
            var currentCount = Affirmations.Count;
            var remainingItems = _allAffirmations.Skip(currentCount).Take(BatchSize);

            if (!remainingItems.Any()) return;

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                foreach (var item in remainingItems)
                {
                    Affirmations.Add(item);
                    Debug.WriteLine($"Added affirmation: {item.Text}");
                }
            });

            // If we've used all items, reshuffle and start over
            if (currentCount + BatchSize >= _allAffirmations.Count)
            {
                _allAffirmations = _allAffirmations.OrderBy(x => _random.Next()).ToList();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in LoadNextBatchAsync: {ex.Message}");
        }
    }

    private Task CleanupOldItemsAsync()
    {
        if (Affirmations.Count <= MaxVisibleItems) return Task.CompletedTask;

        return MainThread.InvokeOnMainThreadAsync(() =>
        {
            try
            {
                var currentPos = CurrentPosition;
                var itemsToRemove = new List<Affirmation>();
                
                // Keep a window of items around the current position
                var minKeepIndex = Math.Max(0, currentPos - 3);
                var maxKeepIndex = Math.Min(Affirmations.Count - 1, currentPos + 3);

                for (int i = 0; i < Affirmations.Count; i++)
                {
                    if (i < minKeepIndex || i > maxKeepIndex)
                    {
                        itemsToRemove.Add(Affirmations[i]);
                    }
                }

                foreach (var item in itemsToRemove)
                {
                    if (Affirmations.Contains(item))
                    {
                        Affirmations.Remove(item);
                    }
                }

                Debug.WriteLine($"Cleaned up {itemsToRemove.Count} items. Current count: {Affirmations.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during cleanup: {ex.Message}");
            }
        });
    }

    [RelayCommand]
    private async Task PositionChanged(int position)
    {
        CurrentPosition = position;
        
        // Load more items when we're near the end
        if (position >= Affirmations.Count - 2)
        {
            await LoadNextBatchAsync();
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
        try
        {
            if (string.IsNullOrWhiteSpace(FeelingText))
                return;

            var currentFeeling = FeelingText;
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    if (Shell.Current?.Navigation == null)
                    {
                        Logger.Log("Shell.Current.Navigation is null");
                        throw new InvalidOperationException("Navigation is not available");
                    }

                    var emotionalResponsePage = new EmotionalResponsePage(currentFeeling);
                    await Shell.Current.Navigation.PushAsync(emotionalResponsePage);
                    FeelingText = string.Empty;
                }
                catch (Exception innerEx)
                {
                    Logger.Log($"Navigation error: {innerEx.Message}\n{innerEx.StackTrace}");
                    await Shell.Current.DisplayAlert(
                        "Error",
                        "Unable to process your request. Please restart the app.",
                        "OK");
                }
            });
        }
        catch (Exception ex)
        {
            Logger.Log($"Outer error in OnFeelingSubmitted: {ex.Message}\n{ex.StackTrace}");
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.DisplayAlert(
                    "Error",
                    "Something went wrong. Please try again.",
                    "OK");
            });
        }
    }

    // Add a method to handle the scrolled event
    public async Task OnScrolled(ItemsViewScrolledEventArgs args)
    {
        if (_isLoadingMore) return;

        try
        {
            // Load more items when we're near the end
            if (args.LastVisibleItemIndex >= Affirmations.Count - 2)
            {
                _isLoadingMore = true;
                await LoadNextBatchAsync();
            }
        }
        finally
        {
            _isLoadingMore = false;
        }
    }

    public async Task InitializeAsync()
    {
        // Load initial batch of affirmations
        await LoadNextBatchAsync();
    }

    [RelayCommand]
    private async Task NavigateToEmotion(string emotion)
    {
        switch (emotion.ToLower())
        {
            case "angry":
                await Shell.Current.GoToAsync(nameof(Angry));
                break;
            case "anxious":
                await Shell.Current.GoToAsync(nameof(Anxious));
                break;
            case "bored":
                await Shell.Current.GoToAsync(nameof(Bored));
                break;
            case "depressed":
                await Shell.Current.GoToAsync(nameof(Depressed));
                break;
            case "inspired":
                await Shell.Current.GoToAsync(nameof(Inspired));
                break;
            case "grateful":
                await Shell.Current.GoToAsync(nameof(Grateful));
                break;
            case "restless":
                await Shell.Current.GoToAsync(nameof(Restless));
                break;
            case "insecure":
                await Shell.Current.GoToAsync(nameof(Insecure));
                break;
            case "hopeful":
                await Shell.Current.GoToAsync(nameof(Hopeful));
                break;
            case "content":
                await Shell.Current.GoToAsync(nameof(Content));
                break;
            case "confident":
                await Shell.Current.GoToAsync(nameof(Confident));
                break;
            case "motivated":
                await Shell.Current.GoToAsync(nameof(Motivated));
                break;
        }
    }
}