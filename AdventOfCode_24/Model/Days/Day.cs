using AdventOfCode_24.Model.Logging;
using AdventOfCode_24.Model.Visualization;
using AdventOfCode_24.Model.WebConnection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;

namespace AdventOfCode_24.Model.Days;

public abstract class Day : IDay, IComparable<IDay>
{
    public PixelRenderer? Visualization { get; private set; }
    public LogMessages Log { get; } = new();
    public DayData? Data { get; private set; }
    public abstract int Year { get; }
    public abstract int DayNumber { get; }
    protected string[] Input = [];
    public List<int> PartNumbers => Parts.Keys.ToList();

    protected Dictionary<int, Func<string>> Parts { get; private set; } = [];

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
            Log.Log("Issue reading data:");
            Log.Log(ex.Message);
        }
    }

    public void Run(int part, bool isTest)
    {
        InputToLines(isTest ? Data?.TestInput : Data?.Input);

        Log.Messages.Clear();
        var start = DateAndTime.Now;
        Log.Log("Starting " + (isTest ? "Test" : "Run") + " for: " + Year + "." + DayNumber + "." + part + "\n" + start);
        Log.Log("...");

        string result = string.Empty;
        if (Data == null)
        {
            Log.Error("No data! Can not run Day!");
            return;
        }
        try
        {
            result = Parts[part].Invoke();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            if (ex.StackTrace != null)
                Log.Error(ex.StackTrace);
        }
        
        var end = DateAndTime.Now;
        var time = end.Subtract(start);
        Log.Log("");
        Log.Log("Run ended at:\n" + end);
        Log.Write("Time: " + time.ToString(), Colors.CornflowerBlue);
        Log.Log("");
        if (isTest)
        {
            if (result == string.Empty)
                Log.Error("Failed: No Result");
            string? expected = Data.GetExpectedForPart(part);
            if (!string.IsNullOrEmpty(expected))
            {
                if (result == expected)
                    Log.Success("Test Successful!");
                else
                    Log.Error("Test Failed!");

                Log.Log("");
                Log.Log("Expected:");
                Log.Log(expected);
            }
        }

        Log.Log("Result:");
        Log.Write(result, Colors.Orange);
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

    public void SetParts(Dictionary<int, Func<string>> functions)
    {
        Parts = functions;
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