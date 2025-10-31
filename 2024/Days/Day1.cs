using AdventOfCodeCore.Models.Days;

namespace _2024.Days;

public class Day1 : Day
{
    public override int Year => 2024;
    public override int DayNumber => 1;

    protected override string Part1()
    {
        List<int> left = [];
        List<int> right = [];

        GetLists(ref left, ref right);

        left.Sort();
        right.Sort();
        
        var total = left.Select((t, i) => Math.Abs(t - right[i])).Sum();
        return total.ToString();
    }

    protected override string Part2()
    {
        List<int> left = [];
        List<int> right = [];

        GetLists(ref left, ref right);

        Dictionary<int, int> rightNumbers = [];
        foreach (var nr in right)
        {
            if (!rightNumbers.TryAdd(nr, 1))
                rightNumbers[nr]++;
        }
        
        var total = 0;
        foreach (var nr in left)
        {
            if (!rightNumbers.ContainsKey(nr))
                continue;

            total += nr * rightNumbers[nr];
        }

        return total.ToString();
    }
    
    private void GetLists(ref List<int> left, ref List<int> right)
    {
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
    }
}