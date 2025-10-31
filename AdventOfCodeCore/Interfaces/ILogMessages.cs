using AdventOfCodeCore.Models.Logging;
using AdventOfCodeCore.Models.Visualization;

namespace AdventOfCodeCore.Interfaces;

public interface ILogMessages
{
    void Log(string message);
    void Error(string message);
    void Success(string message);
    void Write(string message, Color color);
    void Clear();
    event Action<LogMessage> MessageLogged;
}