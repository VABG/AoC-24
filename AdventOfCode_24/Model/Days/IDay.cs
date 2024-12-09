namespace AdventOfCode_24.Model.Days;

public interface IDay
{
    void Run(int part, bool isTest);

    int Year { get; }
    int DayNumber { get; }

}