namespace MuizzaApp1.Models
{
    public class PurchaseResult
    {
        public bool Success { get; }
        public string ErrorMessage { get; }
        public string TransactionId { get; }

        private PurchaseResult(bool success, string errorMessage = null, string transactionId = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
            TransactionId = transactionId;
        }

        public static PurchaseResult Successful(string transactionId = null) => new PurchaseResult(true, transactionId: transactionId);
        public static PurchaseResult Failed(string errorMessage) => new PurchaseResult(false, errorMessage);
    }
} 