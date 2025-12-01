using AdventOfCodeCore.Models.Days;

namespace Problems._2024;

public class Day2 : Day
{
    public override int Year => 2024;
    public override int DayNumber => 2;

    protected override string Part1()
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

    protected override string Part2()
    {
        var safeReports = 0;
        var reportNr = 0;
        foreach (var line in Input)
        {
            if (string.IsNullOrEmpty(line))
                continue;
            var numberStrings = line.Split(' ');
            var numbers = numberStrings.Select(int.Parse).ToArray();
            if (numbers.Length == 0)
                continue;
            if (CheckSafe3(numbers, true) || CheckSafe3(numbers, false))
            {
                safeReports++;
                Log.Success(reportNr + " is Safe:" + line);
            }
            else
                Log.Error(reportNr + " is Unsafe:" + line);

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

    private bool CheckSafe3(int[] numbers, bool upwards)
    {
        List<int> numberList = numbers.ToList();

        for (int skip = 0; skip < numbers.Length; skip++)
        {
            var numberCopy = numberList.ToList();
            numberCopy.RemoveAt(skip);

            bool success = true;
            for (int i = 1; i < numberCopy.Count; i++)
            {
                int diff = numberCopy[i] - numberCopy[i - 1];
                if (!IsSafeChange(upwards, diff))
                {
                    success = false;
                    break;
                }
            }

            if (success)
                return true;
        }

        return false;
    }

    private bool IsUnsafe(int diff, bool upwards)
    {
        return diff == 0 || Math.Abs(diff) > 3 || (upwards ? diff < 0 : diff > 0);
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