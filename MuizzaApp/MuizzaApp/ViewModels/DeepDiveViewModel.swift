import SwiftUI

@MainActor
class DeepDiveViewModel: ObservableObject {
    @Published var isLoading = true
    @Published var detailedResponse = ""

    let feeling: String
    private let advisorSystemPrompt: String
    private let openAIService = OpenAIService.shared

    init(feeling: String, advisorSystemPrompt: String) {
        self.feeling = feeling
        self.advisorSystemPrompt = advisorSystemPrompt
        Task { await initialize() }
    }

    func initialize() async {
        isLoading = true
        do {
            detailedResponse = try await openAIService.generateDeepDiveResponse(
                feeling: feeling,
                advisorSystemPrompt: advisorSystemPrompt
            )
        } catch {
            detailedResponse = "We're having trouble generating a detailed response right now. Please try again later."
        }
        isLoading = false
    }
}
