import Foundation

class NotesService {
    static let shared = NotesService()
    private let network = NetworkService.shared

    func saveNote(content: String) async throws -> Bool {
        let appleUserId = UserDefaults.standard.string(forKey: "AppleUserId") ?? ""
        guard !appleUserId.isEmpty else { return false }

        struct NoteContent: Encodable {
            let content: String
            let userId: String

            enum CodingKeys: String, CodingKey {
                case content = "Content"
                case userId = "UserId"
            }
        }

        let url = URL(string: AppConfiguration.apiBaseURL)!.appendingPathComponent("api/Notes")
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = try JSONEncoder().encode(NoteContent(content: content, userId: appleUserId))
        let (_, response) = try await URLSession.shared.data(for: request)
        guard let httpResponse = response as? HTTPURLResponse else { return false }
        return (200...299).contains(httpResponse.statusCode)
    }

    func getNotes() async throws -> [Note] {
        let appleUserId = UserDefaults.standard.string(forKey: "AppleUserId") ?? ""
        guard !appleUserId.isEmpty else { return [] }

        let url = URL(string: AppConfiguration.apiBaseURL)!.appendingPathComponent("api/Notes")
        var components = URLComponents(url: url, resolvingAgainstBaseURL: false)!
        components.queryItems = [URLQueryItem(name: "userId", value: appleUserId)]

        let (data, _) = try await URLSession.shared.data(from: components.url!)
        let decoder = JSONDecoder()
        decoder.dateDecodingStrategy = .iso8601
        return try decoder.decode([Note].self, from: data)
    }

    func updateNote(_ note: Note) async throws -> Bool {
        let appleUserId = UserDefaults.standard.string(forKey: "AppleUserId") ?? ""
        guard !appleUserId.isEmpty else { return false }

        struct NoteUpdate: Encodable {
            let content: String
            let userId: String

            enum CodingKeys: String, CodingKey {
                case content = "Content"
                case userId = "UserId"
            }
        }

        let url = URL(string: AppConfiguration.apiBaseURL)!.appendingPathComponent("api/Notes/\(note.id)")
        var request = URLRequest(url: url)
        request.httpMethod = "PUT"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = try JSONEncoder().encode(NoteUpdate(content: note.content, userId: appleUserId))
        let (_, response) = try await URLSession.shared.data(for: request)
        guard let httpResponse = response as? HTTPURLResponse else { return false }
        return (200...299).contains(httpResponse.statusCode)
    }

    func deleteNote(id: Int) async throws -> Bool {
        let appleUserId = UserDefaults.standard.string(forKey: "AppleUserId") ?? ""
        guard !appleUserId.isEmpty else { return false }
        return try await network.delete("api/Notes/\(id)?userId=\(appleUserId)")
    }
}
