using Avalonia.Media;

namespace AdventOfCode_24.Model.Logging;

public class LogMessage(string message, Color color)
{
    public string Message { get; } = message;
    public Color Color { get; } = color;
}