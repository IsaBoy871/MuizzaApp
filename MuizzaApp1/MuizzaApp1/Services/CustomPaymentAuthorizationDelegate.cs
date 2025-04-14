#if IOS
using Foundation;
using PassKit;
using System;
using System.Threading.Tasks;

namespace MuizzaApp1.Services
{
    public class CustomPaymentAuthorizationDelegate : PKPaymentAuthorizationControllerDelegate
    {
        private readonly TaskCompletionSource<bool> _tcs;
        private readonly Func<string, Task> _processPayment;
        private readonly string _packType;

        public CustomPaymentAuthorizationDelegate(TaskCompletionSource<bool> tcs, Func<string, Task> processPayment, string packType)
        {
            _tcs = tcs;
            _processPayment = processPayment;
            _packType = packType;
        }

        public override async void DidAuthorizePayment(PKPaymentAuthorizationController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
        {
            try
            {
                await _processPayment(_packType);
                completion(PKPaymentAuthorizationStatus.Success);
                _tcs.SetResult(true);
            }
            catch (Exception)
            {
                completion(PKPaymentAuthorizationStatus.Failure);
                _tcs.SetResult(false);
            }
        }

        public override void DidFinish(PKPaymentAuthorizationController controller)
        {
            controller.DismissAsync();
            _tcs.TrySetResult(false);
        }
    }
}
#endif 