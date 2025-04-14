using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using MuizzaApp1.ViewModels;
using MuizzaApp1.Services;
using MuizzaApp1.Contracts.Services;

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

        public AdvisorSelectionPage(
            ISubscriptionService subscriptionService,
            AdvisorService advisorService,
            IUserService userService)
        {
            InitializeComponent();
            _viewModel = new AdvisorSelectionViewModel(subscriptionService, advisorService, userService);
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
                    // Check if user has enough deep dives
                    if (_viewModel.DeepDiveBalance <= 0)
                    {
                        await Shell.Current.DisplayAlert(
                            "No Deep Dives Left", 
                            "You don't have any Deep Dives remaining. Please purchase more to continue.", 
                            "OK");
                        return;
                    }

                    // Decrement the balance
                    await _viewModel.DecrementDeepDiveBalanceAsync();

                    // URI encode both parameters
                    var encodedFeeling = Uri.EscapeDataString(_viewModel.Feeling);
                    var encodedPrompt = Uri.EscapeDataString(selectedAdvisor.SystemPrompt);

                    // Navigate to deep dive page with proper parameters
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

        private async void OnReturnToQuotesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///QuotesPage");
        }
    }
} 