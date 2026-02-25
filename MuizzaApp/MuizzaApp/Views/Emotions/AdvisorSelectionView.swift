import SwiftUI

struct AdvisorSelectionView: View {
    @StateObject private var viewModel: AdvisorSelectionViewModel

    init(feeling: String) {
        _viewModel = StateObject(wrappedValue: AdvisorSelectionViewModel(feeling: feeling))
    }

    var body: some View {
        ZStack {
            Color(hex: "F6E5CB").ignoresSafeArea()

            ScrollView {
                VStack(spacing: 20) {
                    Text("Choose Your Advisor")
                        .font(.custom("fredoka", size: 24))
                        .fontWeight(.bold)

                    Text("Deep Dive CatNips: \(viewModel.deepDiveBalance)")
                        .font(.custom("fredoka", size: 14))
                        .foregroundColor(.secondary)

                    ForEach(viewModel.advisors) { advisor in
                        NavigationLink(destination: DeepDiveView(
                            feeling: viewModel.feeling,
                            advisorSystemPrompt: advisor.systemPrompt,
                            advisorName: advisor.name
                        )) {
                            AdvisorCard(advisor: advisor)
                        }
                        .simultaneousGesture(TapGesture().onEnded {
                            Task { await viewModel.decrementDeepDiveBalance() }
                        })
                    }
                }
                .padding()
            }
        }
        .navigationTitle("Advisors")
        .navigationBarTitleDisplayMode(.inline)
    }
}

struct AdvisorCard: View {
    let advisor: Advisor

    var body: some View {
        HStack(spacing: 16) {
            Image(advisor.imagePath)
                .resizable()
                .scaledToFill()
                .frame(width: 60, height: 60)
                .clipShape(Circle())
                .overlay(Circle().stroke(Color(hex: "7d60cb"), lineWidth: 2))

            VStack(alignment: .leading, spacing: 4) {
                Text(advisor.name)
                    .font(.custom("fredoka", size: 18))
                    .fontWeight(.semibold)
                    .foregroundColor(.primary)
                Text(advisor.description)
                    .font(.custom("fredoka", size: 14))
                    .foregroundColor(.secondary)
            }

            Spacer()

            Image(systemName: "chevron.right")
                .foregroundColor(.secondary)
        }
        .padding()
        .background(Color.white.cornerRadius(16))
    }
}
