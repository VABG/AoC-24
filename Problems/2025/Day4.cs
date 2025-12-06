using AdventOfCodeCore.Models.Days;
using AdventOfCodeCore.Models.Visualization;

namespace Problems._2025;

public class Day4 : Day
{
    public override int Year => 2025;

    public override int DayNumber => 4;

    protected override string Part1()
    {
        int height = Input.Length;
        int width = Input[0].Length;

        CreatePixelRenderer(width, height);
        var input = Input.Select(s => s.ToCharArray()).ToArray();
        Render();
        UpdatePixels(input);

        int marked = 0;
        
        for (int y = 0; y <  height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (input[y][x] != '@')
                    continue;
                
                int neighbors = CoundNeighbors(x, y, input, width, height);
                if (neighbors < 4)
                {
                    marked++;
                    if(IsTest)
                        Log.Success("x:" + x + " y:" +y + " "+ neighbors);
                    PixelRenderer?.DrawPixel(new Pixel(x,y, Colors.IndianRed));
                    Render();
                    if(IsTest)
                        Thread.Sleep(100);
                }
            }
            Thread.Sleep(1);
        }
        
        return marked.ToString();
    }

    private void UpdatePixels(char[][] input)
    {
        PixelRenderer!.Clear(Colors.Transparent);
        if (PixelRenderer == null)
            return;
        int height = input.Length;
        int width = input[0].Length;
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (input[y][x] == '@')
                    PixelRenderer.DrawPixel(new Pixel(x,y, new Color(150, 0, 0, 200)));
                if(input[y][x] == 'x')
                    PixelRenderer.DrawPixel(new Pixel(x,y, Colors.Red));
            }
        }
        //Render();
        //Thread.Sleep(100);
    }

    protected override string Part2()
    {
        int height = Input.Length;
        int width = Input[0].Length;
        
        CreatePixelRenderer(width, height);
        var input = Input.Select(s => s.ToCharArray()).ToArray();
        UpdatePixels(input);
        Render();
        int total = 0;
        while (true)
        {
            var removed = RemoveRolls(input, height, width);
            if (removed == 0)
                break;
            total += removed;
            UpdatePixels(input);
        }
        
        return total.ToString();
    }


    private int RemoveRolls(char[][] input, int height, int width)
    {
        int marked = 0;
        for (int y = 0; y <  height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (input[y][x] != '@')
                    continue;
                
                int neighbors = CoundNeighbors(x, y, input, width, height);
                if (neighbors >= 4) 
                    continue;
                input[y][x] = 'x';
                marked++;
                PixelRenderer?.DrawPixel(new Pixel(x,y, Colors.Orange));

                if (!IsTest) 
                    continue;
                
                Render();
                Thread.Sleep(50);
            }
        }
        Render();
        ClearRemoved(input);
        Thread.Sleep(50);
        Log.Log(marked.ToString());
        return marked;
    }

    private int ClearRemoved(char[][] input)
    {
        int removed = 0;
        int width = input[0].Length;
        int height = input.Length;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (input[y][x] != 'x')
                    continue;
                input[y][x] = '.';
                removed++;
            }
        }

        return removed;
    }

    private int CoundNeighbors(int x, int y, char[][] input, int width, int height)
    {
        int total = -1;
        for (int x2 = x-1; x2 <= x+1; x2++)
        {
            for (int y2 = y - 1; y2 <= y + 1; y2++)
            {
                if (!InBorders(x2, y2, width, height))
                    continue;
                
                char c = input[y2][x2];
                if (c is '@' or 'x')
                    total++;
            }
        }
        return total;
    }

    private bool InBorders(int x, int y, int width, int height)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
            return false;
        return true;
    }
}