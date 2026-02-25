import SwiftUI

struct Emotion: Identifiable {
    let id = UUID()
    let name: String
    let imageSource: String
    let color: Color
}
