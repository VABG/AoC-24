using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.WebConnection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_24.Days;

public class Day1 : Day
{
    public override int DayNumber => 1;
    
    public Day1()
    { 
        
    }

    public override async void Run()
    {
        var input = await InputReader.Read(1);

        List<int> left = [];
        List<int> right = [];

        foreach (var line in input)
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
        Log(total.ToString());
    }
}