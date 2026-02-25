import SwiftUI

struct NotesListView: View {
    @StateObject private var viewModel = NotesListViewModel()
    @State private var showNewNote = false

    var body: some View {
        ZStack {
            Color(hex: "F6E5CB").ignoresSafeArea()

            VStack {
                if viewModel.notes.isEmpty {
                    VStack(spacing: 16) {
                        Spacer()
                        Image(systemName: "note.text")
                            .font(.system(size: 50))
                            .foregroundColor(.secondary)
                        Text("No notes yet")
                            .font(.custom("fredoka", size: 20))
                        Text("Tap + to create your first note")
                            .font(.custom("fredoka", size: 16))
                            .foregroundColor(.secondary)
                        Spacer()
                    }
                } else {
                    List {
                        ForEach(viewModel.notes) { note in
                            VStack(alignment: .leading, spacing: 8) {
                                Text(note.content)
                                    .font(.custom("fredoka", size: 16))
                                    .lineLimit(3)
                                Text(note.createdAt, style: .date)
                                    .font(.custom("fredoka", size: 12))
                                    .foregroundColor(.secondary)
                            }
                            .padding(.vertical, 4)
                            .swipeActions(edge: .trailing) {
                                Button(role: .destructive) {
                                    viewModel.confirmDelete(note)
                                } label: {
                                    Label("Delete", systemImage: "trash")
                                }
                            }
                        }
                    }
                    .listStyle(.plain)
                    .scrollContentBackground(.hidden)
                }
            }
        }
        .navigationTitle("Notes")
        .navigationBarTitleDisplayMode(.inline)
        .toolbar {
            ToolbarItem(placement: .topBarTrailing) {
                NavigationLink(destination: NotesPageView()) {
                    Image(systemName: "plus")
                }
            }
        }
        .task { await viewModel.loadNotes() }
        .alert("Delete Note?", isPresented: $viewModel.showDeleteConfirmation) {
            Button("Delete", role: .destructive) {
                Task { await viewModel.deleteNote() }
            }
            Button("Cancel", role: .cancel) { }
        } message: {
            Text("This action cannot be undone.")
        }
    }
}
