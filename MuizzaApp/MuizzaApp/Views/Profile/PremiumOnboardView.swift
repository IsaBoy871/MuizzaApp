import SwiftUI

struct PremiumOnboardView: View {
    @StateObject private var viewModel = PremiumOnboardViewModel()
    var onComplete: (() -> Void)?

    var body: some View {
        ZStack {
            Color(hex: "F6E5CB").ignoresSafeArea()

            ScrollView {
                VStack(spacing: 24) {
                    Image(systemName: "crown.fill")
                        .resizable()
                        .frame(width: 60, height: 60)
                        .foregroundColor(Color(hex: "FFD700"))
                        .padding(.top, 30)

                    Text("Unlock Premium")
                        .font(.custom("fredoka", size: 28))
                        .fontWeight(.bold)

                    VStack(spacing: 12) {
                        featureRow(icon: "magnifyingglass", text: "500 searches per month")
                        featureRow(icon: "bubble.left.and.bubble.right", text: "200 deep dives per month")
                        featureRow(icon: "bell", text: "Personalized notifications")
                        featureRow(icon: "note.text", text: "Unlimited notes")
                    }
                    .padding()
                    .background(Color.white.cornerRadius(16))

                    HStack(spacing: 16) {
                        planCard(title: "Monthly", price: "£6.99", period: "/mo", isSelected: viewModel.isMonthlySelected) {
                            viewModel.isMonthlySelected = true
                        }
                        planCard(title: "Yearly", price: "£48.00", period: "/yr", isSelected: !viewModel.isMonthlySelected) {
                            viewModel.isMonthlySelected = false
                        }
                    }

                    Button(action: {
                        Task {
                            await viewModel.processSubscription()
                            onComplete?()
                        }
                    }) {
                        Text("Start Free Trial")
                            .font(.custom("fredoka", size: 20))
                            .fontWeight(.semibold)
                            .foregroundColor(.white)
                            .frame(maxWidth: .infinity)
                            .padding()
                            .background(Color(hex: "7d60cb"))
                            .cornerRadius(16)
                    }

                    Button(action: {
                        UserDefaults.standard.set(true, forKey: "HasCompletedOnboarding")
                        onComplete?()
                    }) {
                        Text("Skip for now")
                            .font(.custom("fredoka", size: 16))
                            .foregroundColor(.secondary)
                    }
                }
                .padding()
            }
        }
    }

    private func featureRow(icon: String, text: String) -> some View {
        HStack(spacing: 12) {
            Image(systemName: icon)
                .foregroundColor(Color(hex: "7d60cb"))
                .frame(width: 24)
            Text(text)
                .font(.custom("fredoka", size: 16))
            Spacer()
        }
    }

    private func planCard(title: String, price: String, period: String, isSelected: Bool, action: @escaping () -> Void) -> some View {
        Button(action: action) {
            VStack(spacing: 8) {
                Text(title)
                    .font(.custom("fredoka", size: 16))
                    .fontWeight(.semibold)
                HStack(alignment: .lastTextBaseline, spacing: 2) {
                    Text(price)
                        .font(.custom("fredoka", size: 24))
                        .fontWeight(.bold)
                    Text(period)
                        .font(.custom("fredoka", size: 14))
                }
            }
            .foregroundColor(isSelected ? .white : .primary)
            .frame(maxWidth: .infinity)
            .padding()
            .background(
                RoundedRectangle(cornerRadius: 16)
                    .fill(isSelected ? Color(hex: "7d60cb") : Color.white)
            )
            .overlay(
                RoundedRectangle(cornerRadius: 16)
                    .stroke(Color(hex: "7d60cb"), lineWidth: isSelected ? 0 : 2)
            )
        }
    }
}
