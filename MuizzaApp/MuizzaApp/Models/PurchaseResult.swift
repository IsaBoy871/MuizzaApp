import Foundation

struct PurchaseResult {
    let success: Bool
    let errorMessage: String?
    let transactionId: String?

    static func successful(transactionId: String? = nil) -> PurchaseResult {
        PurchaseResult(success: true, errorMessage: nil, transactionId: transactionId)
    }

    static func failed(_ errorMessage: String) -> PurchaseResult {
        PurchaseResult(success: false, errorMessage: errorMessage, transactionId: nil)
    }
}
