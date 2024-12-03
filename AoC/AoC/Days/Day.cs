namespace AoC.Days;

public abstract class Day : IDay
{
    public abstract void Run();

    // TODO: Replace with write Log method.
    public string Log { get; protected set; } = "";
    
    // Drawing surface thing here and help methods
}