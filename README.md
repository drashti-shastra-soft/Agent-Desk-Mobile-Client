# AgentDesk Mobile Client

A cross-platform **.NET MAUI** mobile application that gives agents a single place to manage their **clients** and **policies** on the go. It follows the same workflows as the AgentDesk web platform in a touch-friendly mobile design and talks to the AgentDesk backend API.

---

## Table of Contents

1. [Features](#features)
2. [Tech Stack](#tech-stack)
3. [Architecture](#architecture)
4. [Project Structure](#project-structure)
5. [Getting Started](#getting-started)
6. [Configuration](#configuration)
7. [Build and Run](#build-and-run)
8. [Roadmap](#roadmap)
9. [Contributing](#contributing)
10. [License](#license)

---

## Features

### Client Management
- Add a new client with a streamlined client form
- Edit an existing client using the same page, which opens in edit mode when a client is passed in
- Multi-tab edit experience that matches the web version:
  - Client details
  - Identification
  - Financial Information
  - Health & Lifestyle
  - Documents
- Form validation with inline error messages

### Policy Management
- Policy list displayed as cards, with the blue header showing key details
- Collapsible cards: expand a single policy or expand/collapse all from the page header
- Collapsed view shows the essentials (Policy Holder, DOB, Pay Mode, Plan/Term/PPT)
- Plan / Term / PPT shown in a compact format (for example `178/99/20`)
- Server-side search from the bottom search bar
- Filter popup (Pay Mode options with an **Apply Filter** action)
- Update Policy Premium action, shown only when the pay mode allows it
- Update FUP popup with validated payment details

## Tech Stack

| Area | Technology |
|---|---|
| Framework | .NET MAUI |
| Language | C# |
| Architecture | MVVM with CommunityToolkit.Mvvm (`[ObservableProperty]`, `[RelayCommand]`) |
| UI toolkit | CommunityToolkit.Maui (Expander, popups), Syncfusion MAUI controls |
| Networking | REST API over HTTP, JSON serialization |
| Platforms | Android (iOS and others can be enabled in the project file) |

## Architecture

The app follows **MVVM**:

- **Models** (`AgentDesk.Models`): plain classes that also serve as API DTOs (for example `Client`, `Policy`)
- **ViewModels**: derive from a shared `BaseViewModel` and use source-generated observable properties and commands
- **Views / Pages**: XAML pages bound to their view models
- **Services**: API access, such as policy listing with search criteria

Notable design choices:

- A single **AddClientPage** handles both add and edit. Edit mode is driven by a `Client` query property, and each edit tab is its own `ContentView` with a child view model.
- Policy card expansion state (`IsExpanded`) lives on the `Policy` model, and the page view model exposes a `ToggleAll` command for the header button.
- Search and filtering go through the API (`SearchCriteria`) instead of filtering the loaded list locally.
- Validation in popup view models follows one pattern: `[Required]` attributes on observable properties, with error helpers such as `GetFirstError` and `HasXError`.

## Project Structure

> Adjust folder names to match your solution.

```
.
├── Models/          # Data models and API DTOs
├── ViewModels/      # BaseViewModel and per-page view models
├── Pages/           # Full-screen pages (Client form, Policy page, ...)
├── Views/           # Reusable ContentViews (edit tabs, cards)
├── Popups/          # Popup pages (filters, Update FUP, ...)
├── Services/        # API and application services
├── Platforms/       # Platform-specific code
├── Resources/       # Fonts, images, styles
├── App.xaml
├── MauiProgram.cs
└── AgentDesk.csproj
```

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) version matching the project's target framework
- .NET MAUI workload:
  ```bash
  dotnet workload install maui
  ```
- Visual Studio 2022 (latest) with the **.NET Multi-platform App UI development** workload, or VS Code with the **.NET MAUI** extension
- Android SDK with an emulator or a physical device
- Access to the AgentDesk API (base URL and credentials)

### Installation

```bash
# 1. Clone the repository
git clone <repository-url>
cd Agent-Desk-Mobile-Client

# 2. Restore dependencies
dotnet restore
```

## Configuration

| Setting | Notes |
|---|---|
| API base URL | Set the AgentDesk API environment in your service or constants class |
| Syncfusion license key | Register at startup in `MauiProgram.cs` if your Syncfusion packages require it |
| Secrets and signing | Never commit passwords, API keys, or keystores |

## Build and Run

```bash
# Debug build
dotnet build -f net9.0-android -c Debug

# Build and run on a device or emulator
dotnet build -f net9.0-android -t:Run

# Release build
dotnet publish -f net9.0-android -c Release
```

> Replace `net9.0-android` with the target framework in your `.csproj`.

In Visual Studio, open the `.sln` file, choose a device or emulator, and press **F5**.

## Roadmap

- [ ] Complete the multi-tab client edit experience (Identification, Financial Information, Health & Lifestyle, Documents)
- [ ] Add unit tests for view models and services
- [ ] Set up CI (build and test) with GitHub Actions

## Contributing

1. Fork the project
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m "Add your feature"`
4. Push the branch: `git push origin feature/your-feature`
5. Open a Pull Request

## License

License to be defined. Add a `LICENSE` file to specify usage terms.
