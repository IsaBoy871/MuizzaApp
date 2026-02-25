import SwiftUI

struct ContentView: View {
    @EnvironmentObject var authService: AuthService
    @State private var hasCompletedOnboarding = UserDefaults.standard.bool(forKey: "HasCompletedOnboarding")

    var body: some View {
        Group {
            if authService.isAuthenticated && hasCompletedOnboarding {
                MainTabView()
            } else {
                OnboardingView()
            }
        }
        .onReceive(NotificationCenter.default.publisher(for: UserDefaults.didChangeNotification)) { _ in
            hasCompletedOnboarding = UserDefaults.standard.bool(forKey: "HasCompletedOnboarding")
        }
    }
}

struct MainTabView: View {
    @State private var selectedTab = 0

    var body: some View {
        NavigationStack {
            TabView(selection: $selectedTab) {
                QuotesPageView()
                    .tabItem {
                        Image(systemName: "quote.bubble")
                        Text("Quotes")
                    }
                    .tag(0)

                BrainPageView()
                    .tabItem {
                        Image(systemName: "brain.head.profile")
                        Text("Brain")
                    }
                    .tag(1)

                NotesListView()
                    .tabItem {
                        Image(systemName: "note.text")
                        Text("Notes")
                    }
                    .tag(2)
            }
            .tint(Color(hex: "F6E5CB"))
        }
    }
}
