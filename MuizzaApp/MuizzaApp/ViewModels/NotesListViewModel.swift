import SwiftUI

@MainActor
class NotesListViewModel: ObservableObject {
    @Published var notes: [Note] = []
    @Published var noteToDelete: Note?
    @Published var showDeleteConfirmation = false

    private let notesService = NotesService.shared

    func loadNotes() async {
        do {
            notes = try await notesService.getNotes()
        } catch {
            print("Error loading notes: \(error)")
        }
    }

    func confirmDelete(_ note: Note) {
        noteToDelete = note
        showDeleteConfirmation = true
    }

    func deleteNote() async {
        guard let note = noteToDelete else { return }
        do {
            let success = try await notesService.deleteNote(id: note.id)
            if success {
                notes.removeAll { $0.id == note.id }
            }
        } catch {
            print("Error deleting note: \(error)")
        }
        noteToDelete = nil
    }
}
