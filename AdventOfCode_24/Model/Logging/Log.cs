using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_24.Model.Logging
{
    public class Log
    {
        public List<string> Messages { get; private set; }

        public Log()
        {
            Messages = [];
        }

        public void Write(string message)
        {
            Messages.Add(message);
        }
    }
}
