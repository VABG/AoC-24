using System.Collections.Generic;

namespace AdventOfCode_24.Model.Logging
{
    public class Log
    {
        public List<string> Messages { get; private set; } = [];
        public delegate void MessageUpdated();
        public event MessageUpdated UpdateMessage;

        public Log()
        {
        }

        public void Write(string message)
        {
            Messages.Add(message);
            UpdateMessage?.Invoke();
        }
    }
}
