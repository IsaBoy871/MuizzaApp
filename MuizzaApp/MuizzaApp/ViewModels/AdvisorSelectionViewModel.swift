import SwiftUI

@MainActor
class AdvisorSelectionViewModel: ObservableObject {
    @Published var advisors: [Advisor] = []
    @Published var isLoading = false
    @Published var deepDiveBalance = 0
    @Published var feeling: String

    private let advisorService = AdvisorService.shared
    private let userService = UserService()

    init(feeling: String) {
        self.feeling = feeling
        Task {
            await loadAdvisors()
            await updateDeepDiveBalance()
        }
    }

    func loadAdvisors() async {
        isLoading = true
        advisors = await advisorService.getAdvisors()
        isLoading = false
    }

    func updateDeepDiveBalance() async {
        deepDiveBalance = (try? await userService.getDeepDiveBalance()) ?? 0
    }

    func decrementDeepDiveBalance() async {
        deepDiveBalance = (try? await userService.decrementDeepDiveBalance()) ?? deepDiveBalance
    }
}
