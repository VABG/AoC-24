using AdventOfCodeCore.Models.Days;

namespace Problems._2025;

public class Day6 : Day
{
    public override int Year => 2025;
    public override int DayNumber => 6;

    protected override string Part1()
    {
        var ops = Input[^1].Split(' ').Where(c => !string.IsNullOrWhiteSpace(c)).ToArray();
        var currentValues = Input[0].Split(' ').Where(c => !string.IsNullOrWhiteSpace(c)).Select(long.Parse).ToArray();
        
        for (int i = 1; i < Input.Length - 1; i++)
        {
            var newValues = Input[i].Split(' ').Where(c => !string.IsNullOrWhiteSpace(c)).Select(long.Parse).ToArray();

            for (int j = 0; j < currentValues.Length; j++)
            {
                if (ops[j] == "*")
                    currentValues[j] *= newValues[j];
                if(ops[j] == "+")
                    currentValues[j] +=  newValues[j];
            }
        }


        return currentValues.Sum().ToString();
    }

    protected override string Part2()
    {
        int width = Input[0].Length;
        int height = Input.Length;

        long total = 0;
        
        var currentNumbers = new List<long>();        
        for (int x = width-1; x >= 0 ; x--)
        {
            string nr = "";

            for (int y = 0; y < Input.Length -1; y++)
            {
                if (Input[y][x] == ' ')
                    continue;
                nr += Input[y][x];
            }
            
            if(!string.IsNullOrWhiteSpace(nr))
                currentNumbers.Add(long.Parse(nr));

            if (Input[height - 1][x] != ' ')
            {
                var op = Input[height - 1][x];
                if (op == '+')
                {
                    var sum =currentNumbers.Sum();
                    Log.Log("+ : " + sum);
                    total += sum;
                }
                if (op == '*')
                {
                    long sum = 0;
                    foreach (var n in currentNumbers)
                    {
                        if (sum == 0)
                            sum = n;
                        else
                            sum *= n;
                    }
                    Log.Log("* : " + sum);
                    total += sum;
                }
                currentNumbers.Clear();
            }
        }
        
        
        return total.ToString();
    }
}