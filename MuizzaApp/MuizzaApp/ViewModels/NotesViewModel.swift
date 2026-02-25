import SwiftUI

@MainActor
class NotesViewModel: ObservableObject {
    @Published var noteContent = ""
    @Published var isSaving = false
    @Published var showSavedAlert = false
    @Published var showErrorAlert = false

    private let notesService = NotesService.shared

    func saveNote() async {
        guard !noteContent.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty else { return }
        isSaving = true
        do {
            let success = try await notesService.saveNote(content: noteContent)
            if success {
                noteContent = ""
                showSavedAlert = true
            } else {
                showErrorAlert = true
            }
        } catch {
            showErrorAlert = true
        }
        isSaving = false
    }
}
