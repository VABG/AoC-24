using AdventOfCodeCore.Interfaces;
using AdventOfCodeCore.Models.Logging;

namespace AdventOfCodeCore.Models.Days;

public interface IDay
{
    int Year { get; }
    int DayNumber { get; }
    
    LogMessages Log { get; }
}