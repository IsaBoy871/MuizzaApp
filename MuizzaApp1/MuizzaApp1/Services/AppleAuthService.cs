using System;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Devices;
using System.Diagnostics;
using MuizzaApp1.Contracts.Services;

#if IOS
using Foundation;
using UIKit;
using AuthenticationServices;
#endif

namespace MuizzaApp1.Services
{
    public class AppleAuthService : IAppleAuthService
    {
        private readonly string _privateKeyId; // Your Key ID
        private readonly string _teamId = "8P2L29K69L"; // Your Team ID
        private readonly string _clientId = "com.isaadeel.muizzaapp"; // Your Service ID
        private readonly string _privateKey; // The content of your .p8 file
        
        public AppleAuthService()
        {
            _privateKeyId = "XSY7AM2SSW"; // The one you already have
            _privateKey = """
                -----BEGIN PRIVATE KEY-----
                MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgNEestO8L3sox1tVt
                SqHAYwLbvWcz2LLUiiaREbBohaegCgYIKoZIzj0DAQehRANCAATz0lQ4rv8MdjRV
                S5Ba095QbeAV8TFtwYK7oxGONEknCbcKk2j3JLi+GqpNlRSgTnxORCkFoVf6Nz0o
                GO69Tpgt
                -----END PRIVATE KEY-----
                """;
        }

        public async Task<AuthenticationResult> AuthenticateAsync()
        {
#if IOS
            var tcs = new TaskCompletionSource<AuthenticationResult>();

            if (DeviceInfo.Platform != DevicePlatform.iOS)
            {
                return new AuthenticationResult { 
                    IsSuccessful = false, 
                    Error = "Apple Sign In is only supported on iOS" 
                };
            }

            try
            {
                var provider = new ASAuthorizationAppleIdProvider();
                var request = provider.CreateRequest();
                
                // Explicitly request email and full name
                request.RequestedScopes = new[] { 
                    ASAuthorizationScope.Email,
                    ASAuthorizationScope.FullName
                };

                var authorizationController = new ASAuthorizationController(new[] { request });
                authorizationController.Delegate = new AuthorizationDelegate(tcs);
                authorizationController.PresentationContextProvider = new PresentationContextProvider();
                authorizationController.PerformRequests();

                var result = await tcs.Task;
                return result;
            }
            catch (Exception ex)
            {
                return new AuthenticationResult { 
                    IsSuccessful = false, 
                    Error = ex.Message 
                };
            }
#else
            return new AuthenticationResult { 
                IsSuccessful = false, 
                Error = "Apple Sign In is only supported on iOS" 
            };
#endif
        }
    }

#if IOS
    public class AuthorizationDelegate : ASAuthorizationControllerDelegate
    {
        private readonly TaskCompletionSource<AuthenticationResult> _tcs;

        public AuthorizationDelegate(TaskCompletionSource<AuthenticationResult> tcs)
        {
            _tcs = tcs;
        }

        public override void DidComplete(ASAuthorizationController controller, ASAuthorization authorization)
        {
            if (authorization.GetCredential<ASAuthorizationAppleIdCredential>() is ASAuthorizationAppleIdCredential appleIdCredential)
            {
                var email = appleIdCredential.Email;
                Debug.WriteLine($"Apple Sign In - User ID: {appleIdCredential.User}");
                Debug.WriteLine($"Apple Sign In - Email: {email}");
                Debug.WriteLine($"Apple Sign In - Full Name: {appleIdCredential.FullName?.GivenName} {appleIdCredential.FullName?.FamilyName}");

                var result = new AuthenticationResult 
                { 
                    IsSuccessful = true,
                    UserId = appleIdCredential.User,
                    Email = email
                };

                if (!string.IsNullOrEmpty(email))
                {
                    Debug.WriteLine("Storing email in preferences");
                    Preferences.Set($"AppleSignIn_Email_{appleIdCredential.User}", email);
                }
                else
                {
                    Debug.WriteLine("Email not provided, checking preferences");
                    email = Preferences.Get($"AppleSignIn_Email_{appleIdCredential.User}", null);
                    Debug.WriteLine($"Retrieved email from preferences: {email}");
                    result.Email = email;
                }

                _tcs.TrySetResult(result);
            }
            else
            {
                Debug.WriteLine("Apple Sign In - No valid credentials received");
                _tcs.TrySetResult(new AuthenticationResult { IsSuccessful = false });
            }
        }

        public override void DidComplete(ASAuthorizationController controller, NSError error)
        {
            _tcs.TrySetResult(new AuthenticationResult 
            { 
                IsSuccessful = false,
                Error = error.LocalizedDescription 
            });
        }
    }

    public class PresentationContextProvider : NSObject, IASAuthorizationControllerPresentationContextProviding
    {
        public UIWindow GetPresentationAnchor(ASAuthorizationController controller)
        {
            return UIApplication.SharedApplication.KeyWindow;
        }
    }
#endif

    public class AuthenticationResult
    {
        public bool IsSuccessful { get; set; }
        public string IdToken { get; set; }
        public string Error { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
} 