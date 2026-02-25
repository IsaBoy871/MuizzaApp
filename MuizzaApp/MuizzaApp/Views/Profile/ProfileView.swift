import SwiftUI

struct ProfileView: View {
    @EnvironmentObject var authService: AuthService
    @StateObject private var viewModel: ProfileViewModel
    @Environment(\.dismiss) private var dismiss

    init() {
        _viewModel = StateObject(wrappedValue: ProfileViewModel(authService: AuthService()))
    }

    var body: some View {
        NavigationStack {
            ZStack {
                Color(hex: "F6E5CB").ignoresSafeArea()

                ScrollView {
                    VStack(spacing: 24) {
                        Image(systemName: "person.circle.fill")
                            .resizable()
                            .frame(width: 80, height: 80)
                            .foregroundColor(Color(hex: "7d60cb"))
                            .padding(.top, 20)

                        VStack(alignment: .leading, spacing: 8) {
                            Text("Your Name")
                                .font(.custom("fredoka", size: 16))
                                .foregroundColor(.secondary)
                            HStack {
                                TextField("Enter your name", text: $viewModel.userName)
                                    .font(.custom("fredoka", size: 18))
                                    .padding()
                                    .background(Color.white)
                                    .cornerRadius(12)

                                Button("Save") {
                                    Task { await viewModel.saveUserName() }
                                }
                                .font(.custom("fredoka", size: 16))
                                .foregroundColor(.white)
                                .padding(.horizontal, 16)
                                .padding(.vertical, 12)
                                .background(Color(hex: "7d60cb"))
                                .cornerRadius(12)
                            }
                        }

                        VStack(alignment: .leading, spacing: 16) {
                            Text("Notifications")
                                .font(.custom("fredoka", size: 20))
                                .fontWeight(.semibold)

                            Toggle(isOn: $viewModel.dailyNotificationsEnabled) {
                                Text("Daily Reminders")
                                    .font(.custom("fredoka", size: 16))
                            }
                            .onChange(of: viewModel.dailyNotificationsEnabled) { _, _ in
                                viewModel.saveNotificationSettings()
                            }

                            Toggle(isOn: $viewModel.weeklyNotificationsEnabled) {
                                Text("Weekly Summary")
                                    .font(.custom("fredoka", size: 16))
                            }
                            .onChange(of: viewModel.weeklyNotificationsEnabled) { _, _ in
                                viewModel.saveNotificationSettings()
                            }

                            Picker("Wake-up Time", selection: $viewModel.selectedTimeIndex) {
                                ForEach(0..<viewModel.availableTimes.count, id: \.self) { index in
                                    Text(viewModel.availableTimes[index]).tag(index)
                                }
                            }
                            .onChange(of: viewModel.selectedTimeIndex) { _, _ in
                                viewModel.saveNotificationSettings()
                            }
                        }
                        .padding()
                        .background(Color.white.cornerRadius(16))

                        Button(action: {
                            viewModel.showSignOutConfirm = true
                        }) {
                            Text("Sign Out")
                                .font(.custom("fredoka", size: 18))
                                .foregroundColor(.red)
                                .frame(maxWidth: .infinity)
                                .padding()
                                .background(Color.white.cornerRadius(16))
                        }
                    }
                    .padding()
                }
            }
            .navigationTitle("Profile")
            .navigationBarTitleDisplayMode(.inline)
            .toolbar {
                ToolbarItem(placement: .topBarTrailing) {
                    Button("Done") { dismiss() }
                }
            }
            .alert("Sign Out", isPresented: $viewModel.showSignOutConfirm) {
                Button("Sign Out", role: .destructive) {
                    viewModel.signOut()
                    dismiss()
                }
                Button("Cancel", role: .cancel) { }
            } message: {
                Text("Are you sure you want to sign out?")
            }
            .alert("Success", isPresented: $viewModel.showSaveSuccess) {
                Button("OK", role: .cancel) { }
            } message: {
                Text("Name updated successfully")
            }
        }
    }
}
