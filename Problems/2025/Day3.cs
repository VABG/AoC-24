using AdventOfCodeCore.Models.Days;

namespace Problems._2025;

public class Day3 : Day
{
    public override int Year => 2025;

    public override int DayNumber => 3;

    protected override string Part1()
    {
        long total = 0;
        foreach (var row in Input)
        {
            var numbers = row.Select(c => int.Parse(c.ToString())).ToArray();

            int highestIndex = 0;
            int highestValue = -1;
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                if (numbers[i] > highestValue)
                {
                    highestValue = numbers[i];
                    highestIndex = i;
                }
            }
            
            int secondHighestValue = -1;
            for (int i = highestIndex + 1; i < numbers.Length ; i++)
                if (numbers[i] > secondHighestValue)
                    secondHighestValue = numbers[i];
            
            var res = highestValue.ToString() + secondHighestValue.ToString();
            
            total += long.Parse(res);
        }
        
        return total.ToString();
    }

    protected override string Part2()
    {
        const int digits = 11;
        // Find the highest numbers while enough numbers remain
        long total = 0;
        foreach (var row in Input)
        {
            
            var numbers = row.Select(c => int.Parse(c.ToString())).ToArray();
            
            List<int> resultIndices = [];
            while (true)
            {
                var startIndex = resultIndices.Count != 0 ? resultIndices.Last() : -1;
                var endIndex = numbers.Length - (digits - resultIndices.Count);
                var highestIndex = GetHighestIndex(
                    numbers, startIndex, endIndex);
                
                if (highestIndex == -1)
                {
                    break;
                }
                
                resultIndices.Add(highestIndex);
            }

            
            var resultString = resultIndices.Select(i => numbers[i].ToString()).Aggregate((a, b) => a + b);
            total += long.Parse(resultString);
            
            Log.Log(resultString);
        }
        
        return total.ToString();
    }

    private int GetHighestIndex(int[] values, int startIndex, int maxIndex)
    {
        int highestIndex = -1;
        int highestValue = -1;
        if (maxIndex > values.Length)
            return -1;
        for (int i = startIndex + 1; i < maxIndex; i++)
        {
            if (values[i] > highestValue)
            {
                highestIndex = i;
                highestValue = values[i];
            }
        }

        return highestIndex;
    }
}