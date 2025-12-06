namespace AdventOfCodeCore.Models.Days;

public class StopException : Exception
{
    public StopException() : base("Stop Requested")
    {
    }

    public override string? StackTrace => null;
}