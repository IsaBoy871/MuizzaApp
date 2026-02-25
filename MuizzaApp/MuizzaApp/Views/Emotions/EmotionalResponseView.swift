import SwiftUI

struct EmotionalResponseView: View {
    @StateObject private var viewModel: EmotionalResponseViewModel
    @Environment(\.dismiss) private var dismiss

    init(feeling: String) {
        _viewModel = StateObject(wrappedValue: EmotionalResponseViewModel(feeling: feeling))
    }

    var body: some View {
        ZStack {
            Color(hex: "F6E5CB").ignoresSafeArea()

            ScrollView {
                VStack(spacing: 20) {
                    Text("Feeling: \(viewModel.currentFeeling)")
                        .font(.custom("fredoka", size: 18))
                        .foregroundColor(.secondary)

                    if viewModel.isLoading {
                        ProgressView("Generating your response...")
                            .padding(.top, 40)
                    } else if let error = viewModel.errorMessage {
                        VStack(spacing: 16) {
                            Image(systemName: "exclamationmark.triangle")
                                .font(.system(size: 40))
                                .foregroundColor(.orange)
                            Text(error)
                                .font(.custom("fredoka", size: 16))
                                .multilineTextAlignment(.center)
                        }
                        .padding(.top, 40)
                    } else if viewModel.hasResponse {
                        responseContent
                    }

                    HStack {
                        Text("CatNips: \(viewModel.searchBalance)")
                            .font(.custom("fredoka", size: 14))
                            .foregroundColor(.secondary)
                    }
                    .padding(.top, 8)
                }
                .padding()
            }
        }
        .navigationTitle("Response")
        .navigationBarTitleDisplayMode(.inline)
    }

    private var responseContent: some View {
        VStack(spacing: 20) {
            VStack(spacing: 12) {
                Image(systemName: "sparkles")
                    .font(.title)
                    .foregroundColor(Color(hex: "7d60cb"))
                Text(viewModel.affirmation)
                    .font(.custom("fredoka", size: 22))
                    .fontWeight(.bold)
                    .multilineTextAlignment(.center)
            }
            .padding()
            .background(Color.white.cornerRadius(16))

            VStack(alignment: .leading, spacing: 8) {
                Text(viewModel.explanation)
                    .font(.custom("fredoka", size: 16))
                    .lineSpacing(4)
            }
            .padding()
            .background(Color.white.cornerRadius(16))

            NavigationLink(destination: AdvisorSelectionView(feeling: viewModel.currentFeeling)) {
                HStack {
                    Image(systemName: "bubble.left.and.bubble.right")
                    Text("Deep Dive")
                        .fontWeight(.semibold)
                }
                .font(.custom("fredoka", size: 18))
                .foregroundColor(.white)
                .frame(maxWidth: .infinity)
                .padding()
                .background(Color(hex: "7d60cb"))
                .cornerRadius(16)
            }
        }
    }
}
