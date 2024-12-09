using System;
using System.Linq;
using AdventOfCode_24.Model.Days;

namespace AdventOfCode_24.Days;

public class Day2 : Day
{
    public override int Year => 2024;
    public override int DayNumber => 2;

    public Day2()
    {
        Parts.Add(1, Part1);    
    }

    private string Part1()
    {
        var safeReports = 0;
        int reportNr = 0;
        foreach (var line in Input)
        {
            if (string.IsNullOrEmpty(line))
                continue;
            var numberStrings = line.Split(' ');
            var numbers = numberStrings.Select(int.Parse).ToArray();
            if (IsSafe(numbers))
            {
                safeReports++;
                Log.Success(reportNr + " is Safe");
            }
            else
                Log.Error(reportNr + " is Unsafe");
            
            reportNr++;
        }
        
        return safeReports.ToString();
    }

    private bool IsSafe(int[] numbers)
    {
        var diff = numbers[1] - numbers[0];
        if (Math.Abs(diff) > 3 || diff == 0)
            return false;
        var isUpward = diff > 0;
        for (var i = 2; i < numbers.Length; i++)
        {
            var isSafe = IsSafeChange(isUpward, numbers[i] - numbers[i - 1]);
            if (!isSafe)
                return false;
        }
        return true;
    }

    private bool IsSafeChange(bool isUpward, int difference)
    {
        if (Math.Abs(difference) > 3)
            return false;
        
        switch (difference)
        {
            case > 0 when isUpward:
            case < 0 when !isUpward:
                return true;
            default:
                return false;
        }
    }
    
    
}