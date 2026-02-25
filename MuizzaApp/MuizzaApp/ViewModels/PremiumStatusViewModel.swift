import SwiftUI

@MainActor
class PremiumStatusViewModel: ObservableObject {
    @Published var searchBalance = 0
    @Published var deepDiveBalance = 0
    @Published var isPremiumUser = false
    @Published var isRefreshing = false

    private let userService = UserService()
    private let subscriptionService = SubscriptionService.shared

    func loadData() async {
        await checkPremiumStatus()
        await loadBalances()
    }

    func loadBalances() async {
        searchBalance = (try? await userService.getSearchBalance()) ?? 0
        deepDiveBalance = (try? await userService.getDeepDiveBalance()) ?? 0
    }

    func refreshBalances() async {
        isRefreshing = true
        await loadBalances()
        isRefreshing = false
    }

    private func checkPremiumStatus() async {
        await subscriptionService.checkPremiumStatus()
        isPremiumUser = subscriptionService.isPremium
    }
}
