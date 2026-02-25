import SwiftUI

struct EmotionCardView: View {
    let emotion: Emotion

    var body: some View {
        VStack(spacing: 8) {
            Image(emotion.imageSource)
                .resizable()
                .scaledToFit()
                .frame(width: 60, height: 60)
                .clipShape(Circle())
                .overlay(Circle().stroke(emotion.color, lineWidth: 2))

            Text(emotion.name)
                .font(.custom("fredoka", size: 16))
                .fontWeight(.medium)
                .foregroundColor(.primary)
        }
        .frame(maxWidth: .infinity)
        .padding(.vertical, 16)
        .background(
            RoundedRectangle(cornerRadius: 16)
                .fill(emotion.color.opacity(0.15))
        )
    }
}
