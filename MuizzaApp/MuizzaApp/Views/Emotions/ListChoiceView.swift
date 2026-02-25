import SwiftUI

struct ListChoiceView: View {
    @StateObject private var viewModel: ListChoiceViewModel

    init(emotion: String, choice: String) {
        _viewModel = StateObject(wrappedValue: ListChoiceViewModel(emotion: emotion, choice: choice))
    }

    var body: some View {
        ZStack {
            Color(hex: "F6E5CB").ignoresSafeArea()

            ScrollView {
                VStack(spacing: 20) {
                    if viewModel.isLoading {
                        ProgressView()
                            .padding(.top, 40)
                    } else {
                        VStack(spacing: 12) {
                            Image(systemName: "sparkles")
                                .font(.title)
                                .foregroundColor(Color(hex: "7d60cb"))
                            Text(viewModel.affirmationText)
                                .font(.custom("fredoka", size: 20))
                                .fontWeight(.semibold)
                                .multilineTextAlignment(.center)
                        }
                        .padding()
                        .background(Color.white.cornerRadius(16))

                        VStack(alignment: .leading, spacing: 8) {
                            Text("Thoughts")
                                .font(.custom("fredoka", size: 18))
                                .fontWeight(.semibold)
                            Text(viewModel.thoughtsText)
                                .font(.custom("fredoka", size: 16))
                                .lineSpacing(4)
                        }
                        .padding()
                        .background(Color.white.cornerRadius(16))
                    }
                }
                .padding()
            }
        }
        .navigationTitle(viewModel.title)
        .navigationBarTitleDisplayMode(.inline)
    }
}
