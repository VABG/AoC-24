using System;
using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.WebConnection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode_24.Model.Logging;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Media.Imaging;
using System.Threading;
using System.Collections.Concurrent;

namespace AdventOfCode_24.ViewModels;

public class MainViewModel : ViewModelBase
{
    public WriteableBitmap? WriteableBitmap => _selectedDay?.Renderer?.WriteableBitmap;

    private bool _wiggleState = true;
    private Thickness _wiggleThickness;

    public Thickness WiggleThickness
    {
        get => _wiggleThickness;
        set
        {
            _wiggleThickness = value;
            OnPropertyChanged(nameof(WiggleThickness));
        }
    }

    private bool _isWaitingForScrollDelay;
    private LogMessage? _scrollTarget;

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

    private List<Day>? _days;

    public List<Day>? Days
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

    public List<int>? Parts => _selectedDay?.PartNumbers;

    private int? _selectedPart;

    public int? SelectedPart
    {
        get => _selectedPart;
        set
        {
            _selectedPart = value;
            OnPropertyChanged(nameof(TestResult));
            OnPropertyChanged(nameof(SelectedPart));
        }
    }

    private ConcurrentBag<LogMessage> _messageCache = [];
    public ObservableCollection<LogMessage>? Log = [];

    private LogMessage? _selectedLogItem;

    public LogMessage? SelectedLogItem
    {
        get => _selectedLogItem;
        set
        {
            _selectedLogItem = value;
            OnPropertyChanged(nameof(SelectedLogItem));
        }
    }

    private readonly DaysReader _daysReader;

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
        get => SelectedDay?.Data?.GetExpectedForPart(SelectedPart);
        set
        {
            if (SelectedDay is { Data: not null })
            {
                SelectedDay.Data.SetExpectedForPart(_selectedPart, value);
            }

            OnPropertyChanged(nameof(TestResult));
        }
    }

    public bool CanRunTest => (SelectedDay?.Data?.TestInput != null
                               && !SelectedDay.IsRunning);

    private bool _canSwitchTest = true;

    public bool CanSwitchTest
    {
        get => _canSwitchTest;
        set
        {
            _canSwitchTest = value;
            OnPropertyChanged(nameof(CanSwitchTest));
        }
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
        _daysReader = new DaysReader();
        Years = _daysReader.Days.Keys.ToList();
        SelectedYear = Years.Last();
        ChangeYear();
        _cookie = CookieData.ActiveCookie;
        SelectedDay = Days?.Last();
    }

    private async void ChangeDay(Day? newDay)
    {
        CanRun = false;
        SelectedPart = null;

        if (_selectedDay != null)
        {
            _selectedDay.Log.UpdateMessage -= LogUpdated;
            _selectedDay.UpdateVisuals -= VisualizationOnUpdateVisuals;
            _selectedDay.CompleteRun -= SelectedDayOnCompleteRun;
        }

        _selectedDay = newDay;
        

        if (_selectedDay != null)
        {
            await _selectedDay.Load();
            if (_selectedDay.PartNumbers.Count == 0)
            {
                return;
            }
            OnPropertyChanged(nameof(Parts));

            if (Parts != null)
                _selectedPart = Parts[Parts.Count-1];
            else
               _selectedPart = null;
            CanRun = true;
            
            _selectedDay.CompleteRun += SelectedDayOnCompleteRun;

        }

        SelectedPart = _selectedPart;

        UpdateVisualization();
        UpdateLog();

        OnPropertyChanged(nameof(SelectedDay));
        OnPropertyChanged(nameof(TestData));
        OnPropertyChanged(nameof(TestResult));
        OnPropertyChanged(nameof(CanRunTest));
    }

    private void SelectedDayOnCompleteRun()
    {
        CanSwitchTest = true;
    }

    private void VisualizationOnUpdateVisuals()
    {
        OnPropertyChanged(nameof(WriteableBitmap));
        WiggleThickness = new Thickness(0, _wiggleState ? 0 : 1);
        _wiggleState = !_wiggleState;
    }

    private void UpdateLog()
    {
        if (_selectedDay == null)
            return;

        _selectedDay.Log.UpdateMessage += LogUpdated;
        SelectedLogItem = null;
        _messageCache.Clear();
        Log?.ReplaceCollection(_selectedDay.Log.Messages);
        if (Log != null && Log.Count != 0)
            SelectedLogItem = Log?.Last();
        else
            SelectedLogItem = null;
    }

    private void UpdateVisualization()
    {
        if (_selectedDay == null)
            return;

        _selectedDay.UpdateVisuals += VisualizationOnUpdateVisuals;
    }

    private void ChangeYear()
    {
        OnPropertyChanged(nameof(SelectedYear));
        Days = _daysReader.Days[SelectedYear];
        SelectedDay = Days.Last();
    }

    private void LogUpdated(LogMessage message)
    {
        _messageCache.Add(message);

        if (_messageCache.Count > 20 || !_isWaitingForScrollDelay)
        {
            Log?.AddRange(_messageCache);
            _messageCache.Clear();
        }


        _scrollTarget = message;
        if (_isWaitingForScrollDelay)
            return;

        _isWaitingForScrollDelay = true;
        Wait();
    }

    private async Task Wait()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(10));
        SelectedLogItem = _scrollTarget;
        _isWaitingForScrollDelay = false;
    }

    public void Run()
    {
        Run(false);
    }

    public void RunTest()
    {
        Run(true);
    }

    public void OpenSite()
    {
        if (SelectedDay != null)
            InputReader.OpenSite(SelectedDay.Year, SelectedDay.DayNumber);
    }

    private async Task Run(bool isTest)
    {
        CanSwitchTest = false;
        Log?.Clear();
        if (SelectedPart == -1)
        {
            _selectedDay?.Log.Log("No parts implemented!");
            CanSwitchTest = true;
            return;
        }
        if (_selectedDay == null)
        {
            CanSwitchTest = true;
            return;
        }
        await _selectedDay.Load();
        if (SelectedPart == null)
        {
            CanSwitchTest = true;
            return;
        }
        _selectedDay.Run(SelectedPart.Value, isTest);
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