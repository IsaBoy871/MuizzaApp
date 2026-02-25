import Foundation

@MainActor
class UserService: ObservableObject {
    private let network = NetworkService.shared

    func createUser(appleUserId: String) async throws -> User {
        let body: [String: Any] = [
            "appleUserId": appleUserId,
            "subscriptionTier": "Free"
        ]
        let data = try JSONSerialization.data(withJSONObject: body)
        let url = URL(string: AppConfiguration.apiBaseURL)!.appendingPathComponent("api/users")
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = data
        let (responseData, _) = try await URLSession.shared.data(for: request)
        return try JSONDecoder().decode(User.self, from: responseData)
    }

    func getUserByAppleId(_ appleUserId: String) async throws -> User? {
        try await network.getOptional("api/users/apple/\(appleUserId)")
    }

    func updateUserName(appleUserId: String, name: String) async throws -> User {
        struct NameUpdate: Encodable { let name: String }
        return try await network.putReturning("api/users/apple/\(appleUserId)/name", body: NameUpdate(name: name))
    }

    func getCurrentUser() async throws -> User {
        let appleUserId = UserDefaults.standard.string(forKey: "AppleUserId") ?? ""
        guard !appleUserId.isEmpty else {
            throw AppError.notSignedIn
        }
        if let user = try await getUserByAppleId(appleUserId) {
            return user
        }
        return try await createUser(appleUserId: appleUserId)
    }

    func updateSubscriptionTier(appleUserId: String, tier: String) async throws {
        guard let user = try await getUserByAppleId(appleUserId) else {
            throw AppError.userNotFound
        }
        try await network.putString("api/Users/\(user.id)/subscription", body: "\"\(tier)\"")
    }

    func getSearchBalance() async throws -> Int {
        let user = try await getCurrentUser()
        return try await network.getInt("api/users/apple/\(user.appleUserId ?? "")/search-balance")
    }

    func getDeepDiveBalance() async throws -> Int {
        let user = try await getCurrentUser()
        return try await network.getInt("api/users/apple/\(user.appleUserId ?? "")/deep-dive-balance")
    }

    func decrementSearchBalance() async throws -> Int {
        let user = try await getCurrentUser()
        return try await network.postReturningInt("api/users/apple/\(user.appleUserId ?? "")/search-balance/decrement")
    }

    func decrementDeepDiveBalance() async throws -> Int {
        let user = try await getCurrentUser()
        return try await network.postReturningInt("api/users/apple/\(user.appleUserId ?? "")/deep-dive-balance/decrement")
    }

    func addSearchBalance(_ amount: Int) async throws -> Int {
        let user = try await getCurrentUser()
        let data = "\(amount)".data(using: .utf8)
        return try await network.postReturningInt("api/users/apple/\(user.appleUserId ?? "")/search-balance/add", body: data)
    }

    func addDeepDiveBalance(_ amount: Int) async throws -> Int {
        let user = try await getCurrentUser()
        let data = "\(amount)".data(using: .utf8)
        return try await network.postReturningInt("api/users/apple/\(user.appleUserId ?? "")/deep-dive-balance/add", body: data)
    }

    func updateBalances(searchBalance: Int, deepDiveBalance: Int) async throws {
        let user = try await getCurrentUser()
        struct BalanceUpdate: Encodable { let searchBalance: Int; let deepDiveBalance: Int }
        try await network.put("api/users/apple/\(user.appleUserId ?? "")/balances",
                              body: BalanceUpdate(searchBalance: searchBalance, deepDiveBalance: deepDiveBalance))
    }
}

enum AppError: LocalizedError {
    case notSignedIn
    case userNotFound
    case invalidResponse

    var errorDescription: String? {
        switch self {
        case .notSignedIn: return "No user is currently signed in"
        case .userNotFound: return "User not found"
        case .invalidResponse: return "Invalid response"
        }
    }
}
