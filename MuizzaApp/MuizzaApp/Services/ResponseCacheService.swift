import Foundation

class ResponseCacheService {
    static let shared = ResponseCacheService()
    private let cacheExpirationDays = 90
    private let defaults = UserDefaults.standard

    private let commonResponses: [String: EmotionalResponse] = [
        "anxious": EmotionalResponse(
            affirmation: "I am stronger than my anxiety",
            explanation: "Hey, when you're anxious, your brain releases cortisol - it's totally normal! Your amygdala (your brain's alarm system) is just trying to protect you by making you extra alert.\n\n• Take three deep breaths - this activates your parasympathetic system\n\n• Ground yourself by naming 5 things you can see\n\n• Remember this feeling will pass - your brain is just being overprotective"
        ),
        "sad": EmotionalResponse(
            affirmation: "I have the inner strength to overcome this sadness",
            explanation: "Your brain's just a bit low on feel-good chemicals right now, which is why things might feel heavier than usual. This happens when your brain's emotional centers are processing difficult feelings.\n\n• Do one small activity you usually enjoy\n\n• Try gentle movement to boost your natural mood chemicals\n\n• Connect with someone you trust - social connection releases oxytocin"
        ),
        "stressed": EmotionalResponse(
            affirmation: "I can manage this stress and find my balance",
            explanation: "When you're stressed, your brain's in 'fight or flight' mode, making everything feel overwhelming. Your body's stress response is trying to give you energy to handle challenges.\n\n• Break big tasks into smaller, manageable steps\n\n• Take a 2-minute break to stretch or move around\n\n• Focus on just one thing at a time - this helps your brain feel more in control"
        )
    ]

    func getCachedResponse(feeling: String) -> (affirmation: String, explanation: String)? {
        let normalized = normalizeFeeling(feeling)

        if let common = commonResponses[normalized] {
            return (common.affirmation, common.explanation)
        }

        if let similarKey = commonResponses.keys.first(where: { $0.contains(normalized) || normalized.contains($0) }),
           let similar = commonResponses[similarKey] {
            return (similar.affirmation, similar.explanation)
        }

        let key = cacheKey(for: normalized)
        guard let data = defaults.data(forKey: key),
              let cached = try? JSONDecoder().decode(EmotionalResponse.self, from: data) else {
            return nil
        }

        if Date().timeIntervalSince(cached.cachedAt) > Double(cacheExpirationDays) * 86400 {
            defaults.removeObject(forKey: key)
            return nil
        }

        return (cached.affirmation, cached.explanation)
    }

    func cacheResponse(feeling: String, affirmation: String, explanation: String) {
        let response = EmotionalResponse(affirmation: affirmation, explanation: explanation)
        if let data = try? JSONEncoder().encode(response) {
            defaults.set(data, forKey: cacheKey(for: normalizeFeeling(feeling)))
        }
    }

    func clearAllCache() {
        let domain = Bundle.main.bundleIdentifier!
        defaults.removePersistentDomain(forName: domain)
    }

    private func normalizeFeeling(_ feeling: String) -> String {
        feeling.lowercased()
            .trimmingCharacters(in: .whitespacesAndNewlines)
            .replacingOccurrences(of: "i'm feeling ", with: "")
            .replacingOccurrences(of: "i am feeling ", with: "")
            .replacingOccurrences(of: "i feel ", with: "")
    }

    private func cacheKey(for feeling: String) -> String {
        "emotion_response_\(feeling)"
    }
}
