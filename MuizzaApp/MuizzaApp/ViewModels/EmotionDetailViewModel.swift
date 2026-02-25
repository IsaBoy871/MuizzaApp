import SwiftUI

@MainActor
class EmotionDetailViewModel: ObservableObject {
    let emotionName: String
    private let listChoiceService = ListChoiceService.shared

    init(emotionName: String) {
        self.emotionName = emotionName
    }

    func navigateToListChoice(_ choice: String) async throws -> ListChoice? {
        try await listChoiceService.getListChoiceByTitle(choice)
    }
}
