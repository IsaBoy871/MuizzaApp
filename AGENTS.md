# AGENTS.md

## Cursor Cloud specific instructions

### Product overview

MuizzaApp1 is a .NET MAUI mental health/emotional wellness mobile app targeting Android, iOS, and macOS Catalyst. The backend API is hosted externally on Azure — no backend source code is in this repo.

### Build commands

Only the **Android** target can be built on Linux. iOS and macOS Catalyst require macOS with Xcode.

```bash
# Restore (Android only — must restrict TFM to avoid iOS workload errors)
dotnet restore MuizzaApp1/MuizzaApp1.csproj -p:TargetFramework=net9.0-android

# Build
dotnet build MuizzaApp1/MuizzaApp1.csproj -f net9.0-android --no-restore

# Lint / code style check
dotnet format MuizzaApp1/MuizzaApp1.csproj --verify-no-changes --no-restore
```

All commands run from `/workspace/MuizzaApp1`.

### Key caveats

- **Case-sensitivity symlink**: The `.csproj` references `Resources\raw\payment.html` (lowercase) but the directory is `Resources/Raw/` (uppercase). A symlink `Resources/raw -> Resources/Raw` is required on Linux. The update script creates it automatically.
- **No test projects**: The repository contains no automated test projects.
- **Multi-TFM restriction**: Always pass `-p:TargetFramework=net9.0-android` to `dotnet restore` or `-f net9.0-android` to `dotnet build`. Without this, MSBuild tries to restore iOS/macCatalyst workloads which are unavailable on Linux and the command will fail.
- **Pre-existing warnings**: The build produces 2 warnings from `Microsoft.NET.ILLink.Tasks` about an explicit package reference; these are pre-existing and harmless. `dotnet format` reports many pre-existing whitespace issues.
- **Running the app**: The app produces a signed APK at `bin/Debug/net9.0-android/com.companyname.muizzaapp1-Signed.apk`. It cannot be run directly on a headless Linux VM — it requires an Android emulator or physical device.

### Environment dependencies

- .NET 9 SDK (installed via `dotnet-install.sh` to `~/.dotnet`)
- MAUI Android workload (`dotnet workload install maui-android`)
- Android SDK (command-line tools + platform 35 + build-tools 35.0.0)
- OpenJDK 17
