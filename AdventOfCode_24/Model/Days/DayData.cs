using System;

namespace AdventOfCode_24.Model.Days
{
    [Serializable]
    public class DayData
    {
        public DayData() { }

        public DayData(string input)
        {
            Input = input;
        }

        public string Input { get; set; }
        public string? TestInput { get; set; }
        public string? TestResult { get; set; }
    }
}
