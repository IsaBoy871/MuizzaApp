using MuizzaApp1.Contracts.Services;
using MuizzaApp1.ViewModels.Base;

namespace MuizzaApp1.ViewModels
{
    public class GetStarted4PageViewModel : ViewModelBase
    {
        readonly INavigationService _navigationService;

        public Command NavigateCommand => new(async () => await _navigationService.NavigateToPage<GetStartedNamePage>());

        public GetStarted4PageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
} 