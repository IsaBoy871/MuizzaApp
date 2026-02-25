import Foundation

struct EmotionalResponse: Codable {
    var affirmation: String
    var explanation: String
    var cachedAt: Date

    init(affirmation: String, explanation: String) {
        self.affirmation = affirmation
        self.explanation = explanation
        self.cachedAt = Date()
    }
}
