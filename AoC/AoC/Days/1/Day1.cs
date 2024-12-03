using AoC.WebConnection;

namespace AoC.Days._1;

public class Day1 : Day
{
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
        Console.WriteLine(total);
    }

}