using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode_24.ViewModels.Sections;

public class LogViewModel : DayBaseViewModel
{
    private bool _isWaitingForScrollDelay;

    private ConcurrentBag<LogMessage> _messageCache = [];
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

    protected override void UpdateDay(Day? previous)
    {
        if (previous != null)
            previous.Log.UpdateMessage -= LogUpdated;
        SelectedLogItem = null;

        if (Day == null)
            return;

        Day.Log.UpdateMessage += LogUpdated;
        _messageCache.Clear();
        Log?.ReplaceCollection(Day.Log.Messages);
        if (Log != null && Log.Count != 0)
            SelectedLogItem = Log?.Last();
        else
            SelectedLogItem = null;
    }

    protected override void UpdatePart(int? previous)
    {
        // Do nothing (unless I properly implement part specific logs)
    }

    public void ClearLog()
    {
        Day?.Log.Messages.Clear();
        Log?.Clear();
    }

    private void LogUpdated(LogMessage message)
    {
        _messageCache.Add(message);
        if (_isWaitingForScrollDelay)
            return;

        _isWaitingForScrollDelay = true;
        WaitToUpdateLog();
    }

    private void CacheToLog()
    {
        Log?.AddRange(_messageCache.Reverse());
        _messageCache.Clear();
        SelectedLogItem = Log?.Last();
    }

    private async Task WaitToUpdateLog()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(10));
        _isWaitingForScrollDelay = false;
        CacheToLog();
    }
}
