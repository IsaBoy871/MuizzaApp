using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MuizzaApp1.Contracts.Services;
using MuizzaApp1.Models;

namespace MuizzaApp1.ViewModels
{
    public partial class AdvisorSelectionViewModel : BaseViewModel
    {
        private readonly AdvisorService _advisorService;
        private readonly IUserService _userService;

        [ObservableProperty]
        private ObservableCollection<Advisor> advisors;

        [ObservableProperty]
        private string feeling;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private int deepDiveBalance;

        public AdvisorSelectionViewModel(
            ISubscriptionService subscriptionService,
            AdvisorService advisorService,
            IUserService userService) : base(subscriptionService)
        {
            _advisorService = advisorService;
            _userService = userService;
            LoadAdvisorsAsync();
            UpdateDeepDiveBalance();
        }

        private async void LoadAdvisorsAsync()
        {
            try
            {
                IsLoading = true;
                var advisorList = await _advisorService.GetAdvisorsAsync();
                Advisors = new ObservableCollection<Advisor>(advisorList);
            }
            catch (Exception ex)
            {
                // Handle or log error appropriately
                System.Diagnostics.Debug.WriteLine($"Error loading advisors: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void UpdateDeepDiveBalance()
        {
            DeepDiveBalance = await _userService.GetDeepDiveBalanceAsync();
        }

        public async Task DecrementDeepDiveBalanceAsync()
        {
            DeepDiveBalance = await _userService.DecrementDeepDiveBalanceAsync();
        }
    }
} 