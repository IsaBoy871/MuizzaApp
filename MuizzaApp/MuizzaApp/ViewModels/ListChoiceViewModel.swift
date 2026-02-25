import SwiftUI

@MainActor
class ListChoiceViewModel: ObservableObject {
    @Published var title: String
    @Published var affirmationText = ""
    @Published var thoughtsText = ""
    @Published var isLoading = true

    private let listChoiceService = ListChoiceService.shared

    init(emotion: String, choice: String) {
        self.title = choice
        Task { await loadData(choice: choice) }
    }

    func loadData(choice: String) async {
        isLoading = true
        do {
            if let listChoice = try await listChoiceService.getListChoiceByTitle(choice) {
                affirmationText = listChoice.affirmation
                thoughtsText = listChoice.thoughts
            }
        } catch {
            print("Error loading list choice: \(error)")
        }
        isLoading = false
    }
}
