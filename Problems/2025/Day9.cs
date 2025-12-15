using AdventOfCodeCore.Models.Days;

namespace Problems._2025;

readonly struct Point2d(long x, long y)
{
    public long X => x;
    public long Y => y;
}

public class Day9 : Day
{
    public override int Year => 2025;
    public override int DayNumber => 9;

    
    
    protected override string Part1()
    {
        var points = new List<Point2d>();
        foreach (var row in Input)
        {
            var nrs = row.Split(',');
            points.Add(new Point2d(long.Parse(nrs[0]), long.Parse(nrs[1])));
        }
        
        if(IsTest)
            CreatePixelRenderer(12, 12);

        long largestArea = 0;
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i+1; j < points.Count; j++)
            {
                var area = GetArea(points[i], points[j]);
                if (area > largestArea)
                {
                    Log.Log(points[i] + " & " +  points[j]);
                    largestArea = area;
                }
            }
        }
        
        return largestArea.ToString();
    }

    private long GetArea(Point2d p1, Point2d p2)
    {
        return (Math.Abs(p1.X - p2.X) + 1) * (Math.Abs(p1.Y - p2.Y) +1);
    }
}