using System.IO;
using AdventOfCodeCore.Models.Settings;

namespace AdventOfCodeUI.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;

    public SettingsViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    private bool _buttonsActive;

    public bool ButtonsActive
    {
        get => _buttonsActive;
        set
        {
            _buttonsActive = value;
            OnPropertyChanged(nameof(ButtonsActive));
        }
    }
    
    private string _status = "";
    public string Status
    {
        get => _status;
        set
        {
            _status= value;
            OnPropertyChanged(nameof(Status));
        }
    }
    
    public string? Cookie
    {
        get => Settings.User.Value.Cookie;
        set
        {
            Settings.User.Value.Cookie = value;
            OnPropertyChanged(nameof(Cookie));
            UpdateButtonStatus();
        }
    }

    public string? DllFilePath
    {
        get => Settings.User.Value.DllPath;
        set
        {
            Settings.User.Value.DllPath = value;
            OnPropertyChanged(nameof(DllFilePath));
            UpdateButtonStatus();
        }
    }

    private void UpdateButtonStatus()
    {
        ButtonsActive = ShouldButtonsBeActive();
    }

    private bool ShouldButtonsBeActive()
    {
        var success = true;
        string newStatus = "";
        if (string.IsNullOrEmpty(Cookie))
        {
            success = false;
            newStatus = "Cookie not set";
        }

        if (string.IsNullOrEmpty(DllFilePath))
        {
            success = false;
            var str = !string.IsNullOrEmpty(Status) ? "\n" : "";
            newStatus +=  str + "Dll file not set";
        }
        else
        {
            var dir = new DirectoryInfo(DllFilePath);
            if (!dir.Exists)
            {
                var str = !string.IsNullOrEmpty(Status) ? "\n" : "";
                newStatus += str + "Target folder is missing";
                success = false;
            }
        }
        Status = newStatus;
        return success;
    }

    public void Cancel()
    {
        _mainViewModel.SettingsOpen = false;
    }
    
    public void Save()
    {
        Settings.SaveSettings();
        _mainViewModel.SettingsOpen = false;
    }
}