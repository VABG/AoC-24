using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCodeCore.DataReading;
using AdventOfCodeCore.Models.Days;
using AdventOfCodeCore.Models.Settings;
using AdventOfCodeCore.Models.WebConnection;
using AdventOfCodeUI.ViewModels.Sections;

namespace AdventOfCodeUI.ViewModels;

public class MainViewModel : ViewModelBase
{
    private int[]? _years;
    
    public int[]? Years
    {
        get => _years;
        set
        {
            _years = value;
            OnPropertyChanged(nameof(Years));
        }
    }
    private int? _selectedYear;
    public int? SelectedYear
    {
        get => _selectedYear;
        set
        {
            _selectedYear = value;
            ChangeYear();
        }
    }

    private Day[]? _days;

    public Day[]? Days
    {
        get => _days;
        set
        {
            _days = value;
            OnPropertyChanged(nameof(Days));
        }
    }

    private Day? _selectedDay;

    public Day? SelectedDay
    {
        get => _selectedDay;
        set => ChangeDay(value);
    }

    public int[]? Parts => _selectedDay?.PartNumbers;

    private int? _selectedPart;

    public int? SelectedPart
    {
        get => _selectedPart;
        set
        {
            _selectedPart = value;
            foreach (var section in _viewSections)
                section.SetPart(value);
            OnPropertyChanged(nameof(SelectedPart));
        }
    }

    private Dictionary<int, List<Day>> _daysData = [];

    private bool _canRun;

    public bool CanRun
    {
        get => _canRun;
        set
        {
            _canRun = value;
            OnPropertyChanged(nameof(CanRun));
        }
    }

    public bool CanRunTest => !string.IsNullOrWhiteSpace(SelectedDay?.Data?.TestInput)
                   && !SelectedDay.IsRunning;

    private bool _canSwitchTest = true;

    public bool CanChangeDay
    {
        get => _canSwitchTest;
        set
        {
            _canSwitchTest = value;
            OnPropertyChanged(nameof(CanChangeDay));
        }
    }

    public LogViewModel Log { get; }
    public TestDataViewModel TestData { get; }
    public VisualizerViewModel Visualizer { get; }
    public DescriptionViewModel Description { get; }
    
    private readonly List<DayBaseViewModel> _viewSections;
    
    public SettingsViewModel? SettingsVm { get; }

    private bool _settingsOpen;

    public bool SettingsOpen
    {
        get => _settingsOpen;
        set
        {
            _settingsOpen = value;
            OnPropertyChanged(nameof(SettingsOpen));
        }
    }

    public MainViewModel()
    {
        _viewSections = [];
        Log = new LogViewModel();
        _viewSections.Add(Log);
        TestData = new TestDataViewModel(this);
        _viewSections.Add(TestData);
        Visualizer = new VisualizerViewModel();
        _viewSections.Add(Visualizer);
        Description = new DescriptionViewModel();
        _viewSections.Add(Description);
        SettingsVm =  new SettingsViewModel(this);
        if (!Settings.HasSettings)
            SettingsOpen = true;
        else
            ReadDays();
    }

    public void ReadDays()
    {
        _daysData= DaysReader.ReadYearsAndDays(Settings.User.Value.DllPath);
        Years = _daysData.Keys.ToArray();
        SelectedYear = Years.LastOrDefault();
        ChangeYear();
        SelectedDay = Days?.LastOrDefault();
    }

    private async void ChangeDay(Day? newDay)
    {
        CanRun = false;
        SelectedPart = null;

        if (_selectedDay != null)
            _selectedDay.RunComplete -= SelectedDayOnCompleteRun;

        _selectedDay = newDay;


        if (_selectedDay != null)
        {
            await _selectedDay.Load();
            if (_selectedDay.PartNumbers.Length == 0)
            {
                return;
            }

            OnPropertyChanged(nameof(Parts));

            if (Parts != null)
                _selectedPart = Parts[Parts.Length - 1];
            else
                _selectedPart = null;
            CanRun = true;

            _selectedDay.RunComplete += SelectedDayOnCompleteRun;
        }

        SelectedPart = _selectedPart;
        foreach(var section in _viewSections)
            section.SetDay(newDay);

        OnPropertyChanged(nameof(SelectedDay));
        OnPropertyChanged(nameof(CanRunTest));
    }

    private void SelectedDayOnCompleteRun()
    {
        CanChangeDay = true;
        CanRun = true;
    }

    private void ChangeYear()
    {
        OnPropertyChanged(nameof(SelectedYear));
        if (SelectedYear != null && _daysData.TryGetValue(SelectedYear.Value, out var value)) 
            Days = value.ToArray();
        SelectedDay = Days?.LastOrDefault();
    }

    public async Task Run()
    {
        await Run(false);
    }

    public async Task RunTest()
    {
        await Run(true);
    }

    public void OpenSite()
    {
        if (SelectedDay != null)
            DayInputReader.OpenSite(SelectedDay.Year, SelectedDay.DayNumber);
    }

    public void OpenSettings()
    {
        SettingsOpen = true;
    }

    private async Task Run(bool isTest)
    {
        CanRun = false;
        CanChangeDay = false;
        Log.ClearLog();
        if (SelectedPart == -1)
        {
            _selectedDay?.Log.Log("No parts implemented!");
            CanChangeDay = true;
            CanRun = true;
            return;
        }

        if (_selectedDay == null)
        {
            CanChangeDay = true;
            CanRun = true;
            return;
        }

        await _selectedDay.Load();
        if (SelectedPart == null)
        {
            CanChangeDay = true;
            CanRun = true;
            return;
        }

        _selectedDay.Run(SelectedPart.Value, isTest);
    }
}