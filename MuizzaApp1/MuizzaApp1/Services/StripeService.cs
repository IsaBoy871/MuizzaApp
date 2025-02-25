using Stripe;
using MuizzaApp1.Contracts.Services;

namespace MuizzaApp1.Services
{
    public class StripeService : IStripeService
    {
        private const string StripeSecretKey = "sk_test_51QuLHLHthnAJKWiluZAUg9owJNf8QsATl8Of7U2L6j0jBTaRkjFJjEWr0WbDRFiztAOn7QjwPKS299JK23C58ETs00bU5T4a3m";
        private const string StripePublishableKey = "pk_test_51QuLHLHthnAJKWilwKKnE52s2CKDM8skzRT4tvjjQGxMIFaORne9eTdp9nJaPDEmR7lh3YI67yfVjTt5yYPv2D1000wBKDq19X";

        public StripeService()
        {
            StripeConfiguration.ApiKey = StripeSecretKey;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string description)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100), // Stripe uses cents
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }, // Removed apple_pay for now
                    Description = description,
                    Confirm = true,
                    PaymentMethod = "pm_card_visa" // Test card for development
                };

                var service = new PaymentIntentService();
                var intent = await service.CreateAsync(options);

                return new PaymentResult 
                { 
                    IsSuccessful = intent.Status == "succeeded",
                    TransactionId = intent.Id
                };
            }
            catch (StripeException ex)
            {
                return new PaymentResult 
                { 
                    IsSuccessful = false,
                    Error = ex.Message 
                };
            }
        }
    }
} 