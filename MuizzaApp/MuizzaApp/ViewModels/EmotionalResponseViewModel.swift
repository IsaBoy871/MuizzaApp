import SwiftUI

@MainActor
class EmotionalResponseViewModel: ObservableObject {
    @Published var isLoading = false
    @Published var errorMessage: String?
    @Published var affirmation = ""
    @Published var explanation = ""
    @Published var hasResponse = false
    @Published var searchBalance = 0
    @Published var isNotPremium = true

    private let openAIService = OpenAIService.shared
    private let userService = UserService()
    private let subscriptionService = SubscriptionService.shared
    var currentFeeling: String

    init(feeling: String) {
        self.currentFeeling = feeling
        Task {
            await loadSearchBalance()
            await checkPremiumStatus()
            await generateResponse()
        }
    }

    func generateResponse() async {
        isLoading = true
        errorMessage = nil
        hasResponse = false

        do {
            await loadSearchBalance()

            guard searchBalance > 0 else {
                errorMessage = "You don't have enough Search CatNips. Please purchase more!"
                isLoading = false
                return
            }

            let result = try await openAIService.generateResponse(feeling: currentFeeling)

            if !result.affirmation.isEmpty && !result.explanation.isEmpty {
                _ = try? await userService.decrementSearchBalance()
                await loadSearchBalance()

                affirmation = result.affirmation
                explanation = result.explanation
                hasResponse = true
            } else {
                errorMessage = "Unable to generate a response. Please try again."
            }
        } catch let error as OpenAIError {
            switch error {
            case .rateLimitExceeded:
                errorMessage = "You've reached your daily limit. Try again tomorrow or upgrade to Premium!"
            default:
                errorMessage = "Something went wrong. Please try again later."
            }
        } catch {
            errorMessage = "Unable to connect. Please check your internet connection."
        }

        isLoading = false
    }

    func loadSearchBalance() async {
        searchBalance = (try? await userService.getSearchBalance()) ?? 0
    }

    private func checkPremiumStatus() async {
        await subscriptionService.checkPremiumStatus()
        isNotPremium = !subscriptionService.isPremium
    }
}
