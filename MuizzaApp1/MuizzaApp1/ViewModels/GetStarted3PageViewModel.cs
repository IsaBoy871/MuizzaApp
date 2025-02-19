using MuizzaApp1.Contracts.Services;
using MuizzaApp1.ViewModels.Base;

namespace MuizzaApp1.ViewModels
{
    public class GetStarted3PageViewModel : ViewModelBase
    {
        readonly INavigationService _navigationService;

        public Command NavigateCommand => new(async () => 
        {
            await Shell.Current.GoToAsync(nameof(GetStarted4), new Dictionary<string, object>
            {
                ["DisableTransition"] = true
            });
        });

        public GetStarted3PageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}