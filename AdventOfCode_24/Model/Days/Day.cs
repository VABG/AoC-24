using AdventOfCode_24.Model.Logging;
using AdventOfCode_24.Model.Visualization;
using AdventOfCode_24.Model.WebConnection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace AdventOfCode_24.Model.Days;

public abstract class Day : IDay, IComparable<IDay>
{
    protected PixelRenderer? Visualization { get; private set; }
    public LogMessages Log { get; } = new();
    public DayData? Data { get; private set; }
    public abstract int Year { get; }
    public abstract int DayNumber { get; }
    protected string[] Input = [];
    public List<int> PartNumbers => Parts.Keys.ToList();

    private Dictionary<int, Func<string>> Parts { get; set; } = [];
    
    public delegate void VisualsUpdated(WriteableBitmap bitmap);
    public event VisualsUpdated UpdateVisuals;

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
        if (Data == null)
        {
            Log.Error("No data! Can not run Day!");
            return;
        }
        
        InputToLines(isTest ? Data?.TestInput : Data?.Input);
        Visualization?.Clear(Colors.Transparent);
        Log.Messages.Clear();
        var start = DateAndTime.Now;
        Log.Log("Starting " + (isTest ? "Test" : "Run") + " for: " + Year + "." + DayNumber + "." + part + "\n" +
                start);
        Log.Log("...");

        var result = string.Empty;

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
            UpdateTestInfo(result, part);

        Log.Log("Result:");
        Log.Write(result, Colors.Orange);
    }

    private void UpdateTestInfo(string result, int part)
    {
        if (result == string.Empty)
        {
            Log.Error("Failed: No Result");
            return;
        }
        
        var expected = Data?.GetExpectedForPart(part);
        if (string.IsNullOrEmpty(expected)) 
            return;
        
        if (result == expected)
            Log.Success("Test Successful!");
        else
            Log.Error("Test Failed!");

        Log.Log("");
        Log.Log("Expected:");
        Log.Log(expected);
    }

    private void InputToLines(string? input)
    {
        if (input == null)
        {
            Input = [];
            return;
        }
        var splitInput = input.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
        Input = ClearEmptyLineAtEnd(splitInput);
    }

    private string[] ClearEmptyLineAtEnd(string[] input)
    {
        if (!string.IsNullOrEmpty(input.Last()))
            return input;

        string[] newInput = new string[input.Length - 1];
        for (int i = 0; i < input.Length - 1; i++)
        {
            newInput[i] = input[i];
        }

        return newInput;
    }

    protected void MakeVisualization(int width, int height)
    {
        Visualization = new PixelRenderer(width, height);
    }

    protected void Render()
    {
        if (Visualization != null)
            UpdateVisuals(Visualization.WriteableBitmap);
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