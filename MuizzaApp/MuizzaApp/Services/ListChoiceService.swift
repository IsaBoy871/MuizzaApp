import Foundation

class ListChoiceService {
    static let shared = ListChoiceService()
    private let network = NetworkService.shared

    func getListChoiceByTitle(_ title: String) async throws -> ListChoice? {
        let encoded = title.addingPercentEncoding(withAllowedCharacters: .urlPathAllowed) ?? title
        return try await network.getOptional("api/ListChoice/title/\(encoded)")
    }
}
