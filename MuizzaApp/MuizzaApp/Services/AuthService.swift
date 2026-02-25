import Foundation
import AuthenticationServices

class AuthService: NSObject, ObservableObject {
    @Published var isAuthenticated = false
    @Published var appleUserId: String?
    private var authContinuation: CheckedContinuation<(userId: String, email: String?), Error>?

    override init() {
        super.init()
        let storedId = UserDefaults.standard.string(forKey: "AppleUserId")
        isAuthenticated = storedId != nil && !storedId!.isEmpty
        appleUserId = storedId
    }

    func signInWithApple() async throws -> (userId: String, email: String?) {
        try await withCheckedThrowingContinuation { continuation in
            self.authContinuation = continuation

            let provider = ASAuthorizationAppleIDProvider()
            let request = provider.createRequest()
            request.requestedScopes = [.email, .fullName]

            let controller = ASAuthorizationController(authorizationRequests: [request])
            controller.delegate = self
            controller.performRequests()
        }
    }

    func signOut() {
        UserDefaults.standard.removeObject(forKey: "AppleUserId")
        UserDefaults.standard.removeObject(forKey: "HasCompletedOnboarding")
        isAuthenticated = false
        appleUserId = nil
    }
}

extension AuthService: ASAuthorizationControllerDelegate {
    func authorizationController(controller: ASAuthorizationController, didCompleteWithAuthorization authorization: ASAuthorization) {
        if let credential = authorization.credential as? ASAuthorizationAppleIDCredential {
            let userId = credential.user
            let email = credential.email

            UserDefaults.standard.set(userId, forKey: "AppleUserId")

            if let email = email {
                UserDefaults.standard.set(email, forKey: "AppleSignIn_Email_\(userId)")
            }

            DispatchQueue.main.async {
                self.isAuthenticated = true
                self.appleUserId = userId
            }

            authContinuation?.resume(returning: (userId: userId, email: email))
            authContinuation = nil
        }
    }

    func authorizationController(controller: ASAuthorizationController, didCompleteWithError error: Error) {
        authContinuation?.resume(throwing: error)
        authContinuation = nil
    }
}
