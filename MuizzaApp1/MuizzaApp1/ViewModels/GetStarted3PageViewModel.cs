using MuizzaApp1.Contracts.Services;
using MuizzaApp1.ViewModels.Base;
using MuizzaApp1.Views;

namespace MuizzaApp1.ViewModels
{
    public class GetStarted3PageViewModel : ViewModelBase
    {
        readonly INavigationService _navigationService;

        public Command NavigateCommand => new(async () => await _navigationService.NavigateToPage<QuotesPage>());

        public GetStarted3PageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}