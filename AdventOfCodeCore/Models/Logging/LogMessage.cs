using AdventOfCodeCore.Models.Visualization;

namespace AdventOfCodeCore.Models.Logging;

public struct LogMessage(string message, Color color)
{
    public string Message { get; } = message;
    public Color Color { get; } = color;
}