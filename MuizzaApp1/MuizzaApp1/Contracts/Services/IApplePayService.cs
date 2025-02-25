namespace MuizzaApp1.Contracts.Services
{
    public interface IApplePayService
    {
        Task<PaymentResult> ProcessPaymentAsync(decimal amount, string description);
    }

    public class PaymentResult
    {
        public bool IsSuccessful { get; set; }
        public string TransactionId { get; set; }
        public string Error { get; set; }
    }
} 