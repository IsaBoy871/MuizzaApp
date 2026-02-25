import SwiftUI

@MainActor
class QuotesViewModel: ObservableObject {
    @Published var affirmations: [Affirmation] = []
    @Published var isLoading = false
    @Published var hasNoInternet = false
    @Published var feelingText = ""
    @Published var currentPosition = 0
    @Published var emotionsList: [Emotion] = []
    @Published var navigateToEmotionalResponse = false
    @Published var currentFeeling = ""

    private let affirmationsService = AffirmationsService.shared
    private var allAffirmations: [Affirmation] = []
    private let batchSize = 3

    init() {
        initializeEmotions()
        Task { await loadInitialAffirmations() }
    }

    private func initializeEmotions() {
        emotionsList = [
            Emotion(name: "Angry", imageSource: "angry_woman", color: Color(hex: "FF6B6B")),
            Emotion(name: "Anxious", imageSource: "anxious_woman", color: Color(hex: "7d60cb")),
            Emotion(name: "Bored", imageSource: "bored_woman", color: Color(hex: "7c82ff")),
            Emotion(name: "Depressed", imageSource: "depressed_woman", color: Color(hex: "89888d")),
        ]
    }

    func loadInitialAffirmations() async {
        guard affirmations.isEmpty else { return }
        isLoading = true
        do {
            allAffirmations = try await affirmationsService.getAffirmations().shuffled()
            loadNextBatch()
        } catch {
            print("Error loading affirmations: \(error)")
            hasNoInternet = true
        }
        isLoading = false
    }

    func loadNextBatch() {
        let start = affirmations.count
        let end = min(start + batchSize, allAffirmations.count)
        guard start < end else {
            allAffirmations.shuffle()
            return
        }
        affirmations.append(contentsOf: allAffirmations[start..<end])
    }

    func onPositionChanged(_ position: Int) {
        currentPosition = position
        if position >= affirmations.count - 2 {
            loadNextBatch()
        }
    }

    func submitFeeling() {
        guard !feelingText.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty else { return }
        currentFeeling = feelingText
        feelingText = ""
        navigateToEmotionalResponse = true
    }
}

extension Color {
    init(hex: String) {
        let hex = hex.trimmingCharacters(in: CharacterSet.alphanumerics.inverted)
        var int: UInt64 = 0
        Scanner(string: hex).scanHexInt64(&int)
        let a, r, g, b: UInt64
        switch hex.count {
        case 6:
            (a, r, g, b) = (255, int >> 16, int >> 8 & 0xFF, int & 0xFF)
        case 8:
            (a, r, g, b) = (int >> 24, int >> 16 & 0xFF, int >> 8 & 0xFF, int & 0xFF)
        default:
            (a, r, g, b) = (255, 0, 0, 0)
        }
        self.init(.sRGB, red: Double(r) / 255, green: Double(g) / 255, blue: Double(b) / 255, opacity: Double(a) / 255)
    }
}
