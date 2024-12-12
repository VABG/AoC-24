using AdventOfCode_24.Model.Logging;
using AdventOfCode_24.Model.Visualization;
using AdventOfCode_24.Model.WebConnection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System.Diagnostics;

namespace AdventOfCode_24.Model.Days;

public abstract class Day : IDay, IComparable<IDay>
{
    public PixelRenderer? Renderer { get; private set; }
    public LogMessages Log { get; } = new();
    public DayData? Data { get; private set; }
    public abstract int Year { get; }
    public abstract int DayNumber { get; }
    protected string[] Input = [];
    public List<int> PartNumbers => Parts.Keys.ToList();

    private readonly BackgroundWorker _worker;
    private int _partToRun;
    protected bool IsTest;
    public bool IsRunning { get; private set; }

    private Dictionary<int, Func<string>> Parts { get; set; } = [];
    
    public delegate void VisualsUpdated();
    public event VisualsUpdated UpdateVisuals;
    
    public delegate void RunComplete();
    public event RunComplete CompleteRun;

    public Day()
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
        Renderer?.Clear(Colors.Transparent);
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
        CompleteRun();
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

    protected void CreateRenderer(int width, int height)
    {
        Renderer = new PixelRenderer(width, height);
    }

    public void Render()
    {
        if (Renderer != null)
            UpdateVisuals();
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

    public void Wait(double durationSeconds)
    {
        var durationTicks = Math.Round(durationSeconds * Stopwatch.Frequency);
        var sw = Stopwatch.StartNew();

        while (sw.ElapsedTicks < durationTicks)
        {

        }
    }
}