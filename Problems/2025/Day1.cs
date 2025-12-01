using AdventOfCodeCore.Models.Days;

namespace Problems._2025;

public class Day1 : Day
{
    public override int Year => 2025;

    public override int DayNumber => 1;

    protected override string Part1()
    {
        var rows = Input;
        int position = 50;
        int result = 0;
        foreach (var row in rows)
        {
            char dir = row[0];
            var nr = int.Parse(row.Substring(1));
            
            if (dir == 'R')
                position += nr;
            if (dir == 'L')
                position -= nr;

            position %= 100;

            if (position < 0)
                position = 100 + position;
            
            if (position == 0)
                result++;
        }
        
        return result.ToString();
    }

    protected override string Part2()
    {
        var rows = Input;
        int position = 50;
        int result = 0;
        foreach (var row in rows)
        {
            char c = row[0];
            var nr = int.Parse(row.Substring(1));

            bool dir = c != 'L';

            for (int i = 0; i < nr; i++)
            {
                position += dir ?  1 : -1;

                if (position < 0)
                    position = 99;
                if (position > 99)
                    position = 0;
                
                if (position == 0)
                    result++;
            }
        }
        return result.ToString();
    }
    
}