import SwiftUI

@MainActor
class ProfileViewModel: ObservableObject {
    @Published var userName = ""
    @Published var dailyNotificationsEnabled = true
    @Published var weeklyNotificationsEnabled = true
    @Published var selectedTimeIndex = 0
    @Published var showSaveSuccess = false
    @Published var showSignOutConfirm = false

    let availableTimes = ["4:00 AM", "5:00 AM", "6:00 AM", "7:00 AM", "8:00 AM", "9:00 AM"]

    private let userService = UserService()
    private let authService: AuthService

    init(authService: AuthService) {
        self.authService = authService
        loadUserData()
        loadNotificationSettings()
    }

    private func loadUserData() {
        Task {
            let appleUserId = UserDefaults.standard.string(forKey: "AppleUserId") ?? ""
            guard !appleUserId.isEmpty else { return }
            if let user = try? await userService.getUserByAppleId(appleUserId) {
                userName = user.name ?? ""
            }
        }
    }

    private func loadNotificationSettings() {
        dailyNotificationsEnabled = UserDefaults.standard.bool(forKey: "DailyNotifications")
        weeklyNotificationsEnabled = UserDefaults.standard.bool(forKey: "WeeklyNotifications")
        selectedTimeIndex = UserDefaults.standard.integer(forKey: "SelectedTimeIndex")
    }

    func saveUserName() async {
        let appleUserId = UserDefaults.standard.string(forKey: "AppleUserId") ?? ""
        guard !appleUserId.isEmpty else { return }
        do {
            _ = try await userService.updateUserName(appleUserId: appleUserId, name: userName)
            showSaveSuccess = true
        } catch {
            print("Error saving name: \(error)")
        }
    }

    func saveNotificationSettings() {
        UserDefaults.standard.set(dailyNotificationsEnabled, forKey: "DailyNotifications")
        UserDefaults.standard.set(weeklyNotificationsEnabled, forKey: "WeeklyNotifications")
        UserDefaults.standard.set(selectedTimeIndex, forKey: "SelectedTimeIndex")
    }

    func signOut() {
        authService.signOut()
    }
}
