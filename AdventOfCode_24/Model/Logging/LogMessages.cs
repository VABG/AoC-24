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
            UpdateMessage?.Invoke(logMessage);
        }


    }
}
