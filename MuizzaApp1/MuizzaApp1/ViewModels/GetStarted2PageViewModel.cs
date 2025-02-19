using MuizzaApp1.Contracts.Services;
using MuizzaApp1.ViewModels.Base;

namespace MuizzaApp1.ViewModels
{
    public class GetStarted2PageViewModel : ViewModelBase
    {
        readonly INavigationService _navigationService;

        public Command NavigateCommand => new(async () => 
        {
            await Shell.Current.GoToAsync(nameof(GetStarted3), new Dictionary<string, object>
            {
                ["DisableTransition"] = true
            });
        });

        public GetStarted2PageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}