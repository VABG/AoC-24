using System.Collections;
using System.Drawing;
using AdventOfCodeCore.Models.Days;
using AdventOfCodeCore.Models.Visualization;
using Color = AdventOfCodeCore.Models.Visualization.Color;

namespace Problems._2025;

public class Day7 : Day
{
    public override int Year => 2025;
    public override int DayNumber => 7;


    protected override string Part1()
    {
        CreatePixelRenderer(Input[0].Length, Input.Length);
        PixelRenderer?.Clear(Colors.Transparent);
        var data = new Pixel[Input[0].Length, Input.Length];
        for (int y = 0; y < Input.Length; y++)
        {
            for (int x = 0; x < Input[0].Length; x++)
            {
                data[x, y] = Input[y][x] switch
                {
                    '^' => new Pixel(x, y, Colors.Gray),
                    'S' => new Pixel(x, y, Colors.Cyan),
                    _ => new Pixel(x, y, Colors.Transparent)
                };
            }
        }

        PixelRenderer?.DrawPixels(data);
        Render();
        int startX = Input[0].IndexOf('S');
        data[startX, 1] = new Pixel(startX, 1, Colors.Red);
        int splits = 0;
        for (int y = 1; y < Input.Length - 1; y++)
        {
            for (int x = 0; x < Input[0].Length; x++)
            {
                CheckStop();

                if (data[x, y].Color != Colors.Red)
                    continue;

                if (data[x, y + 1].Color == Colors.Gray)
                {
                    data[x + 1, y + 1] = new Pixel(x + 1, y + 1, Colors.Red);
                    data[x - 1, y + 1] = new Pixel(x - 1, y + 1, Colors.Red);
                    splits++;
                }
                else
                    data[x, y + 1] = new Pixel(x, y + 1, Colors.Red);
            }

            PixelRenderer?.DrawPixels(data);
            Render();
            Thread.Sleep(IsTest ? 200 : 50);
        }

        return splits.ToString();
    }

    class Point(int x, int y, long count)
    {
        public int X = x;
        public int Y = y;
        public long Count { get; private set; } = count;

        public void Add(long otherCount)
        {
            Count += otherCount;
        }
    }

    protected override string Part2()
    {
        CreatePixelRenderer(Input[0].Length, Input.Length);
        long max = 40;
        if(!IsTest)
            max = 13418215871354;
        
        PixelRenderer?.Clear(Colors.Transparent);
        Render();
        List<Point> activePaths = [];
        int startX = Input[0].IndexOf('S');
        activePaths.Add(new Point(startX, 0, 1));
        int i = 0;
        while (i < Input.Length-1)
        {
            CheckStop();
            List<Point> newPaths = [];
            foreach (var point in activePaths)
            {
                CheckStop();
                point.Y += 1;

                var color = LongToColor(point.Count, max);
                PixelRenderer?.DrawPixel(new Pixel(point.X, point.Y,  color));

                if (Input[point.Y][point.X] != '^')
                    continue;
                
                PixelRenderer?.DrawPixel(new Pixel(point.X, point.Y, color));
                var newPoint = new Point(point.X-1, point.Y, point.Count);
                newPaths.Add(newPoint);
                point.X += 1;
            }
            
            Render();
            Thread.Sleep(IsTest ? 200 : 20);
            activePaths.AddRange(newPaths);
            CombinePaths(activePaths);

            if (IsTest)
            {
                Log.Log("Paths: " + activePaths.Count);
                Log.Log("PathsTotal: " + activePaths.Sum(p => p.Count));
            }
            i++;
        }

        return activePaths.Sum(p => p.Count).ToString();
    }

    private Color LongToColor(long count, long max)
    {
        var maxLog = Math.Log(max);
        var log = Math.Log(count);
        log /= maxLog;
        log *= 2;
        byte red = (byte)Math.Clamp(255 * log, 0, 255);
        byte gb =  (byte)Math.Clamp(255 * (log - 1), 0, 255);
        return new Color(red, gb, gb, (byte)(red *.75 + 64));
    }

    private void CombinePaths(List<Point> paths)
    {
        List<Point> toRemove = [];
        for (int i = 0; i < paths.Count; i++)
        {
            var p1 = paths[i];
            for (int j = i+1; j < paths.Count; j++)
            {
                var p2 = paths[j];
                if (p1.X != p2.X || p1.Y != p2.Y)
                    continue;
                if (toRemove.Contains(p2))
                    continue;
                p1.Add(p2.Count);
                toRemove.Add(p2);
            }
        }

        foreach (var pp in toRemove)
            paths.Remove(pp);    
        
    }
}