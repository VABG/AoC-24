using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode_24.Model.Days;
using Avalonia.Media;

namespace AdventOfCode_24.Days;

public class Day3 : Day
{
    public override int Year => 2024;
    public override int DayNumber => 3;

    private string Part1()
    {
        string pattern = @"[m][u][l][(]\d+[,]\d+[)]";
        RegexOptions options = RegexOptions.Singleline;
        string pattern2 = @"\d+";

        int total = 0;
        foreach (var line in Input)
        {
            var matches = Regex.Matches(line, pattern, options);
            foreach (var match in matches)
            {
                var numbers = Regex.Matches(match.ToString(), pattern2, options);
                if (numbers.Count != 2)
                {
                    Log.Error("Found bad data!" + match.ToString());
                    continue;
                }

                var v1 = int.Parse(numbers[0].ToString());
                var v2 = int.Parse(numbers[1].ToString());

                total += v1 * v2;
            }
        }

        return total.ToString();
    }

    private string Part2()
    {
        string pattern = @"[m][u][l][(]\d+[,]\d+[)]";
        RegexOptions options = RegexOptions.Singleline;
        string pattern2 = @"\d+";

        int total = 0;
        bool multiply = true;

        foreach (var line in Input)
        {
            var doNotSplits = line.Split("don't()");
            int startIndex = 0;
            if (multiply && !line.StartsWith("don't()"))
            {
                var multiplied = GetMatches(doNotSplits[0], pattern, pattern2, options);
                total += multiplied;
                startIndex = 1;
            }

            for (int j = startIndex; j < doNotSplits.Length; j++)
            {
                var doNotSplit = doNotSplits[j];
                var doSplits = doNotSplit.Split("do()");
                for (int i = 1; i < doSplits.Length; i++)
                {
                    var multiplied = GetMatches(doSplits[i], pattern, pattern2, options);
                    total += multiplied;
                }
            }

            if (doNotSplits.Last().Contains("do()"))
                multiply = true;
            else multiply = false;
        }

        return total.ToString();
    }

    private int GetMatches(string part, string pattern, string pattern2, RegexOptions options)
    {
        var matches = Regex.Matches(part, pattern, options).Select(m => m.ToString()).ToList();
        int total = 0;
        foreach (var match in matches)
        {
            var numbers = Regex.Matches(match.ToString(), pattern2, options);
            if (numbers.Count != 2)
            {
                Log.Error("Found bad data!" + match.ToString());
                continue;
            }

            var v1 = int.Parse(numbers[0].ToString());
            var v2 = int.Parse(numbers[1].ToString());

            total += v1 * v2;
        }

        return total;
    }

    private string FancyNotWorkingSolution()
    {
        int lineCount = 1;
        long total = 0;
        foreach (var line in Input)
        {
            if (string.IsNullOrEmpty(line))
                continue;
            Log.Write("\n############## Line: " + lineCount + "####################", Colors.CornflowerBlue);
            int index = 0;
            while (true)
            {
                var mult = GetNextMult(ref index, line);
                if (index == -1)
                    break;
                if (!string.IsNullOrEmpty(mult))
                {
                    var values = mult.Split(',');
                    if (values[0].Length > 3 || values[1].Length > 3)
                    {
                        Log.Error(mult);
                        continue;
                    }

                    if (!int.TryParse(values[0], out var v1) || !int.TryParse(values[1], out var v2))
                    {
                        Log.Error("Failed to parse: " + mult);
                        continue;
                    }

                    total += v1 * v2;
                    Log.Log(mult);
                }
            }

            lineCount++;
        }

        return total.ToString();
    }

    private string GetNextMult(ref int index, string line)
    {
        int startIndex = GoToNextStart(index, line);
        if (startIndex == -1)
        {
            index = -1;
            return string.Empty;
        }

        index = startIndex + 4;
        int numberEnd = GoToLastNumber(index, line);
        if (numberEnd == -1)
            return string.Empty;
        index = numberEnd + 1;

        if (line[index] != ',')
            return string.Empty;

        index++;
        int numberEnd2 = GoToLastNumber(index, line);
        if (numberEnd2 == -1)
            return string.Empty;

        index = numberEnd2 + 1;
        if (line[index] != ')')
            return string.Empty;

        Log.Log(line.Substring(startIndex, index + 1 - startIndex));
        return line.Substring(startIndex + 4, index - (startIndex + 4));
    }

    private int GoToNextStart(int index, string line)
    {
        string target = "mul(";
        int currentIndexOfTarget = 0;
        for (int i = index; i < line.Length; i++)
        {
            if (line[i] == target[currentIndexOfTarget])
            {
                currentIndexOfTarget++;
                if (currentIndexOfTarget == target.Length)
                    return i - (currentIndexOfTarget - 1);
            }
        }

        return -1;
    }

    private int GoToLastNumber(int startIndex, string line)
    {
        int numberCount = 0;
        int index = -1;
        for (int i = startIndex; i < line.Length; i++)
        {
            if (char.IsNumber(line[i]))
            {
                numberCount++;
                index = i;
            }
            else break;
        }

        if (numberCount != 0)
            return index;
        return -1;
    }
}