using AdventOfCodeCore.Interfaces;
using AdventOfCodeCore.Models.Visualization;

namespace AdventOfCodeCore.Models.Logging;

public class LogMessages : ILogMessages
{
    public List<LogMessage> Messages { get; } = [];

    public event Action<LogMessage> MessageLogged = delegate { };

    public void Log(string message)
    {
        Write(message, new Color(255, 128,128,128));
    }
        
    public void Error(string message)
    {
        Write(message, new Color(190, 255,0,0));
    }

    public void Success(string message)
    {
        Write(message, new Color(190, 160,255,0));
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