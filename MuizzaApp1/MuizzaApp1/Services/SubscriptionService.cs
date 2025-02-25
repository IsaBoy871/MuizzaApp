using System;
using System.Threading.Tasks;
using MuizzaApp1.Contracts.Services;
using MuizzaApp1.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IUserService _userService;
    private readonly IPreferences _preferences;
    private const string SEARCH_COUNT_KEY = "daily_search_count";
    private const string LAST_SEARCH_DATE_KEY = "last_search_date";

    public SubscriptionService(IUserService userService, IPreferences preferences)
    {
        _userService = userService;
        _preferences = preferences;
    }

    public async Task<bool> IsPremiumUser()
    {
        var user = await _userService.GetCurrentUserAsync();
        return user?.SubscriptionTier == "Premium";
    }

    public async Task<string> GetSubscriptionTier()
    {
        var user = await _userService.GetCurrentUserAsync();
        return user?.SubscriptionTier ?? "Free";
    }

    public async Task<DateTime?> GetSubscriptionEndDate()
    {
        var user = await _userService.GetCurrentUserAsync();
        return user?.SubscriptionEndDate;
    }

    public async Task<bool> CanMakeSearch()
    {
        var isPremium = await IsPremiumUser();
        var maxSearches = isPremium ? 20 : 2;
        
        var lastSearchDate = _preferences.Get(LAST_SEARCH_DATE_KEY, DateTime.MinValue);
        var today = DateTime.Today;
        
        if (lastSearchDate.Date != today)
        {
            // Reset counter for new day
            _preferences.Set(SEARCH_COUNT_KEY, 0);
            _preferences.Set(LAST_SEARCH_DATE_KEY, today);
            return true;
        }

        var searchCount = _preferences.Get(SEARCH_COUNT_KEY, 0);
        return searchCount < maxSearches;
    }

    public async Task IncrementSearchCount()
    {
        var currentCount = _preferences.Get(SEARCH_COUNT_KEY, 0);
        _preferences.Set(SEARCH_COUNT_KEY, currentCount + 1);
    }

    public async Task<int> GetRemainingSearches()
    {
        var isPremium = await IsPremiumUser();
        var maxSearches = isPremium ? 20 : 2;
        var currentCount = _preferences.Get(SEARCH_COUNT_KEY, 0);
        return Math.Max(0, maxSearches - currentCount);
    }
} 