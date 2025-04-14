using System.Threading.Tasks;
using MuizzaApp1.Models;

namespace MuizzaApp1.Contracts.Services
{
    public interface IInAppPurchaseService
    {
        Task<PurchaseResult> PurchaseAsync(string productId);
    }
} 