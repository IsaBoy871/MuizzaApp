import Foundation

struct ListChoice: Codable, Identifiable {
    let id: Int
    let emotion: String
    let title: String
    let affirmation: String
    let thoughts: String

    enum CodingKeys: String, CodingKey {
        case id = "id"
        case emotion = "emotion"
        case title = "title"
        case affirmation = "affirmation"
        case thoughts = "thoughts"
    }
}
