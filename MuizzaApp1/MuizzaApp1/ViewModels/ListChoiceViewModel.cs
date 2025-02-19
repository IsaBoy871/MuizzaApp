using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Json;
using MuizzaApp1.Models;
using MuizzaApp1.Services;

namespace MuizzaApp1.ViewModels;

public partial class ListChoiceViewModel : ObservableObject
{
    private readonly IListChoiceService _listChoiceService;
    
    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string affirmationText;

    [ObservableProperty]
    private string thoughtsText;

    [ObservableProperty]
    private bool isLoading;

    public ListChoiceViewModel(IListChoiceService listChoiceService, string emotion, string choice)
    {
        _listChoiceService = listChoiceService;
        Title = choice;
        LoadAffirmationData(choice);
    }

    private async Task LoadAffirmationData(string choice)
    {
        try
        {
            IsLoading = true;
            
            var listChoice = await _listChoiceService.GetListChoiceByTitleAsync(choice);
            
            if (listChoice != null)
            {
                AffirmationText = listChoice.Affirmation;
                ThoughtsText = listChoice.Thoughts;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load affirmation", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
}

public class AffirmationData
{
    public string Text { get; set; }
    // Add other properties from your API response
} 