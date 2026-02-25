import SwiftUI

struct AffirmationCard: View {
    let affirmation: Affirmation

    var body: some View {
        VStack {
            Spacer()
            Text(affirmation.text)
                .font(.custom("fredoka", size: 20))
                .fontWeight(.medium)
                .multilineTextAlignment(.center)
                .foregroundColor(.primary)
                .padding(.horizontal, 24)
            Spacer()
            if let category = affirmation.category {
                Text(category)
                    .font(.custom("fredoka", size: 14))
                    .foregroundColor(.secondary)
                    .padding(.bottom, 16)
            }
        }
        .frame(maxWidth: .infinity)
        .background(
            RoundedRectangle(cornerRadius: 20)
                .fill(Color.white)
                .shadow(color: .black.opacity(0.08), radius: 10, x: 0, y: 4)
        )
        .padding(.horizontal, 8)
    }
}
