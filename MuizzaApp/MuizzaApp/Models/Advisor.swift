import Foundation

struct Advisor: Identifiable {
    let id = UUID()
    let name: String
    let imagePath: String
    let description: String
    let systemPrompt: String
}
