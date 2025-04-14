using MuizzaApp1.Models;

namespace MuizzaApp1.Contracts.Services
{
    public interface IApplePayService
    {
        Task<PaymentResult> ProcessPaymentAsync(decimal amount, string description);
    }
} 