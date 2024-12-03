namespace AdventOfCode_24.Model.Days;

public interface IDay
{
    void Run(int part, bool isTest);

    int DayNumber { get; }
    int Year { get; }

}