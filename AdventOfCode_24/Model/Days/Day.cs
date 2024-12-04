using AdventOfCode_24.Model.Logging;
using AdventOfCode_24.Model.Visualization;

namespace AdventOfCode_24.Model.Days;

public abstract class Day : IDay
{
    public PixelRenderer? Visualization { get; private set; }
    public Log Logger { get; }
    protected DayData Data { get; private set; }

    public abstract int DayNumber { get; }

    public Day()
    {
        // Load DayData
        Logger = new Log();
    }

    public abstract void Run();

    protected void Log(string message)
    {
        Logger.Write(message);
    }

    protected void MakeVisualization(int width, int height)
    {
        Visualization = new PixelRenderer(width, height);
    }

    protected void ClearVisualization()
    {
        Visualization = null;
    }
}