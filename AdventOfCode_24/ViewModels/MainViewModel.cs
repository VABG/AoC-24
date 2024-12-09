using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.WebConnection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode_24.Model.Logging;
using Avalonia.Logging;
using DynamicData;

namespace AdventOfCode_24.ViewModels;

public class MainViewModel : ViewModelBase
{
    // TODO: Connect visualizer

    public List<int> Years { get; }

    private int _selectedYear;
    public int SelectedYear 
    { 
        get => _selectedYear; 
        set
        {
            _selectedYear = value;
            ChangeYear();
        }
    }

    private List<Day> _days;
    public List<Day> Days
    {
        get => _days;
        set
        {
            _days = value;
            OnPropertyChanged(nameof(Days));
        }
    }

    private Day _selectedDay;
    public Day SelectedDay
    {
        get => _selectedDay;
        set => ChangeDay(value);
    }

    private List<int> _parts;
    public List<int> Parts
    {
        get => _parts;
        set
        {
            _parts = value;
            OnPropertyChanged(nameof(Parts));
        }
    }

    private int _selectedPart;
    public int SelectedPart
    {
        get => _selectedPart;
        set
        {
            _selectedPart = value;
            OnPropertyChanged(nameof(SelectedPart));
        }
    }

    public ObservableCollection<LogMessage> Log { get; } = [];

    private AllDays _allDays;

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

    public string? TestData
    {
        get => SelectedDay?.Data?.TestInput;
        set
        {
            if (SelectedDay != null && SelectedDay.Data != null)
                SelectedDay.Data.TestInput = value;
            OnPropertyChanged(nameof(TestData));
            OnPropertyChanged(nameof(CanRunTest));
        }
    }

    public string? TestResult
    {
        get => SelectedDay?.Data?.TestResult;
        set
        {
            if (SelectedDay != null && SelectedDay.Data != null)
                SelectedDay.Data.TestResult = value;
            OnPropertyChanged(nameof(TestResult));
            OnPropertyChanged(nameof(CanRunTest));
        }
    }

    public bool CanRunTest
    {
        get => SelectedDay?.Data?.TestInput != null;
    }

    private string _cookie;
    public string Cookie
    {
        get => _cookie;
        set
        {
            _cookie = value;
            OnPropertyChanged(nameof(Cookie));
        }
    }

    public MainViewModel()
    {
        _allDays = new AllDays();
        Years = _allDays.Days.Keys.ToList();
        SelectedYear = Years.Last();
        ChangeYear();
        Cookie = CookieData.ActiveCookie;
        SelectedDay = Days.Last();
    }

    private async void ChangeDay(Day newDay)
    {
        CanRun = false;

        if (_selectedDay != null)
            _selectedDay.Log.UpdateMessage -= LogUpdated;

        _selectedDay = newDay;
        await _selectedDay.Load();
        _selectedDay.Log.UpdateMessage += LogUpdated;
        Log.Clear();
        Log.AddRange(_selectedDay.Log.Messages);
        OnPropertyChanged(nameof(SelectedDay));
        Parts = _selectedDay.PartNumbers;
        if (Parts.Count == 0)
        {
            SelectedPart = -1;
            return;
        }

        SelectedPart = Parts.Last();
        CanRun = true;

        OnPropertyChanged(nameof(TestData));
        OnPropertyChanged(nameof(TestResult));
        OnPropertyChanged(nameof(CanRunTest));
    }

    private void ChangeYear()
    {
        OnPropertyChanged(nameof(SelectedYear));
        Days = _allDays.Days[SelectedYear];
        SelectedDay = Days.Last();
    }


    private void LogUpdated(LogMessage message)
    {
        Log.Add(message);
    }

    public async void Run()
    {
        await Run(false);
    }

    public async void RunTest()
    {
        await Run(true);
    }

    public void OpenSite()
    {
        if (SelectedDay != null)
            InputReader.OpenSite(SelectedDay.Year, SelectedDay.DayNumber);
    }

    private async Task Run(bool isTest)
    {
        Log.Clear();
        if (SelectedPart == -1)
            _selectedDay.Log.Log("No parts implemented!");
        await _selectedDay.Load();
        _selectedDay.Run(SelectedPart, isTest);
    }

    public void SaveTestData()
    {
        SelectedDay?.WriteData();
    }

    public void SaveCookie()
    {
        CookieData.SetCookie(Cookie);
    }
}
