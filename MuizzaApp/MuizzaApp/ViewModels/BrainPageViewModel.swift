import SwiftUI
import UserNotifications

@MainActor
class BrainPageViewModel: ObservableObject {
    @Published var welcomeMessage = "Hey, friend"
    @Published var affirmation = ""
    @Published var intention = ""
    @Published var gratitude = ""
    @Published var isCompleted = false

    private let dailyEntryKey = "current_daily_entry"
    private let userService = UserService()

    init() {
        loadCurrentEntry()
        Task { await loadUserName() }
    }

    private func loadUserName() async {
        do {
            let user = try await userService.getCurrentUser()
            if let name = user.name, !name.isEmpty {
                welcomeMessage = "Hey, \(name)"
            }
        } catch {
            // Keep default welcome message
        }
    }

    func loadCurrentEntry() {
        guard let data = UserDefaults.standard.data(forKey: dailyEntryKey),
              let entry = try? JSONDecoder().decode(DailyEntry.self, from: data),
              entry.expiresAt > Date() else {
            clearCurrentEntry()
            return
        }
        affirmation = entry.affirmation
        intention = entry.intention
        gratitude = entry.gratitude
        isCompleted = true
    }

    func clearCurrentEntry() {
        affirmation = ""
        intention = ""
        gratitude = ""
        isCompleted = false
        UserDefaults.standard.removeObject(forKey: dailyEntryKey)
    }

    func saveDailyEntry() {
        guard !affirmation.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty,
              !intention.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty,
              !gratitude.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty else {
            return
        }

        let entry = DailyEntry(
            affirmation: affirmation,
            intention: intention,
            gratitude: gratitude,
            createdAt: Date(),
            expiresAt: Calendar.current.startOfDay(for: Date()).addingTimeInterval(86400)
        )

        if let data = try? JSONEncoder().encode(entry) {
            UserDefaults.standard.set(data, forKey: dailyEntryKey)
        }
        isCompleted = true
        scheduleNotifications(entry: entry)
    }

    private func scheduleNotifications(entry: DailyEntry) {
        let center = UNUserNotificationCenter.current()
        center.requestAuthorization(options: [.alert, .badge, .sound]) { granted, _ in
            guard granted else { return }

            center.removeAllPendingNotificationRequests()

            let confirmContent = UNMutableNotificationContent()
            confirmContent.title = "You Classy Cat!"
            confirmContent.body = "Your daily reflections have been saved."
            let confirmTrigger = UNTimeIntervalNotificationTrigger(timeInterval: 2, repeats: false)
            center.add(UNNotificationRequest(identifier: "confirm", content: confirmContent, trigger: confirmTrigger))

            let items: [(String, String, Int)] = [
                ("Your Affirmation", entry.affirmation, 10),
                ("Your Intention", entry.intention, 14),
                ("Your Gratitude", entry.gratitude, 18),
            ]

            for (title, body, hour) in items {
                let content = UNMutableNotificationContent()
                content.title = title
                content.body = body

                var dateComponents = DateComponents()
                dateComponents.hour = hour
                let trigger = UNCalendarNotificationTrigger(dateMatching: dateComponents, repeats: true)
                center.add(UNNotificationRequest(identifier: title, content: content, trigger: trigger))
            }
        }
    }
}
