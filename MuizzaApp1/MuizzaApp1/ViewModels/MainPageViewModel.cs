using MuizzaApp1.Contracts.Services;
using MuizzaApp1.ViewModels.Base;

namespace MuizzaApp1.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public Command NavigateCommand => new(async () => await _navigationService.NavigateToPage<GetStarted2>());

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}