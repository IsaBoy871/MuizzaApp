import Foundation

struct User: Codable, Identifiable {
    let id: Int64
    var name: String?
    var email: String?
    var appleUserId: String?
    var subscriptionTier: String?
    var subscriptionEndDate: Date?
    var createdAt: Date?
    var hasTrial: Bool
    var searchBalance: Int
    var deepDiveBalance: Int

    var stringId: String { String(id) }
}
