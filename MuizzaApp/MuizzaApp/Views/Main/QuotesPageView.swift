import SwiftUI

struct QuotesPageView: View {
    @StateObject private var viewModel = QuotesViewModel()
    @State private var showProfile = false
    @State private var showPremiumStatus = false

    var body: some View {
        ZStack {
            Color(hex: "F6E5CB").ignoresSafeArea()

            VStack(spacing: 0) {
                headerBar

                if viewModel.hasNoInternet {
                    noInternetView
                } else {
                    ScrollView {
                        VStack(spacing: 20) {
                            affirmationsCarousel
                            feelingInput
                            emotionsGrid
                        }
                        .padding(.horizontal)
                    }
                }
            }
        }
        .navigationDestination(isPresented: $viewModel.navigateToEmotionalResponse) {
            EmotionalResponseView(feeling: viewModel.currentFeeling)
        }
        .sheet(isPresented: $showProfile) {
            ProfileView()
        }
        .sheet(isPresented: $showPremiumStatus) {
            PremiumStatusView()
        }
    }

    private var headerBar: some View {
        HStack {
            Button(action: { showProfile = true }) {
                Image(systemName: "person.circle")
                    .font(.title2)
                    .foregroundColor(.primary)
            }

            Spacer()

            Text("Muizza")
                .font(.custom("fredoka", size: 24))
                .fontWeight(.bold)

            Spacer()

            Button(action: { showPremiumStatus = true }) {
                Image(systemName: "crown")
                    .font(.title2)
                    .foregroundColor(Color(hex: "FFD700"))
            }
        }
        .padding(.horizontal)
        .padding(.vertical, 12)
    }

    private var affirmationsCarousel: some View {
        TabView(selection: $viewModel.currentPosition) {
            ForEach(Array(viewModel.affirmations.enumerated()), id: \.element.id) { index, affirmation in
                AffirmationCard(affirmation: affirmation)
                    .tag(index)
            }
        }
        .tabViewStyle(.page(indexDisplayMode: .automatic))
        .frame(height: 200)
        .onChange(of: viewModel.currentPosition) { _, newValue in
            viewModel.onPositionChanged(newValue)
        }
    }

    private var feelingInput: some View {
        VStack(spacing: 12) {
            Text("How are you feeling?")
                .font(.custom("fredoka", size: 20))
                .fontWeight(.semibold)

            HStack {
                TextField("Tell me what's on your mind...", text: $viewModel.feelingText)
                    .font(.custom("fredoka", size: 16))
                    .padding(12)
                    .background(Color.white)
                    .cornerRadius(12)

                Button(action: { viewModel.submitFeeling() }) {
                    Image(systemName: "arrow.right.circle.fill")
                        .font(.title)
                        .foregroundColor(Color(hex: "7d60cb"))
                }
            }
        }
        .padding(.vertical, 8)
    }

    private var emotionsGrid: some View {
        VStack(alignment: .leading, spacing: 12) {
            Text("Or select an emotion")
                .font(.custom("fredoka", size: 18))
                .fontWeight(.medium)

            LazyVGrid(columns: [GridItem(.flexible()), GridItem(.flexible())], spacing: 16) {
                ForEach(viewModel.emotionsList) { emotion in
                    NavigationLink(destination: EmotionDetailView(emotionName: emotion.name)) {
                        EmotionCardView(emotion: emotion)
                    }
                }
            }
        }
    }

    private var noInternetView: some View {
        VStack(spacing: 20) {
            Spacer()
            Image(systemName: "wifi.slash")
                .font(.system(size: 60))
                .foregroundColor(.gray)
            Text("No Internet Connection")
                .font(.custom("fredoka", size: 22))
            Text("Please check your connection and try again")
                .font(.custom("fredoka", size: 16))
                .foregroundColor(.secondary)
            Button("Retry") {
                Task { await viewModel.loadInitialAffirmations() }
            }
            .buttonStyle(.borderedProminent)
            Spacer()
        }
    }
}
