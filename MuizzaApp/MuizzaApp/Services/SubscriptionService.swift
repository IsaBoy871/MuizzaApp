import Foundation

@MainActor
class SubscriptionService: ObservableObject {
    static let shared = SubscriptionService()
    private let userService = UserService()
    private let premiumMonthlySearchBalance = 500
    private let premiumMonthlyDeepDiveBalance = 200
    private let lastBalanceResetKey = "last_balance_reset_date"

    @Published var isPremium = false

    func checkPremiumStatus() async {
        do {
            let user = try await userService.getCurrentUser()
            isPremium = user.subscriptionTier == "Premium"
        } catch {
            isPremium = false
        }
    }

    func getSubscriptionTier() async -> String {
        do {
            let user = try await userService.getCurrentUser()
            return user.subscriptionTier ?? "Free"
        } catch {
            return "Free"
        }
    }

    func canMakeSearch() async -> Bool {
        do {
            let balance = try await userService.getSearchBalance()
            return balance > 0
        } catch {
            return false
        }
    }

    func getRemainingSearches() async -> Int {
        do {
            return try await userService.getSearchBalance()
        } catch {
            return 0
        }
    }
}
