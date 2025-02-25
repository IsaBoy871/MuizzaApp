namespace MuizzaApp1.Contracts.Services
{
    public interface IStripeService
    {
        Task<PaymentResult> ProcessPaymentAsync(decimal amount, string description);
    }
} 