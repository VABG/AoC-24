using System.Collections.Generic;
using Avalonia.Media;

namespace AdventOfCode_24.Model.Logging
{
    public class LogMessages
    {
        public List<LogMessage> Messages { get; private set; } = [];
        public delegate void MessageUpdated(LogMessage message);
        public event MessageUpdated UpdateMessage;

        public void Log(string message)
        {
            Write(message, Colors.LightGray);;
        }
        
        public void Error(string message)
        {
            Write(message, Colors.Red);
        }

        public void Success(string message)
        {
            Write(message, Colors.GreenYellow);
        }

        public void Write(string message, Color color)
        {
            var logMessage = new LogMessage(message, color); 
            Messages.Add(logMessage);
            UpdateMessage?.Invoke(logMessage);
        }


    }
}
