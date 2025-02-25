public interface ISubscriptionService
{
    Task<bool> IsPremiumUser();
    Task<string> GetSubscriptionTier();
    Task<DateTime?> GetSubscriptionEndDate();
    Task<bool> CanMakeSearch();
    Task IncrementSearchCount();
    Task<int> GetRemainingSearches();
} 