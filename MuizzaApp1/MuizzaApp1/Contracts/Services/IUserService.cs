using MuizzaApp1.Models;

namespace MuizzaApp1.Contracts.Services
{
    public interface IUserService
    {
        Task<User> GetCurrentUserAsync();
        Task<User> GetUserByAppleIdAsync(string appleUserId);
        Task<User> CreateUserAsync(string appleUserId, string email);
        Task UpdateSubscriptionTierAsync(string appleUserId, string tier);
        Task UpdateTrialStatusAsync(string appleUserId, bool hasTrial);
        
        // Balance management methods
        Task<int> GetSearchBalanceAsync();
        Task<int> GetDeepDiveBalanceAsync();
        Task<int> DecrementSearchBalanceAsync();
        Task<int> DecrementDeepDiveBalanceAsync();
        Task<int> AddSearchBalanceAsync(int amount);
        Task<int> AddDeepDiveBalanceAsync(int amount);
        Task UpdateBalancesAsync(int searchBalance, int deepDiveBalance);
    }
} 