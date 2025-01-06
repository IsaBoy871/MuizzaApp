using System.Threading.Tasks;

namespace MuizzaApp1.Contracts.Services
{
    public interface INavigationService
    {
        Task NavigateBack();
        Task NavigateToPage<T>(object parameter = null) where T : Page;
    }
}