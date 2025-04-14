using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MuizzaApp1.Contracts.Services;

namespace MuizzaApp1.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly IInAppPurchaseService _inAppPurchaseService;

        public PaymentService(
            ILogger<PaymentService> logger,
            IInAppPurchaseService inAppPurchaseService)
        {
            _logger = logger;
            _inAppPurchaseService = inAppPurchaseService;
        }

        public async Task<PaymentResult> ProcessApplePayment(string productType)
        {
            try
            {
                var productId = GetProductId(productType);
                var result = await _inAppPurchaseService.PurchaseAsync(productId);

                if (result.Success)
                {
                    return PaymentResult.Successful();
                }
                else
                {
                    _logger.LogWarning("Payment failed: {ErrorMessage}", result.ErrorMessage);
                    return PaymentResult.Failed(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for product type: {ProductType}", productType);
                return PaymentResult.Failed("An error occurred while processing your payment.");
            }
        }

        private string GetProductId(string productType)
        {
            return productType.ToLower() switch
            {
                "starter" => "com.muizza.catnips.starter",
                "popular" => "com.muizza.catnips.popular",
                "premium" => "com.muizza.catnips.premium",
                _ => throw new ArgumentException($"Invalid product type: {productType}")
            };
        }
    }
} 