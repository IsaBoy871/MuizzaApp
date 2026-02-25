import SwiftUI

struct BrainPageView: View {
    @StateObject private var viewModel = BrainPageViewModel()

    var body: some View {
        ZStack {
            Color(hex: "F6E5CB").ignoresSafeArea()

            ScrollView {
                VStack(spacing: 24) {
                    Text(viewModel.welcomeMessage)
                        .font(.custom("fredoka", size: 28))
                        .fontWeight(.bold)
                        .padding(.top, 20)

                    VStack(alignment: .leading, spacing: 16) {
                        entryField(title: "Today's Affirmation", placeholder: "I am...", text: $viewModel.affirmation, icon: "sparkles")
                        entryField(title: "Today's Intention", placeholder: "Today I will...", text: $viewModel.intention, icon: "target")
                        entryField(title: "Today's Gratitude", placeholder: "I'm grateful for...", text: $viewModel.gratitude, icon: "heart.fill")
                    }

                    if viewModel.isCompleted {
                        HStack {
                            Image(systemName: "checkmark.circle.fill")
                                .foregroundColor(.green)
                            Text("Saved! You'll receive reminders throughout the day.")
                                .font(.custom("fredoka", size: 14))
                                .foregroundColor(.secondary)
                        }
                        .padding()
                        .background(Color.white.cornerRadius(12))
                    } else {
                        Button(action: { viewModel.saveDailyEntry() }) {
                            Text("Save & Set Reminders")
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
        .navigationTitle("Brain Check-in")
        .navigationBarTitleDisplayMode(.inline)
    }

    private func entryField(title: String, placeholder: String, text: Binding<String>, icon: String) -> some View {
        VStack(alignment: .leading, spacing: 8) {
            HStack {
                Image(systemName: icon)
                    .foregroundColor(Color(hex: "7d60cb"))
                Text(title)
                    .font(.custom("fredoka", size: 18))
                    .fontWeight(.semibold)
            }
            TextField(placeholder, text: text, axis: .vertical)
                .font(.custom("fredoka", size: 16))
                .lineLimit(3...6)
                .padding()
                .background(Color.white)
                .cornerRadius(12)
                .disabled(viewModel.isCompleted)
        }
    }
}
