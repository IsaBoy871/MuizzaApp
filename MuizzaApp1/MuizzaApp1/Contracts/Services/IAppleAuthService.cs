using System.Threading.Tasks;
using MuizzaApp1.Services;

namespace MuizzaApp1.Contracts.Services
{
    public interface IAppleAuthService
    {
        Task<AuthenticationResult> AuthenticateAsync();
    }
} 