using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views
{
    [QueryProperty(nameof(Feeling), "feeling")]
    public partial class AdvisorSelectionPage : ContentPage
    {
        private readonly AdvisorSelectionViewModel _viewModel;
        private string _feeling;

        public string Feeling
        {
            get => _feeling;
            set
            {
                _feeling = value;
                InitializeViewModel();
            }
        }

        public AdvisorSelectionPage(ISubscriptionService subscriptionService)
        {
            InitializeComponent();
            _viewModel = new AdvisorSelectionViewModel(subscriptionService);
            BindingContext = _viewModel;
        }

        private void InitializeViewModel()
        {
            if (!string.IsNullOrEmpty(_feeling))
            {
                _viewModel.Feeling = _feeling;
            }
        }

        private async void OnAdvisorSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Advisor selectedAdvisor)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"Selected Advisor: {selectedAdvisor.Name}");
                    System.Diagnostics.Debug.WriteLine($"Current Feeling: {_feeling}");
                    System.Diagnostics.Debug.WriteLine($"System Prompt: {selectedAdvisor.SystemPrompt}");

                    // URI encode both parameters
                    var encodedFeeling = Uri.EscapeDataString(_feeling);
                    var encodedPrompt = Uri.EscapeDataString(selectedAdvisor.SystemPrompt);

                    var navigationString = $"DeepDivePage?feeling={encodedFeeling}&advisorSystemPrompt={encodedPrompt}";
                    System.Diagnostics.Debug.WriteLine($"Navigation String: {navigationString}");

                    await Shell.Current.GoToAsync(navigationString);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Navigation Error: {ex}");
                    await Shell.Current.DisplayAlert("Error", "Navigation failed", "OK");
                }
            }
        }
    }
} 