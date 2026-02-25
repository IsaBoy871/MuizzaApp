import SwiftUI
import AuthenticationServices

struct OnboardingView: View {
    @EnvironmentObject var authService: AuthService
    @StateObject private var viewModel: OnboardingViewModel

    init() {
        _viewModel = StateObject(wrappedValue: OnboardingViewModel(authService: AuthService()))
    }

    var body: some View {
        NavigationStack {
            ZStack {
                Color(hex: "F6E5CB").ignoresSafeArea()

                TabView(selection: $viewModel.currentStep) {
                    signInStep.tag(0)
                    welcomeStep2.tag(1)
                    welcomeStep3.tag(2)
                    welcomeStep4.tag(3)
                    nameStep.tag(4)
                    premiumOnboardStep.tag(5)
                }
                .tabViewStyle(.page(indexDisplayMode: .never))
                .animation(.easeInOut, value: viewModel.currentStep)
            }
        }
        .alert("Error", isPresented: $viewModel.showError) {
            Button("OK", role: .cancel) { }
        } message: {
            Text(viewModel.errorMessage)
        }
        .environmentObject(authService)
    }

    private var signInStep: some View {
        VStack(spacing: 30) {
            Spacer()
            Image(systemName: "heart.circle.fill")
                .resizable()
                .frame(width: 120, height: 120)
                .foregroundColor(Color(hex: "FF6B6B"))

            Text("Welcome to Muizza")
                .font(.custom("fredoka", size: 32))
                .fontWeight(.bold)

            Text("Your personal emotional wellness companion")
                .font(.custom("fredoka", size: 18))
                .multilineTextAlignment(.center)
                .padding(.horizontal, 40)

            Spacer()

            SignInWithAppleButton(.signIn, onRequest: { request in
                request.requestedScopes = [.email, .fullName]
            }, onCompletion: { result in
                Task {
                    switch result {
                    case .success(let auth):
                        if let credential = auth.credential as? ASAuthorizationAppleIDCredential {
                            UserDefaults.standard.set(credential.user, forKey: "AppleUserId")
                            authService.isAuthenticated = true
                            authService.appleUserId = credential.user

                            let userService = UserService()
                            let existing = try? await userService.getUserByAppleId(credential.user)
                            if existing == nil {
                                _ = try? await userService.createUser(appleUserId: credential.user)
                            }
                            viewModel.currentStep = 1
                        }
                    case .failure:
                        viewModel.errorMessage = "Sign in failed"
                        viewModel.showError = true
                    }
                }
            })
            .signInWithAppleButtonStyle(.black)
            .frame(height: 50)
            .padding(.horizontal, 40)
            .padding(.bottom, 60)
        }
    }

    private var welcomeStep2: some View {
        VStack(spacing: 30) {
            Spacer()
            Image(systemName: "sparkles")
                .resizable()
                .frame(width: 80, height: 80)
                .foregroundColor(Color(hex: "7d60cb"))

            Text("Discover Your Inner Strength")
                .font(.custom("fredoka", size: 28))
                .fontWeight(.bold)
                .multilineTextAlignment(.center)

            Text("Get AI-powered emotional support tailored just for you")
                .font(.custom("fredoka", size: 18))
                .multilineTextAlignment(.center)
                .padding(.horizontal, 40)

            Spacer()

            Button(action: { viewModel.currentStep = 2 }) {
                Text("Next")
                    .font(.custom("fredoka", size: 20))
                    .fontWeight(.semibold)
                    .foregroundColor(.white)
                    .frame(maxWidth: .infinity)
                    .padding()
                    .background(Color(hex: "7d60cb"))
                    .cornerRadius(16)
            }
            .padding(.horizontal, 40)
            .padding(.bottom, 60)
        }
    }

    private var welcomeStep3: some View {
        VStack(spacing: 30) {
            Spacer()
            Image(systemName: "brain.head.profile")
                .resizable()
                .frame(width: 80, height: 80)
                .foregroundColor(Color(hex: "FF6B6B"))

            Text("Daily Brain Check-ins")
                .font(.custom("fredoka", size: 28))
                .fontWeight(.bold)
                .multilineTextAlignment(.center)

            Text("Set your affirmation, intention, and gratitude every day")
                .font(.custom("fredoka", size: 18))
                .multilineTextAlignment(.center)
                .padding(.horizontal, 40)

            Spacer()

            Button(action: { viewModel.currentStep = 3 }) {
                Text("Next")
                    .font(.custom("fredoka", size: 20))
                    .fontWeight(.semibold)
                    .foregroundColor(.white)
                    .frame(maxWidth: .infinity)
                    .padding()
                    .background(Color(hex: "FF6B6B"))
                    .cornerRadius(16)
            }
            .padding(.horizontal, 40)
            .padding(.bottom, 60)
        }
    }

    private var welcomeStep4: some View {
        VStack(spacing: 30) {
            Spacer()
            Image(systemName: "person.2.circle")
                .resizable()
                .frame(width: 80, height: 80)
                .foregroundColor(Color(hex: "7c82ff"))

            Text("Meet Your AI Advisors")
                .font(.custom("fredoka", size: 28))
                .fontWeight(.bold)
                .multilineTextAlignment(.center)

            Text("Choose from unique counseling styles for deeper conversations")
                .font(.custom("fredoka", size: 18))
                .multilineTextAlignment(.center)
                .padding(.horizontal, 40)

            Spacer()

            Button(action: { viewModel.currentStep = 4 }) {
                Text("Next")
                    .font(.custom("fredoka", size: 20))
                    .fontWeight(.semibold)
                    .foregroundColor(.white)
                    .frame(maxWidth: .infinity)
                    .padding()
                    .background(Color(hex: "7c82ff"))
                    .cornerRadius(16)
            }
            .padding(.horizontal, 40)
            .padding(.bottom, 60)
        }
    }

    private var nameStep: some View {
        VStack(spacing: 30) {
            Spacer()
            Text("What should we call you?")
                .font(.custom("fredoka", size: 28))
                .fontWeight(.bold)

            TextField("Your name", text: $viewModel.userName)
                .font(.custom("fredoka", size: 20))
                .padding()
                .background(Color.white)
                .cornerRadius(12)
                .padding(.horizontal, 40)

            Spacer()

            Button(action: {
                Task { await viewModel.saveNameAndContinue() }
            }) {
                Text("Let's Go!")
                    .font(.custom("fredoka", size: 20))
                    .fontWeight(.semibold)
                    .foregroundColor(.white)
                    .frame(maxWidth: .infinity)
                    .padding()
                    .background(Color(hex: "7d60cb"))
                    .cornerRadius(16)
            }
            .padding(.horizontal, 40)
            .padding(.bottom, 60)
        }
    }

    private var premiumOnboardStep: some View {
        PremiumOnboardView {
            viewModel.completeOnboarding()
        }
    }
}
