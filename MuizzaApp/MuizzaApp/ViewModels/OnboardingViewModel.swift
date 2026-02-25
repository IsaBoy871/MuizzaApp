import SwiftUI

@MainActor
class OnboardingViewModel: ObservableObject {
    @Published var currentStep = 0
    @Published var userName = ""
    @Published var isSigningIn = false
    @Published var showError = false
    @Published var errorMessage = ""

    private let authService: AuthService
    private let userService = UserService()

    init(authService: AuthService) {
        self.authService = authService
    }

    func signInWithApple() async {
        isSigningIn = true
        do {
            let result = try await authService.signInWithApple()

            let existingUser = try await userService.getUserByAppleId(result.userId)
            if existingUser == nil {
                _ = try await userService.createUser(appleUserId: result.userId)
            }

            currentStep = 1
        } catch {
            errorMessage = "Unable to sign in with Apple. Please try again."
            showError = true
        }
        isSigningIn = false
    }

    func saveNameAndContinue() async {
        guard !userName.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty else {
            errorMessage = "Please enter your name first!"
            showError = true
            return
        }

        do {
            let appleUserId = UserDefaults.standard.string(forKey: "AppleUserId") ?? ""
            _ = try await userService.updateUserName(appleUserId: appleUserId, name: userName.trimmingCharacters(in: .whitespacesAndNewlines))
            currentStep = 5
        } catch {
            errorMessage = "Failed to save your name. Please try again."
            showError = true
        }
    }

    func completeOnboarding() {
        UserDefaults.standard.set(true, forKey: "HasCompletedOnboarding")
    }
}
