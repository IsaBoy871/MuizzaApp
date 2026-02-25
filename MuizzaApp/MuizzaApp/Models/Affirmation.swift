import Foundation

struct Affirmation: Codable, Identifiable {
    let id: Int
    let text: String
    let category: String?
}
