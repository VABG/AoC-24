using Avalonia.Media;
using Avalonia;
using Avalonia.Controls;

namespace AdventOfCode_24.Model.Logging;

public struct LogMessage(string message, Color color)
{
    public string Message { get; } = message;
    public Color Color { get; } = color;

    public void CopyToClipboard(object msg)
    {
        if (msg is not Visual visual)
            return;
        var topLevel = TopLevel.GetTopLevel(visual);
        topLevel?.Clipboard?.SetTextAsync(Message);
    }
}