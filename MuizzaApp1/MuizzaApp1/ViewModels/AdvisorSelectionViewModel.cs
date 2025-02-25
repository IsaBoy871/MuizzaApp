using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MuizzaApp1.ViewModels
{
    public partial class AdvisorSelectionViewModel : BaseViewModel
    {
        private readonly AdvisorService _advisorService;

        [ObservableProperty]
        private ObservableCollection<Advisor> advisors;

        [ObservableProperty]
        private string feeling;

        public AdvisorSelectionViewModel(
            ISubscriptionService subscriptionService,
            AdvisorService advisorService = null) : base(subscriptionService)
        {
            _advisorService = advisorService ?? new AdvisorService();
            LoadAdvisors();
        }

        private void LoadAdvisors()
        {
            var advisorList = _advisorService.GetAdvisors();
            Advisors = new ObservableCollection<Advisor>(advisorList);
        }
    }
} 