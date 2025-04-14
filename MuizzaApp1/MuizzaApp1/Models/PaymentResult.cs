namespace MuizzaApp1.Models
{
    public class PaymentResult
    {
        public bool IsSuccessful { get; set; }
        public string TransactionId { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public string Error { get; set; }
    }
} 