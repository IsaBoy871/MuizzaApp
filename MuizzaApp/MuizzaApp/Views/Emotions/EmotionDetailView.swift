import SwiftUI

struct EmotionDetailView: View {
    let emotionName: String
    @StateObject private var viewModel: EmotionDetailViewModel
    @State private var selectedChoice: ListChoice?
    @State private var showListChoice = false

    private let choices: [String: [String]] = [
        "Angry": ["Someone wronged me", "I feel unheard", "Life is unfair", "I'm angry at myself"],
        "Anxious": ["Worried about future", "Social anxiety", "Work pressure", "Health concerns"],
        "Bored": ["Nothing excites me", "Stuck in a routine", "Lacking purpose", "Uninspired"],
        "Depressed": ["Feeling empty", "Lost motivation", "Overwhelmed by sadness", "Can't see the point"],
    ]

    init(emotionName: String) {
        self.emotionName = emotionName
        _viewModel = StateObject(wrappedValue: EmotionDetailViewModel(emotionName: emotionName))
    }

    var body: some View {
        ZStack {
            Color(hex: "F6E5CB").ignoresSafeArea()

            ScrollView {
                VStack(spacing: 20) {
                    Text("I'm feeling \(emotionName)")
                        .font(.custom("fredoka", size: 24))
                        .fontWeight(.bold)

                    Text("What resonates with you?")
                        .font(.custom("fredoka", size: 18))
                        .foregroundColor(.secondary)

                    ForEach(choices[emotionName] ?? [], id: \.self) { choice in
                        Button(action: {
                            Task {
                                if let lc = try? await viewModel.navigateToListChoice(choice) {
                                    selectedChoice = lc
                                    showListChoice = true
                                }
                            }
                        }) {
                            Text(choice)
                                .font(.custom("fredoka", size: 16))
                                .foregroundColor(.primary)
                                .frame(maxWidth: .infinity, alignment: .leading)
                                .padding()
                                .background(Color.white.cornerRadius(12))
                        }
                    }
                }
                .padding()
            }
        }
        .navigationTitle(emotionName)
        .navigationBarTitleDisplayMode(.inline)
        .navigationDestination(isPresented: $showListChoice) {
            if let choice = selectedChoice {
                ListChoiceView(emotion: emotionName, choice: choice.title)
            }
        }
    }
}
