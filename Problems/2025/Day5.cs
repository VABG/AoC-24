using AdventOfCodeCore.Models.Days;

namespace Problems._2025;

class Range(long start, long end)
{
    public long Start = start;
    public long End = end;

    public bool InRange(long value)
    {
        return Start <= value && value <= End;
    }

    public bool Contains(Range range)
    {
        return (Start <= range.Start && End >= range.End);
    }
}

class Ranges()
{
    private List<Range> _ranges = [];

    public void AddRange(Range newRange)
    {
        if (_ranges.Count == 0)
            _ranges.Add(newRange);
        else
            CheckForMatchAndAdd(newRange);
    }

    private void CheckForMatchAndAdd(Range newRange)
    {
        Range? startRange = null;
        Range? endRange = null;
        foreach (var range in _ranges)
        {
            if (range.InRange(newRange.Start))
                startRange = range;

            if (range.InRange(newRange.End))
                endRange = range;

            if (endRange != null && startRange != null)
                break;
        }

        if (startRange == null && endRange == null)
        {
            VerifyRanges(newRange);
            _ranges.Add(newRange);
            return;
        }

        if (startRange == endRange)
            return;

        if (startRange != null && endRange != null)
        {
            startRange.End = endRange.End;
            _ranges.Remove(endRange);
            VerifyRanges(startRange);
            return;
        }

        if (startRange != null)
        {
            startRange.End = newRange.End;
            VerifyRanges(startRange);
        }

        if (endRange != null)
        {
            endRange.Start = newRange.Start;
            VerifyRanges(endRange);
        }
    }

    private void VerifyRanges(Range changedRange)
    {
        List<Range> toRemove = [];
        foreach (var range in _ranges)
        {
            if (changedRange == range)
                continue;
            
            if (changedRange.Contains(range))
                toRemove.Add(range);
        }

        foreach (var range in toRemove)
            _ranges.Remove(range);
    }

    public long GetTotalRangeLength()
    {
        long total = 0;
        foreach (var range in _ranges)
        {
            if (range.End == range.Start)
                total += 1;

            else total += (range.End + 1) - range.Start;
        }

        return total;
    }
}

public class Day5 : Day
{
    public override int Year => 2025;
    public override int DayNumber => 5;

    protected override string Part1()
    {
        List<Range> ranges = [];

        int index = 0;
        while (!string.IsNullOrWhiteSpace(Input[index]))
        {
            var range = Input[index].Split('-');
            var low = long.Parse(range[0]);
            var high = long.Parse(range[1]);
            ranges.Add(new Range(low, high));
            index++;
        }

        index++;

        int result = 0;

        while (index < Input.Length)
        {
            var value = long.Parse(Input[index]);

            bool success = false;
            foreach (var range in ranges)
                if (range.InRange(value))
                    success = true;

            if (success)
                result++;

            index++;
        }

        return result.ToString();
    }

    protected override string Part2()
    {
        Ranges ranges = new Ranges();

        int index = 0;
        while (!string.IsNullOrWhiteSpace(Input[index]))
        {
            var range = Input[index].Split('-');
            var newRange = new Range(long.Parse(range[0]), long.Parse(range[1]));

            ranges.AddRange(newRange);
            index++;
        }

        return ranges.GetTotalRangeLength().ToString();
    }
}