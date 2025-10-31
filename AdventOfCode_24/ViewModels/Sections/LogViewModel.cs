using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCodeCore.Models.Days;
using AdventOfCodeCore.Models.Logging;
using AdventOfCodeUI.ViewModels.Sections.Logging;

namespace AdventOfCodeUI.ViewModels.Sections;

public class LogViewModel : DayBaseViewModel
{
    private bool _isWaitingForScrollDelay;

    private readonly ConcurrentBag<LogMessageViewModel> _messageCache = [];
    public ObservableCollection<LogMessageViewModel> Log { get; } = [];

    private LogMessageViewModel? _selectedLogItem;

    public LogMessageViewModel? SelectedLogItem
    {
        get => _selectedLogItem;
        set
        {
            _selectedLogItem = value;
            OnPropertyChanged(nameof(SelectedLogItem));
        }
    }

    protected override void UpdateDay(Day? previous)
    {
        if (previous != null)
            previous.Log.MessageLogged -= LogUpdated;
        SelectedLogItem = null;

        if (Day == null)
        {
            SelectedLogItem = null;
            Log.Clear();
            _messageCache.Clear();
            return;
        }

        Day.Log.MessageLogged += LogUpdated;
        _messageCache.Clear();
        if (Day.Log.Messages.Count > 0)
            Log.ReplaceCollection(Day.Log.Messages.Select(m =>  new LogMessageViewModel(m)));
        else 
            Log.Clear();

        SelectedLogItem = Log.LastOrDefault();
        OnPropertyChanged(nameof(Log));
    }

    protected override void UpdatePart(int? previous)
    {
        // Do nothing (unless I properly implement part specific logs)
    }

    public void ClearLog()
    {
        Day?.Log.Messages.Clear();
        Log.Clear();
    }

    private void LogUpdated(LogMessage message)
    {
        _messageCache.Add(new LogMessageViewModel(message));
        if (_isWaitingForScrollDelay)
            return;

        _isWaitingForScrollDelay = true;
        WaitToUpdateLog();
    }

    private void CacheToLog()
    {
        Log.AddRange(_messageCache.Reverse());
        _messageCache.Clear();
        if (Log.Count > 0)
            SelectedLogItem = Log.LastOrDefault();
    }

    private async Task WaitToUpdateLog()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(10));
        _isWaitingForScrollDelay = false;
        CacheToLog();
    }
}
