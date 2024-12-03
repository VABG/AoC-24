using AdventOfCode_24.Model.Days;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_24.Days;

public class Day1 : Day
{
    public override int DayNumber => 1;
    public override int Year => 2024;

    public Day1()
    {
        Parts.Add(1, new Func<string>(Part1));
    }

    private string Part1()
    {
        List<int> left = [];
        List<int> right = [];

        foreach (var line in Input)
        {
            if (string.IsNullOrEmpty(line))
                break;

            var values = line.Split(' ');

            if (int.TryParse(values.First(), out var leftVal))
                left.Add(leftVal);
            if (int.TryParse(values.Last(), out var rightVal))
                right.Add(rightVal);
        }

        left.Sort();
        right.Sort();

        var total = left.Select((t, i) => Math.Abs(t - right[i])).Sum();
        return total.ToString();
    }
}