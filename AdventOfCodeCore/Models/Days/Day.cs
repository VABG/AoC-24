using System.ComponentModel;
using System.Diagnostics;
using AdventOfCodeCore.DataReading;
using AdventOfCodeCore.Interfaces;
using AdventOfCodeCore.Models.Logging;
using AdventOfCodeCore.Models.Visualization;
using AdventOfCodeCore.Models.WebConnection;
using Microsoft.VisualBasic;

namespace AdventOfCodeCore.Models.Days;

public abstract class Day : IDay, IComparable<IDay>
{
    public IPixelRenderer? PixelRenderer { get; private set; }
    public ITextRenderer? TextRenderer { get; private set; }
    public LogMessages Log { get; } = new();
    public DayData? Data { get; private set; }
    public abstract int Year { get; }
    public abstract int DayNumber { get; }
    protected string[] Input = [];
    public int[] PartNumbers => Parts.Keys.ToArray();

    private readonly BackgroundWorker _worker;
    private int _partToRun;
    protected bool IsTest;
    public bool IsRunning { get; private set; }

    private Dictionary<int, Func<string>> Parts { get; set; } = [];
    public event  Action UpdateVisuals = delegate { };
    public event Action RunComplete =  delegate { };

    protected virtual string Part1()
    {
        return "Not Implemented";
    }

    protected virtual string Part2()
    {
        return "Not Implemented";
    }
    
    protected Day()
    {
        _worker = new BackgroundWorker();
        _worker.DoWork += WorkerOnDoWork;
    }

    private void WorkerOnDoWork(object? sender, DoWorkEventArgs e)
    {
        Run();
    }

    public async Task Load()
    {
        if (Data != null)
            return;

        try
        {
            Data = await DayInputReader.ReadDayData(this);
        }
        catch (Exception ex)
        {
            Log.Log("Issue reading data:");
            Log.Log(ex.Message);
        }
    }

    public void Run(int part, bool isTest)
    {
        _partToRun = part;
        IsTest = isTest;
        _worker.RunWorkerAsync();
    }

    private void Run()
    {
        IsRunning = true;
        var isTest = IsTest;
        var part = _partToRun;
        
        if (Data == null)
        {
            Log.Error("No data! Can not run Day!");
            IsRunning = false;
            return;
        }
        
        InputToLines(isTest ? Data?.TestInput : Data?.Input);
        PixelRenderer?.Clear(Colors.Transparent);
        Log.Clear();
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
        finally
        {
            IsRunning = false;
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
        RunComplete.Invoke();
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

        var newInput = new string[input.Length - 1];
        for (var i = 0; i < input.Length - 1; i++)
        {
            newInput[i] = input[i];
        }

        return newInput;
    }

    protected void CreatePixelRenderer(int width, int height)
    {
        PixelRenderer = RendererReader.GetPixelRenderer(width, height);
    }

    public void Render()
    {
        if (PixelRenderer != null)
            UpdateVisuals();
    }

    public int CompareTo(IDay? other)
    {
        if (other == null)
            return 1;

        var y = Year.CompareTo(other.Year);
        return y != 0 ? y : DayNumber.CompareTo(other.DayNumber);
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
            DayInputReader.WriteXml(Data, Year, DayNumber);
    }

    public void Wait(double durationSeconds)
    {
        var durationTicks = Math.Round(durationSeconds * Stopwatch.Frequency);
        var sw = Stopwatch.StartNew();

        while (sw.ElapsedTicks < durationTicks)
        {

        }
    }
}