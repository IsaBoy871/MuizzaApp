import Foundation

actor OpenAIService {
    static let shared = OpenAIService()

    private let session: URLSession
    private let apiKey: String
    private var dailyUsage: [String: Int] = [:]
    private var lastRequestTime = Date.distantPast
    private let dailyFreeLimit = 50
    private let minRequestIntervalMs = 1000
    private let cacheService = ResponseCacheService.shared

    private init() {
        self.session = URLSession.shared
        self.apiKey = AppConfiguration.openAIAPIKey
    }

    func generateResponse(feeling: String) async throws -> (affirmation: String, explanation: String) {
        if let cached = cacheService.getCachedResponse(feeling: feeling) {
            return cached
        }

        guard checkDailyLimit() else {
            return fallbackResponse
        }

        await applyRateLimit()
        let result = try await makeAPICall(feeling: feeling)
        cacheService.cacheResponse(feeling: feeling, affirmation: result.affirmation, explanation: result.explanation)
        return result
    }

    func generateDeepDiveResponse(feeling: String, advisorSystemPrompt: String) async throws -> String {
        guard checkDailyLimit() else {
            throw OpenAIError.rateLimitExceeded
        }

        await applyRateLimit()

        let body: [String: Any] = [
            "model": AppConfiguration.longResponseModel,
            "messages": [
                ["role": "system", "content": advisorSystemPrompt],
                ["role": "user", "content": "I'm feeling: \(feeling)"]
            ],
            "temperature": 1.0
        ]

        let data = try JSONSerialization.data(withJSONObject: body)
        var request = URLRequest(url: URL(string: "https://api.openai.com/v1/chat/completions")!)
        request.httpMethod = "POST"
        request.setValue("Bearer \(apiKey)", forHTTPHeaderField: "Authorization")
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = data

        let (responseData, _) = try await session.data(for: request)
        let response = try JSONDecoder().decode(OpenAIResponse.self, from: responseData)
        return response.choices.first?.message.content ?? ""
    }

    func getRemainingSearches() -> Int {
        let today = todayKey
        return max(0, dailyFreeLimit - (dailyUsage[today] ?? 0))
    }

    // MARK: - Private

    private func checkDailyLimit() -> Bool {
        let today = todayKey
        if dailyUsage[today] == nil {
            dailyUsage.removeAll()
            dailyUsage[today] = 0
        }
        guard (dailyUsage[today] ?? 0) < dailyFreeLimit else { return false }
        dailyUsage[today, default: 0] += 1
        return true
    }

    private func applyRateLimit() async {
        let elapsed = Date().timeIntervalSince(lastRequestTime) * 1000
        if elapsed < Double(minRequestIntervalMs) {
            try? await Task.sleep(nanoseconds: UInt64((Double(minRequestIntervalMs) - elapsed) * 1_000_000))
        }
        lastRequestTime = Date()
    }

    private func makeAPICall(feeling: String) async throws -> (affirmation: String, explanation: String) {
        let systemPrompt = """
        You are a friendly AI counselor who explains brain science in simple terms.

        1. AFFIRMATION: One empowering 'I' statement (10-15 words)
        2. EXPLANATION: Two parts:
           - First paragraph: 2-3 sentences (60-80 words) explaining what's happening in the brain using everyday language.
           - Then 3 bullet points with SPECIFIC, ACTIONABLE advice (10-12 words each).

        Format exactly like this:
        AFFIRMATION: I [empowering statement]
        EXPLANATION: [Friendly explanation]

        • [Specific action]


        • [Concrete activity]


        • [Practical step]
        """

        let body: [String: Any] = [
            "model": AppConfiguration.defaultModel,
            "messages": [
                ["role": "system", "content": systemPrompt],
                ["role": "user", "content": "I'm feeling: \(feeling)"]
            ],
            "max_tokens": 150,
            "temperature": 0.7
        ]

        let data = try JSONSerialization.data(withJSONObject: body)
        var request = URLRequest(url: URL(string: "https://api.openai.com/v1/chat/completions")!)
        request.httpMethod = "POST"
        request.setValue("Bearer \(apiKey)", forHTTPHeaderField: "Authorization")
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = data

        let (responseData, _) = try await session.data(for: request)
        let response = try JSONDecoder().decode(OpenAIResponse.self, from: responseData)
        let content = response.choices.first?.message.content ?? ""

        let parts = content.components(separatedBy: "EXPLANATION:")
        let affirmation = parts.first?
            .replacingOccurrences(of: "AFFIRMATION:", with: "")
            .trimmingCharacters(in: .whitespacesAndNewlines) ?? ""
        let explanation = parts.count > 1 ? parts[1].trimmingCharacters(in: .whitespacesAndNewlines) : ""

        return (affirmation, explanation)
    }

    private var fallbackResponse: (affirmation: String, explanation: String) {
        ("I acknowledge how I'm feeling",
         "Hey, we're experiencing high demand right now. Try expressing how you feel in simpler terms, or check back in a little while. Remember, your feelings are valid and it's okay to take time to process them.")
    }

    private var todayKey: String {
        let formatter = DateFormatter()
        formatter.dateFormat = "yyyy-MM-dd"
        return formatter.string(from: Date())
    }
}

enum OpenAIError: LocalizedError {
    case rateLimitExceeded
    case unauthorized
    case generic(String)

    var errorDescription: String? {
        switch self {
        case .rateLimitExceeded: return "Daily limit reached"
        case .unauthorized: return "Authentication error"
        case .generic(let msg): return msg
        }
    }
}
