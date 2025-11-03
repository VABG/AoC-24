using AdventOfCodeCore.Interfaces;
using AdventOfCodeCore.Models.Visualization;

namespace AdventOfCodeCore.Models.Logging;

public class LogMessages : ILogMessages
{
    public List<LogMessage> Messages { get; } = [];

    public event Action<LogMessage> MessageLogged = delegate { };

    public void Log(string message)
    {
        Write(message, new Color(128, 128,128,255));
    }
        
    public void Error(string message)
    {
        Write(message, new Color(255, 0,0,190));
    }

    public void Success(string message)
    {
        Write(message, new Color(160, 255,0,190));
    }

    public void Write(string message, Color color)
    {
        var logMessage = new LogMessage(message, color); 
        Messages.Add(logMessage);
        MessageLogged?.Invoke(logMessage);
    }

    public void Clear()
    {
        Messages.Clear();
    }

}