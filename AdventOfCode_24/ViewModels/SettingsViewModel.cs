using System.IO;
using AdventOfCodeCore.Models.Settings;

namespace AdventOfCodeUI.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;
    private bool _okButtonActive;

    public bool OkButtonActive
    {
        get => _okButtonActive;
        set
        {
            _okButtonActive = value;
            OnPropertyChanged(nameof(OkButtonActive));
        }
    }
    
    private bool _cancelButtonActive;
    
    public bool CancelButtonActive
    {
        get => _cancelButtonActive;
        set
        {
            _cancelButtonActive = value;
            OnPropertyChanged(nameof(CancelButtonActive));
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
    
    public SettingsViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        UpdateButtonStatus();
    }

    private void UpdateButtonStatus()
    {
        OkButtonActive = ShouldOkButtonBeActive();
        CancelButtonActive = Settings.HasSettings || (OkButtonActive && Settings.HasSettings);
    }

    private bool ShouldOkButtonBeActive()
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
            var str = !string.IsNullOrEmpty(newStatus) ? "\n" : "";
            newStatus +=  str + "Dll file not set";
        }
        else
        {
            var dir = new DirectoryInfo(DllFilePath);
            if (!dir.Exists)
            {
                var str = !string.IsNullOrEmpty(newStatus) ? "\n" : "";
                newStatus += str + "Target folder is invalid";
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