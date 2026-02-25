import Foundation

struct DailyEntry: Codable {
    var affirmation: String
    var intention: String
    var gratitude: String
    var createdAt: Date
    var expiresAt: Date
}
