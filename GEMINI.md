# Simple Export to Web (Playnite Plugin)

## Project Overview

**Simple Export to Web** is a [Playnite](https://playnite.link/) extension that allows users to export their game library to a static HTML webpage. It generates a grid-based view of the library, including game titles and cover images.

### Key Features
*   **Static HTML Generation:** Creates a standalone `index.html` file.
*   **Image Handling:** Copies game cover images to a local `images` directory relative to the export path.
*   **Customization:** Users can configure the **Output Directory** and the **Page Title** via the plugin settings.

### Technology Stack
*   **Language:** C#
*   **Framework:** .NET Framework 4.6.2
*   **SDK:** Playnite SDK 6.2.0
*   **UI:** WPF (Windows Presentation Foundation) for Settings View

## Project Structure

*   **`SimpleExporttoWeb.cs`**: The main plugin class inheriting from `GenericPlugin`. Contains the `ExportToWeb` method which implements the core logic (HTML generation and file copying).
*   **`SimpleExporttoWebSettings.cs`**: Defines the settings model (`SimpleExporttoWebSettings`) and the view model (`SimpleExporttoWebSettingsViewModel`) responsible for saving/loading configuration.
*   **`SimpleExporttoWebSettingsView.xaml`**: The XAML file defining the user interface for the plugin settings (Output Directory and Page Title inputs).
*   **`extension.yaml`**: Plugin metadata (ID, Name, Version, etc.).

## Building and Development

### Prerequisites
*   Visual Studio (with .NET Desktop Development workload)
*   .NET Framework 4.6.2 Developer Pack

### Build Commands
This is a standard Visual Studio solution. You can build it using the IDE or via CLI:

```powershell
# Restore NuGet packages
nuget restore SimpleExporttoWeb.sln

# Build in Release mode
msbuild SimpleExporttoWeb.sln /p:Configuration=Release
```

### Debugging
To debug, you typically need to configure the project to start Playnite's executable and load the plugin. Ensure `Playnite.SDK` references are correctly resolved.

## Usage

1.  **Configure:** Open Playnite Settings > Add-ons > Simple Export to Web.
    *   Set the **Output Directory** (e.g., `C:\MyGamesWeb`).
    *   Set the **Webpage Title**.
2.  **Run:** In Playnite, open the Main Menu > Extensions > Simple Export to Web > **Export Game Library to Web**.
3.  **View:** Navigate to the output directory and open `index.html` in a web browser.
