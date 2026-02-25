import Foundation

class AffirmationsService {
    static let shared = AffirmationsService()
    private let network = NetworkService.shared

    func getAffirmations() async throws -> [Affirmation] {
        try await network.get("api/affirmations")
    }
}
