using AdventOfCodeCore.Models.Days;

namespace Problems._2025;

public class Day2 : Day
{
    public override int Year => 2025;

    public override int DayNumber => 2;

    protected override string Part1()
    {
        long total = 0;

        var ranges = Input[0].Split(",");
        foreach (var range in ranges)
        {
            if (string.IsNullOrEmpty(range))
                continue;
            
            var fromTo = range.Split('-');
            var from =  long.Parse(fromTo[0].Trim());
            var to = long.Parse(fromTo[1].Trim());
            for (long i = from; i <= to; i++)
            {
                var str = i.ToString();
                if (str.Length % 2 != 0)
                    continue;
                var l = str.Length / 2;
                var pt1 = str.Substring(0, l);
                var pt2 = str.Substring(l, l);
                if (pt1 == pt2)
                {
                    total += i;
                    Log.Log(i.ToString());
                }
            }
        }

        return total.ToString();
    }

    protected override string Part2()
    {
        long total = 0;

        var ranges = Input[0].Split(",");
        Log.Log("Ranges loaded");
        foreach (var range in ranges)
        {
            //Log.Log("Range: " + range);
            if (string.IsNullOrEmpty(range))
                continue;
            
            var fromTo = range.Split('-');
            var from =  long.Parse(fromTo[0]);
            var to = long.Parse(fromTo[1]);
            for (long i = from; i <= to; i++)
            {
                var str = i.ToString();

                if (FindPattern(str))
                {
                    total += i;
                    Log.Success(i.ToString());
                }
            }
        }

        return total.ToString();
    }

    private bool FindPattern(string number)
    {
        for (int i = 1; i < (number.Length / 2) + 1; i++)
        {
            if (number.Length % i != 0) // can only repeat if total length is divisible by pattern
                continue;

            string activePattern = number.Substring(0, i);
            bool success = true;
            
            for(int j = i; j < number.Length; j+= i)
            {
                if (number.Substring(j, i) == activePattern) 
                    continue;
                
                success = false;
                break;
            }

            if (success)
            {
                Thread.Sleep(5);
                Log.Success("Pattern: " + activePattern);
                return true;
            }
        }

        return false;
    }

}