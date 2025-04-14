using System;
using System.Threading.Tasks;
using MuizzaApp1.Contracts.Services;
using MuizzaApp1.Services;
using System.Diagnostics;

public class SubscriptionService : ISubscriptionService
{
    private readonly IUserService _userService;
    private readonly IPreferences _preferences;
    private const int PREMIUM_MONTHLY_SEARCH_BALANCE = 500;
    private const int PREMIUM_MONTHLY_DEEP_DIVE_BALANCE = 200;
    private const string LAST_BALANCE_RESET_KEY = "last_balance_reset_date";

    public SubscriptionService(IUserService userService, IPreferences preferences)
    {
        _userService = userService;
        _preferences = preferences;
    }

    public async Task<bool> IsPremiumUser()
    {
        var user = await _userService.GetCurrentUserAsync();
        Debug.WriteLine($"IsPremiumUser check - User: {user?.AppleUserId}, SubscriptionTier: {user?.SubscriptionTier}");
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

    public async Task ForceBalanceReset()
    {
        try
        {
            var isPremium = await IsPremiumUser();
            Debug.WriteLine($"ForceBalanceReset - IsPremium: {isPremium}");
            if (!isPremium) return;

            Debug.WriteLine("Forcing balance reset for premium user");
            
            // Set new balances directly without checking previous balances
            var newSearchBalance = PREMIUM_MONTHLY_SEARCH_BALANCE;
            var newDeepDiveBalance = PREMIUM_MONTHLY_DEEP_DIVE_BALANCE;
            
            Debug.WriteLine($"Setting new balances in database - Search: {newSearchBalance}, DeepDive: {newDeepDiveBalance}");

            try
            {
                // Directly set the new balances in the database
                await _userService.UpdateBalancesAsync(newSearchBalance, newDeepDiveBalance);
                Debug.WriteLine($"Successfully updated balances in database");
                
                // Update the reset date
                _preferences.Set(LAST_BALANCE_RESET_KEY, DateTime.UtcNow);
                Debug.WriteLine($"Updated last reset date to: {DateTime.UtcNow}");
            }
            catch (Exception apiEx)
            {
                Debug.WriteLine($"Error updating balances in database: {apiEx}");
                throw;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in ForceBalanceReset: {ex}");
        }
    }

    private async Task CheckAndResetMonthlyBalances()
    {
        try
        {
            var isPremium = await IsPremiumUser();
            Debug.WriteLine($"CheckAndResetMonthlyBalances - IsPremium: {isPremium}");
            if (!isPremium) return;

            var lastResetDate = _preferences.Get(LAST_BALANCE_RESET_KEY, DateTime.MinValue);
            var now = DateTime.UtcNow;
            Debug.WriteLine($"CheckAndResetMonthlyBalances - LastResetDate: {lastResetDate}, Current: {now}");

            // Only reset if we're in a new month
            if (lastResetDate.Month != now.Month || lastResetDate.Year != now.Year)
            {
                Debug.WriteLine($"Monthly reset needed. Last reset: {lastResetDate}, Current: {now}");
                
                // Get current purchased balances before reset
                var currentSearchBalance = await _userService.GetSearchBalanceAsync();
                var currentDeepDiveBalance = await _userService.GetDeepDiveBalanceAsync();
                Debug.WriteLine($"Current balances before reset - Search: {currentSearchBalance}, DeepDive: {currentDeepDiveBalance}");
                
                // Calculate extra purchased CatNips (amount above monthly allocation)
                var extraSearchBalance = Math.Max(0, currentSearchBalance - PREMIUM_MONTHLY_SEARCH_BALANCE);
                var extraDeepDiveBalance = Math.Max(0, currentDeepDiveBalance - PREMIUM_MONTHLY_DEEP_DIVE_BALANCE);
                
                // Set new balances (monthly allocation + any extra purchased CatNips)
                var newSearchBalance = PREMIUM_MONTHLY_SEARCH_BALANCE + extraSearchBalance;
                var newDeepDiveBalance = PREMIUM_MONTHLY_DEEP_DIVE_BALANCE + extraDeepDiveBalance;
                
                Debug.WriteLine($"Setting new balances in database - Search: {newSearchBalance} (including {extraSearchBalance} extra), DeepDive: {newDeepDiveBalance} (including {extraDeepDiveBalance} extra)");

                try
                {
                    // Directly set the new balances in the database
                    await _userService.UpdateBalancesAsync(newSearchBalance, newDeepDiveBalance);
                    Debug.WriteLine($"Successfully updated balances in database");
                    
                    // Only update the reset date after successful database update
                    _preferences.Set(LAST_BALANCE_RESET_KEY, now);
                    Debug.WriteLine($"Updated last reset date to: {now}");
                }
                catch (Exception apiEx)
                {
                    Debug.WriteLine($"Error updating balances in database: {apiEx}");
                    throw;
                }
            }
            else
            {
                Debug.WriteLine("No monthly reset needed - still in same month");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in CheckAndResetMonthlyBalances: {ex}");
        }
    }

    public async Task<bool> CanMakeSearch()
    {
        try
        {
            await CheckAndResetMonthlyBalances();
            var balance = await _userService.GetSearchBalanceAsync();
            return balance > 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in CanMakeSearch: {ex}");
            return false;
        }
    }

    public async Task IncrementSearchCount()
    {
        // This method is kept for interface compatibility but is no longer needed
        return;
    }

    public async Task<int> GetRemainingSearches()
    {
        try
        {
            await CheckAndResetMonthlyBalances();
            var balance = await _userService.GetSearchBalanceAsync();
            Debug.WriteLine($"Current search balance: {balance}");
            return balance;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting search balance: {ex}");
            return 0;
        }
    }
} 