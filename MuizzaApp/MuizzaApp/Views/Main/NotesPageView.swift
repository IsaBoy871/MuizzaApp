import SwiftUI

struct NotesPageView: View {
    @StateObject private var viewModel = NotesViewModel()
    @Environment(\.dismiss) private var dismiss

    var body: some View {
        ZStack {
            Color(hex: "F6E5CB").ignoresSafeArea()

            VStack(spacing: 20) {
                Text("Write a Note")
                    .font(.custom("fredoka", size: 24))
                    .fontWeight(.bold)
                    .padding(.top, 20)

                TextEditor(text: $viewModel.noteContent)
                    .font(.custom("fredoka", size: 16))
                    .frame(minHeight: 200)
                    .padding()
                    .background(Color.white)
                    .cornerRadius(16)

                Button(action: {
                    Task { await viewModel.saveNote() }
                }) {
                    HStack {
                        if viewModel.isSaving {
                            ProgressView()
                                .tint(.white)
                        }
                        Text(viewModel.isSaving ? "Saving..." : "Save Note")
                            .fontWeight(.semibold)
                    }
                    .font(.custom("fredoka", size: 18))
                    .foregroundColor(.white)
                    .frame(maxWidth: .infinity)
                    .padding()
                    .background(Color(hex: "7d60cb"))
                    .cornerRadius(16)
                }
                .disabled(viewModel.isSaving)

                Spacer()
            }
            .padding()
        }
        .navigationTitle("New Note")
        .navigationBarTitleDisplayMode(.inline)
        .alert("Note Saved!", isPresented: $viewModel.showSavedAlert) {
            Button("OK") { dismiss() }
        }
        .alert("Error", isPresented: $viewModel.showErrorAlert) {
            Button("OK", role: .cancel) { }
        } message: {
            Text("Failed to save note. Please try again.")
        }
    }
}
