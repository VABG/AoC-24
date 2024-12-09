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
    public LogMessages Log { get; } = new LogMessages();
    public DayData? Data { get; private set; } = null;
    public abstract int DayNumber { get; }
    public abstract int Year { get; }
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
        Log.Log("Starting " + (isTest ? "Test" : "Run") + " for: " + Year + "." + DayNumber + "." + part);
        Log.Log("...");
        var start = DateAndTime.Now;

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
            Log.Log(ex.Message);
        }
        
        var end = DateAndTime.Now;
        var time = end.Subtract(start);
        Log.Log("");
        Log.Log("Run Ended after: " + time.ToString());
        Log.Log("");
        if (isTest)
        {
            if (result == string.Empty)
                Log.Error("Failed: No Result");
            if (!string.IsNullOrEmpty(Data.TestResult))
            {
                if (result == Data.TestResult)
                    Log.Success("Test Successful!");
                else
                    Log.Error("Test Failed!");

                Log.Log("");
                Log.Log("Expected:");
                Log.Log(Data.TestResult);
            }
        }

        Log.Log("Result:");
        Log.Log(result);
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