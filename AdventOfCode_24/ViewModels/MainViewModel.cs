using System;
using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.WebConnection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode_24.Model.Logging;
using AdventOfCode_24.Model.Visualization;
using Avalonia.Media.Imaging;
using DynamicData;

namespace AdventOfCode_24.ViewModels;

public class MainViewModel : ViewModelBase
{
    private RenderControl? _renderableControl;

    public RenderControl? RenderableControl
    {
        get => _renderableControl;
        set
        {
            _renderableControl = value;
            OnPropertyChanged(nameof(RenderableControl));
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

    public ObservableCollection<LogMessage>? Log { get; } = [];
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

        if (_selectedDay != null)
        {
            _selectedDay.Log.UpdateMessage -= LogUpdated;
                _selectedDay.UpdateVisuals -= VisualizationOnUpdateVisuals;
        }

        _selectedDay = newDay;
        if (_selectedDay == null)
        {
            TestData = null;
            TestResult = null;
        }
        else
        {
            await _selectedDay.Load();
            if (Parts?.Count == 0)
            {
                SelectedPart = null;
                return;
            }

            if (Parts != null)
                SelectedPart = Parts.Last();
            else
                SelectedPart = null;
            CanRun = true;
        }

        UpdateVisualization();
        UpdateLog();

        OnPropertyChanged(nameof(SelectedDay));
        OnPropertyChanged(nameof(Parts));
        OnPropertyChanged(nameof(TestData));
        OnPropertyChanged(nameof(TestResult));
        OnPropertyChanged(nameof(CanRunTest));
    }

    private void VisualizationOnUpdateVisuals(WriteableBitmap bitmap)
    {
        if (_renderableControl == null)
        {
            _renderableControl = new RenderControl();
            OnPropertyChanged(nameof(RenderableControl));
        }
        _renderableControl.UpdateBitmap(bitmap);
        _renderableControl.Update();
    }

    private void UpdateLog()
    {
        if (_selectedDay == null)
            return;

        _selectedDay.Log.UpdateMessage += LogUpdated;
        Log?.Clear();
        Log?.AddRange(_selectedDay.Log.Messages);
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
        Log?.Add(message);
        _scrollTarget = message;
        if (_isWaitingForScrollDelay)
            return;
        _isWaitingForScrollDelay = true;
        Wait();
    }

    private async Task Wait()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1));
        SelectedLogItem = _scrollTarget;
        _isWaitingForScrollDelay = false;
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
        Log?.Clear();
        if (SelectedPart == -1)
            _selectedDay?.Log.Log("No parts implemented!");
        if (_selectedDay == null)
            return;
        await _selectedDay.Load();
        if (SelectedPart == null)
            return;
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