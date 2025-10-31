using AdventOfCodeCore.Models.Logging;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AdventOfCodeUI.ViewModels.Sections.Logging;

public class LogMessageViewModel
{
    private readonly LogMessage _logMessage;
    public Color Color { get; }
    public string Message => _logMessage.Message;

    public LogMessageViewModel(LogMessage logMessage)
    {
        _logMessage = logMessage;
        Color = new Color(_logMessage.Color.A,logMessage.Color.R, _logMessage.Color.G, _logMessage.Color.B);
    }
    
    public void CopyToClipboard(object msg)
    {
        if (msg is not Visual visual)
            return;
        var topLevel = TopLevel.GetTopLevel(visual);
        topLevel?.Clipboard?.SetTextAsync(_logMessage.Message);
    }
}