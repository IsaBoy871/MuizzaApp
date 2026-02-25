import SwiftUI

struct DeepDiveView: View {
    @StateObject private var viewModel: DeepDiveViewModel

    init(feeling: String, advisorSystemPrompt: String, advisorName: String) {
        _viewModel = StateObject(wrappedValue: DeepDiveViewModel(
            feeling: feeling,
            advisorSystemPrompt: advisorSystemPrompt
        ))
    }

    var body: some View {
        ZStack {
            Color(hex: "F6E5CB").ignoresSafeArea()

            ScrollView {
                VStack(spacing: 20) {
                    if viewModel.isLoading {
                        VStack(spacing: 16) {
                            ProgressView()
                                .scaleEffect(1.5)
                            Text("Your advisor is thinking...")
                                .font(.custom("fredoka", size: 16))
                                .foregroundColor(.secondary)
                        }
                        .padding(.top, 60)
                    } else {
                        Text(viewModel.detailedResponse)
                            .font(.custom("fredoka", size: 16))
                            .lineSpacing(6)
                            .padding()
                            .background(Color.white.cornerRadius(16))
                    }
                }
                .padding()
            }
        }
        .navigationTitle("Deep Dive")
        .navigationBarTitleDisplayMode(.inline)
    }
}
