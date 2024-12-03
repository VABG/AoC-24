using AdventOfCode_24.Model.Logging;
using AdventOfCode_24.Model.Visualization;
using AdventOfCode_24.Model.WebConnection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode_24.Model.Days;

public abstract class Day : IDay, IComparable<IDay>
{
    public PixelRenderer? Visualization { get; private set; }
    public Log Logger { get; } = new Log();
    public DayData? Data { get; private set; } = null;
    public abstract int DayNumber { get; }
    public abstract int Year { get; }
    protected string[] Input;
    public List<int> PartNumbers => Parts.Keys.ToList();

    protected Dictionary<int, Func<string>> Parts { get; private set; } = [];

    public Day()
    {

    }

    public async Task Load()
    {
        if (Data != null)
            return;

        try
        {
            Data = await InputReader.Read(this);
        }
        catch (Exception ex)
        {
            Logger.Write("Issue reading data:");
            Logger.Write(ex.Message);
        }
    }


    public void Run(int part, bool isTest)
    {
        InputToLines(isTest ? Data?.TestInput : Data?.Input);

        Logger.Messages.Clear();
        Logger.Write("Starting Run for: " + Year + "." + DayNumber + "." + part);
        Logger.Write("...");
        var start = DateAndTime.Now;

        string result = string.Empty;
        if (Data == null)
        {
            Logger.Write("No data! Can not run Day!");
            return;
        }
        try
        {
            result = Parts[part].Invoke();
        }
        catch (Exception ex)
        {
            Logger.Write(ex.Message);
        }
        
        var end = DateAndTime.Now;
        var time = end.Subtract(start);
        Logger.Write("");
        Logger.Write("Run Ended after: " + time.ToString());
        Logger.Write("");
        if (isTest)
        {
            if (result == string.Empty)
                Logger.Write("Failed: No Result");
            if (!string.IsNullOrEmpty(Data.TestResult))
            {
                if (result == Data.TestResult)
                    Logger.Write("Test Succeeded!");
                else
                    Logger.Write("Test Failed!");

                Logger.Write("");
                Logger.Write("Expected:");
                Logger.Write(Data.TestResult);
            }
        }

        Logger.Write("Result:");
        Logger.Write(result);
    }

    protected void Log(string message)
    {
        Logger.Write(message);
    }

    private void InputToLines(string input)
    {
        Input = input.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
    }

    protected void MakeVisualization(int width, int height)
    {
        Visualization = new PixelRenderer(width, height);
    }

    protected void ClearVisualization()
    {
        Visualization = null;
    }

    public int CompareTo(IDay? other)
    {
        if (other == null)
            return 1;

        var y = Year.CompareTo(other.Year);
        if (y != 0) return y;
        return DayNumber.CompareTo(other.DayNumber);
    }

    public override string ToString()
    {
        return DayNumber.ToString();
    }

    public void WriteData()
    {
        if (Data != null)
            InputReader.WriteXml(Data, Year, DayNumber);
    }
}