using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SimpleExporttoWeb
{
    public class SimpleExporttoWeb : GenericPlugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        private SimpleExporttoWebSettingsViewModel settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("7f89f66d-1cac-4d0e-9c2f-c1f97d82357a");

        public SimpleExporttoWeb(IPlayniteAPI api) : base(api)
        {
            settings = new SimpleExporttoWebSettingsViewModel(this);
            Properties = new GenericPluginProperties
            {
                HasSettings = true
            };
        }

        public override IEnumerable<MainMenuItem> GetMainMenuItems(GetMainMenuItemsArgs args)
        {
            return new List<MainMenuItem>
            {
                new MainMenuItem
                {
                    Description = "Export Game Library to Web",
                    MenuSection = "@Simple Export to Web",
                    Action = (mainMenuItem) => {
                        ExportToWeb();
                    }
                }
            };
        }

        private void ExportToWeb()
        {
            var exportDir = settings.Settings.OutputDirectory;
            if (string.IsNullOrEmpty(exportDir) || !Directory.Exists(exportDir))
            {
                PlayniteApi.Dialogs.ShowErrorMessage("Please set a valid output directory in the plugin settings.", "Export Error");
                return;
            }

            var imageDir = Path.Combine(exportDir, "images");
            if (!Directory.Exists(imageDir))
            {
                Directory.CreateDirectory(imageDir);
            }

            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine($"    <title>{settings.Settings.PageTitle}</title>");
            html.AppendLine("    <style>");
            html.AppendLine("        body { font-family: sans-serif; background-color: #1a1a1a; color: white; display: flex; flex-wrap: wrap; justify-content: center; padding: 20px; }");
            html.AppendLine("        .game-card { width: 200px; margin: 15px; background: #2a2a2a; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 8px rgba(0,0,0,0.5); transition: transform 0.2s; }");
            html.AppendLine("        .game-card:hover { transform: scale(1.05); }");
            html.AppendLine("        .game-card img { width: 100%; height: 280px; object-fit: cover; }");
            html.AppendLine("        .game-card .title { padding: 10px; text-align: center; font-size: 14px; font-weight: bold; }");
            html.AppendLine("    </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            foreach (var game in PlayniteApi.Database.Games.OrderBy(g => g.Name))
            {
                string coverSrc = ""; 
                if (!string.IsNullOrEmpty(game.CoverImage))
                {
                    var fullPath = PlayniteApi.Database.GetFullFilePath(game.CoverImage);
                    if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                    {
                        var ext = Path.GetExtension(fullPath);
                        var fileName = game.Id.ToString() + ext;
                        var destPath = Path.Combine(imageDir, fileName);
                        try {
                            File.Copy(fullPath, destPath, true);
                            coverSrc = "images/" + fileName;
                        } catch (Exception ex) {
                            logger.Error(ex, $"Failed to copy image for {game.Name}");
                        }
                    }
                }

                html.AppendLine("    <div class=\"game-card\">");
                if (!string.IsNullOrEmpty(coverSrc))
                {
                    html.AppendLine($"        <img src=\"{coverSrc}\" alt=\"{game.Name}\">");
                }
                else
                {
                    html.AppendLine("        <div style=\"height: 280px; background: #333; display: flex; align-items: center; justify-content: center; color: #666;\">No Cover</div>");
                }
                html.AppendLine($"        <div class=\"title\">{game.Name}</div>");
                html.AppendLine("    </div>");
            }

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            try
            {
                File.WriteAllText(Path.Combine(exportDir, "index.html"), html.ToString());
                PlayniteApi.Dialogs.ShowMessage("Library exported successfully!", "Export Complete");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to write index.html");
                PlayniteApi.Dialogs.ShowErrorMessage($"Failed to export: {ex.Message}", "Export Error");
            }
        }

        public override void OnGameInstalled(OnGameInstalledEventArgs args)
        {
            // Add code to be executed when game is finished installing.
        }

        public override void OnGameStarted(OnGameStartedEventArgs args)
        {
            // Add code to be executed when game is started running.
        }

        public override void OnGameStarting(OnGameStartingEventArgs args)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameStopped(OnGameStoppedEventArgs args)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameUninstalled(OnGameUninstalledEventArgs args)
        {
            // Add code to be executed when game is uninstalled.
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            // Add code to be executed when Playnite is initialized.
        }

        public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
        {
            // Add code to be executed when Playnite is shutting down.
        }

        public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
        {
            // Add code to be executed when library is updated.
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new SimpleExporttoWebSettingsView();
        }
    }
}