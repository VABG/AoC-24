using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_24.Model.Days
{
    [Serializable]
    public class TestResult
    {
        public int Part { get; set; }
        public string? Result { get; set; }
    }
    
    [Serializable]
    public class DayData
    {
        public DayData() { }

        public DayData(string input)
        {
            Input = input;
        }

        public string Input { get; set; } = string.Empty;
        public string? TestInput { get; set; }
        public List<TestResult> TestResults { get; set; } = [];

        public string? GetExpectedForPart(int? part)
        {
            if (TestResults.Count == 0)
                return null;
            try
            {
                return TestResults.First(t => t.Part == part).Result;
            }
            catch
            {
                return null;
            }
        }

        public void SetExpectedForPart(int? part, string? expected)
        {
            if (part == null)
                return;
            if (TestResults.Count == 0 || !TestResults.Exists(t => t.Part == part))
                TestResults.Add(new TestResult() { Part = part.Value, Result = expected });
            else
                TestResults.First(t => t.Part == part).Result = expected;
        }
    }
}
