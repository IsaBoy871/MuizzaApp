import SwiftUI

struct PremiumStatusView: View {
    @StateObject private var viewModel = PremiumStatusViewModel()
    @Environment(\.dismiss) private var dismiss

    var body: some View {
        NavigationStack {
            ZStack {
                Color(hex: "F6E5CB").ignoresSafeArea()

                ScrollView {
                    VStack(spacing: 24) {
                        Image(systemName: viewModel.isPremiumUser ? "crown.fill" : "crown")
                            .resizable()
                            .frame(width: 50, height: 50)
                            .foregroundColor(Color(hex: "FFD700"))
                            .padding(.top, 20)

                        Text(viewModel.isPremiumUser ? "Premium Member" : "Free Plan")
                            .font(.custom("fredoka", size: 24))
                            .fontWeight(.bold)

                        VStack(spacing: 16) {
                            balanceCard(title: "Search CatNips", balance: viewModel.searchBalance, icon: "magnifyingglass")
                            balanceCard(title: "Deep Dive CatNips", balance: viewModel.deepDiveBalance, icon: "bubble.left.and.bubble.right")
                        }

                        VStack(spacing: 12) {
                            Text("Get More CatNips")
                                .font(.custom("fredoka", size: 20))
                                .fontWeight(.semibold)

                            packButton(title: "Starter Pack", searches: 10, deepDives: 4, price: "£0.80")
                            packButton(title: "Popular Pack", searches: 50, deepDives: 20, price: "£4.99")
                            packButton(title: "Premium Pack", searches: 1000, deepDives: 400, price: "£40.00")
                        }

                        if !viewModel.isPremiumUser {
                            NavigationLink(destination: PremiumOnboardView()) {
                                Text("Upgrade to Premium")
                                    .font(.custom("fredoka", size: 18))
                                    .fontWeight(.semibold)
                                    .foregroundColor(.white)
                                    .frame(maxWidth: .infinity)
                                    .padding()
                                    .background(Color(hex: "7d60cb"))
                                    .cornerRadius(16)
                            }
                        }
                    }
                    .padding()
                }
            }
            .navigationTitle("Subscription")
            .navigationBarTitleDisplayMode(.inline)
            .toolbar {
                ToolbarItem(placement: .topBarTrailing) {
                    Button("Done") { dismiss() }
                }
            }
            .task { await viewModel.loadData() }
            .refreshable { await viewModel.refreshBalances() }
        }
    }

    private func balanceCard(title: String, balance: Int, icon: String) -> some View {
        HStack {
            Image(systemName: icon)
                .font(.title2)
                .foregroundColor(Color(hex: "7d60cb"))
            VStack(alignment: .leading, spacing: 4) {
                Text(title)
                    .font(.custom("fredoka", size: 14))
                    .foregroundColor(.secondary)
                Text("\(balance)")
                    .font(.custom("fredoka", size: 28))
                    .fontWeight(.bold)
            }
            Spacer()
        }
        .padding()
        .background(Color.white.cornerRadius(16))
    }

    private func packButton(title: String, searches: Int, deepDives: Int, price: String) -> some View {
        Button(action: { }) {
            HStack {
                VStack(alignment: .leading, spacing: 4) {
                    Text(title)
                        .font(.custom("fredoka", size: 16))
                        .fontWeight(.semibold)
                    Text("\(searches) searches & \(deepDives) deep dives")
                        .font(.custom("fredoka", size: 13))
                        .foregroundColor(.secondary)
                }
                Spacer()
                Text(price)
                    .font(.custom("fredoka", size: 18))
                    .fontWeight(.bold)
                    .foregroundColor(Color(hex: "7d60cb"))
            }
            .padding()
            .background(Color.white.cornerRadius(12))
        }
        .foregroundColor(.primary)
    }
}
