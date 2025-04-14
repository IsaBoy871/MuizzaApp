using System.Threading.Tasks;

namespace MuizzaApp1.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessApplePayment(string productType);
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static PaymentResult Successful() => new PaymentResult { Success = true };
        public static PaymentResult Failed(string message) => new PaymentResult { Success = false, ErrorMessage = message };
    }
} 