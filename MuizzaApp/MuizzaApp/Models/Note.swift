import Foundation

struct Note: Codable, Identifiable {
    let id: Int
    var content: String
    var userId: String
    var createdAt: Date
    var updatedAt: Date?
}
