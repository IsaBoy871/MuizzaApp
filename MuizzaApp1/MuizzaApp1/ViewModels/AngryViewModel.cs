using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MuizzaApp1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace MuizzaApp1.ViewModels
{
    public partial class AngryViewModel : ObservableObject
    {
        private readonly IListChoiceService _listChoiceService;

        public AngryViewModel(IListChoiceService listChoiceService)
        {
            _listChoiceService = listChoiceService;
        }

        [RelayCommand]
        private async Task NavigateToListChoice(string choice)
        {
            try
            {
                // Pre-fetch the data to ensure it exists
                var listChoice = await _listChoiceService.GetListChoiceByTitleAsync(choice);
                if (listChoice != null)
                {
                    var navigationParameter = new Dictionary<string, object>
                    {
                        { "emotion", "Angry" },
                        { "choice", listChoice.Title }  // Use the exact title from the database
                    };
                    
                    await Shell.Current.GoToAsync($"{nameof(ListChoice)}?emotion=Angry&choice={Uri.EscapeDataString(listChoice.Title)}");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Could not find the selected choice", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
} 