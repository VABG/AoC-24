using AdventOfCodeCore.Models.Days;

namespace Problems._2024;

public class Day5 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 5;

    protected override string Part1()
    {
        ReadInput(out var rules, out var lines);

        int total = 0;
        foreach (var line in lines)
        {
            var correct = IsCorrect(line, rules);
            if (!correct)
            {
                Log.Error(string.Join(",", line));
                continue;
            }

            Log.Success(string.Join(",", line));
            total += line[line.Length / 2];
        }

        return total.ToString();
    }

    protected override string Part2()
    {
        ReadInput(out var rules, out var lines);

        var total = 0;
        foreach (var line in lines)
        {
            var correct = IsCorrect(line, rules);
            if (correct)
                continue;
            Log.Error(string.Join(',', line));
            total += GetSortedCenter(line, rules);
        }

        return total.ToString();
    }

    private int GetSortedCenter(int[] line, Dictionary<int, List<int>> rules)
    {
        var sortable = line.ToList();
        var comparer = new CompareInts(rules);
        sortable.Sort(comparer);

        Log.Success(string.Join(',', sortable));
        return sortable[sortable.Count / 2];
    }

    class CompareInts(Dictionary<int, List<int>> rules) : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (rules.TryGetValue(x, out var value) && value.Contains(y))
                return -1;
            if (rules.TryGetValue(y, out var value2) && value2.Contains(x))
                return 1;
            return 0;
        }
    }

    private void ReadInput(out Dictionary<int, List<int>> rules, out List<int[]> lines)
    {
        rules = [];
        lines = [];
        foreach (var line in Input)
        {
            if (line.Contains('|'))
            {
                var values = line.Split('|');
                var val1 = int.Parse(values[0]);
                var val2 = int.Parse(values[1]);
                if (!rules.ContainsKey(val1))
                    rules[val1] = [];
                rules[val1].Add(val2);
            }
            else if (!string.IsNullOrEmpty(line))
            {
                var values = line.Split(',').Select(int.Parse).ToArray();
                lines.Add(values);
            }
        }
    }

    private bool IsCorrect(int[] line, Dictionary<int, List<int>> rules)
    {
        var correct = true;

        for (var i = 0; i < line.Length; i++)
        {
            var page = line[i];
            if (!rules.ContainsKey(page))
                continue;
            foreach (var after in rules[page])
            {
                var index = Array.IndexOf(line, after);

                if (index == -1 || index >= i)
                    continue;

                correct = false;
                break;
            }

            if (!correct)
                break;
        }

        return correct;
    }
}