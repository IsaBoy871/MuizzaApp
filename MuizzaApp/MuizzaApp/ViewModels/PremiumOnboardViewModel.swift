import SwiftUI

@MainActor
class PremiumOnboardViewModel: ObservableObject {
    @Published var isMonthlySelected = true
    @Published var isProcessingPayment = false

    private let userService = UserService()

    func processSubscription() async {
        do {
            let user = try await userService.getCurrentUser()
            UserDefaults.standard.set(true, forKey: "HasCompletedOnboarding")
        } catch {
            print("Subscription error: \(error)")
        }
    }
}
