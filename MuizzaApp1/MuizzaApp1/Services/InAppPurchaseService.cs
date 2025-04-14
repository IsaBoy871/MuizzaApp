using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using MuizzaApp1.Contracts.Services;
using MuizzaApp1.Models;
#if IOS
using Foundation;
using PassKit;
using UIKit;
#endif

namespace MuizzaApp1.Services
{
    public class InAppPurchaseService : MuizzaApp1.Contracts.Services.IInAppPurchaseService
    {
        private readonly ILogger<InAppPurchaseService> _logger;

        public InAppPurchaseService(ILogger<InAppPurchaseService> logger)
        {
            _logger = logger;
        }

        public async Task<PurchaseResult> PurchaseAsync(string productId)
        {
            try
            {
                if (DeviceInfo.Platform != DevicePlatform.iOS)
                {
                    return PurchaseResult.Failed("Apple Pay is only available on iOS devices");
                }

#if IOS
                // Get the price based on the product ID
                var (price, description) = GetProductDetails(productId);

                // Create a payment request
                var request = new PKPaymentRequest
                {
                    MerchantIdentifier = "merchant.com.muizza.app", // Replace with your merchant ID
                    CountryCode = "GB",
                    CurrencyCode = "GBP",
                    SupportedNetworks = new[] 
                    { 
                        PKPaymentNetwork.Visa,
                        PKPaymentNetwork.Amex,
                        PKPaymentNetwork.Discover,
                        PKPaymentNetwork.MasterCard 
                    },
                    MerchantCapabilities = PKMerchantCapability.ThreeDS | PKMerchantCapability.Credit | PKMerchantCapability.Debit,
                    PaymentSummaryItems = new[]
                    {
                        new PKPaymentSummaryItem
                        {
                            Label = description,
                            Amount = new NSDecimalNumber(price.ToString())
                        }
                    }
                };

                // Check if Apple Pay is available
                if (!PKPaymentAuthorizationViewController.CanMakePayments)
                {
                    _logger.LogWarning("Apple Pay is not available on this device");
                    return PurchaseResult.Failed("Apple Pay is not available on this device");
                }

                // Check if the device can make payments using the specified networks
                if (!PKPaymentAuthorizationViewController.CanMakePaymentsUsingNetworks(request.SupportedNetworks))
                {
                    _logger.LogWarning("No supported payment methods available");
                    return PurchaseResult.Failed("No supported payment methods available");
                }

                // Present the payment sheet
                var controller = new PKPaymentAuthorizationViewController(request);
                if (controller == null)
                {
                    _logger.LogError("Failed to create payment controller");
                    return PurchaseResult.Failed("Unable to create payment controller");
                }

                var tcs = new TaskCompletionSource<PurchaseResult>();
                var paymentDelegate = new PaymentAuthorizationDelegate(tcs);
                controller.Delegate = (IPKPaymentAuthorizationViewControllerDelegate)paymentDelegate;

                // Show the payment sheet
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    var window = UIApplication.SharedApplication.KeyWindow;
                    var viewController = window.RootViewController;
                    viewController.PresentViewController(controller, true, null);
                });

                var result = await tcs.Task;
                return result;
#else
                return PurchaseResult.Failed("Apple Pay is not supported on this platform");
#endif
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing in-app purchase for product: {ProductId}", productId);
                return PurchaseResult.Failed("An error occurred while processing the payment");
            }
        }

        private (decimal price, string description) GetProductDetails(string productId)
        {
            return productId switch
            {
                "com.muizza.catnips.starter" => (0.80m, "10 CatNips"),
                "com.muizza.catnips.popular" => (4.99m, "50 CatNips"),
                "com.muizza.catnips.premium" => (40.00m, "1000 CatNips"),
                "com.muizza.subscription.monthly" => (6.99m, "Monthly Premium Subscription"),
                "com.muizza.subscription.yearly" => (48.00m, "Yearly Premium Subscription"),
                _ => throw new ArgumentException($"Invalid product ID: {productId}")
            };
        }
    }

#if IOS
    internal class PaymentAuthorizationDelegate : PKPaymentAuthorizationViewControllerDelegate
    {
        private readonly TaskCompletionSource<PurchaseResult> _tcs;

        public PaymentAuthorizationDelegate(TaskCompletionSource<PurchaseResult> tcs)
        {
            _tcs = tcs;
        }

        public override void DidAuthorizePayment(PKPaymentAuthorizationViewController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
        {
            completion(PKPaymentAuthorizationStatus.Success);
            _tcs.SetResult(PurchaseResult.Successful(payment.Token.TransactionIdentifier));
        }

        public override void PaymentAuthorizationViewControllerDidFinish(PKPaymentAuthorizationViewController controller)
        {
            controller.DismissViewController(true, null);
            if (!_tcs.Task.IsCompleted)
            {
                _tcs.SetResult(PurchaseResult.Failed("Payment cancelled"));
            }
        }
    }
#endif
} 