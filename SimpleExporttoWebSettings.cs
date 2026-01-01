using Playnite.SDK;
using Playnite.SDK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleExporttoWeb
{
    public class SimpleExporttoWebSettings : ObservableObject
    {
        private string outputDirectory = string.Empty;
        private string pageTitle = "My Game Collection";

        public string OutputDirectory { get => outputDirectory; set => SetValue(ref outputDirectory, value); }
        public string PageTitle { get => pageTitle; set => SetValue(ref pageTitle, value); }
    }

    public class SimpleExporttoWebSettingsViewModel : ObservableObject, ISettings
    {
        private readonly SimpleExporttoWeb plugin;
        private SimpleExporttoWebSettings editingClone { get; set; }

        private SimpleExporttoWebSettings settings;
        public SimpleExporttoWebSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectOutputFolderCommand { get; private set; }

        public SimpleExporttoWebSettingsViewModel(SimpleExporttoWeb plugin)
        {
            // Injecting your plugin instance is required for Save/Load method because Playnite saves data to a location based on what plugin requested the operation.
            this.plugin = plugin;

            // Load saved settings.
            var savedSettings = plugin.LoadPluginSettings<SimpleExporttoWebSettings>();

            // LoadPluginSettings returns null if no saved data is available.
            if (savedSettings != null)
            {
                Settings = savedSettings;
            }
            else
            {
                Settings = new SimpleExporttoWebSettings();
            }

            SelectOutputFolderCommand = new RelayCommand(() =>
            {
                var selectedDir = plugin.PlayniteApi.Dialogs.SelectFolder();
                if (!string.IsNullOrEmpty(selectedDir))
                {
                    Settings.OutputDirectory = selectedDir;
                }
            });
        }

        public void BeginEdit()
        {
            // Code executed when settings view is opened and user starts editing values.
            editingClone = Serialization.GetClone(Settings);
        }

        public void CancelEdit()
        {
            // Code executed when user decides to cancel any changes made since BeginEdit was called.
            // This method should revert any changes made to Option1 and Option2.
            Settings = editingClone;
        }

        public void EndEdit()
        {
            // Code executed when user decides to confirm changes made since BeginEdit was called.
            // This method should save settings made to Option1 and Option2.
            plugin.SavePluginSettings(Settings);
        }

        public bool VerifySettings(out List<string> errors)
        {
            // Code execute when user decides to confirm changes made since BeginEdit was called.
            // Executed before EndEdit is called and EndEdit is not called if false is returned.
            // List of errors is presented to user if verification fails.
            errors = new List<string>();
            return true;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}