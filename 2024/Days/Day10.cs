using AdventOfCodeCore.Models.Days;
using AdventOfCodeCore.Models.Visualization;

namespace _2024.Days;

internal class Day10 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 10;

    private int _width;
    private int _height;
    private readonly Color _red = new Color(128, 0,0,255);

    protected override string Part1()
    {
        _width = Input[0].Length;
        _height = Input.Length;
        List<Pixel> pixels = [];
        Level[,] levels = new Level[_width, _height];
        for (int y = 0; y < _height; y++)
        for (int x = 0; x < _width; x++)
        {
            levels[x, y] = new Level(int.Parse(Input[y][x].ToString()));
            float height = levels[x, y].Height;
            height *= .1f;
            height *= height;

            pixels.Add(new Pixel(x, y, new Color(255, 255, 255, (byte)(height * 200))));
        }

        CreatePixelRenderer(_width, _height);
        PixelRenderer?.DrawPixels(pixels.ToArray());
        Render();
        int total = 0;
        for (int y = 0; y < _height; y++)
        for (int x = 0; x < _width; x++)
        {
            if (levels[x, y].Height != 0)
                continue;
            var score = MoveAndGetScore(levels, x, y);
            if (IsTest)
                Log.Log(score.ToString());
            total += score;
            PixelRenderer?.DrawPixels(pixels.ToArray());
            Render();
            Reset(ref levels);
        }

        return total.ToString();
    }
    
    private void Reset(ref Level[,] level)
    {
        for (int y = 0; y < _height; y++)
        for (int x = 0; x < _width; x++)
            level[x, y].Reset();
    }


    private class Level(int height)
    {
        public bool Visited;
        public int Height { get; } = height;
        public void Reset()
        {
            Visited = false;
        }
    }

    private int MoveAndGetScore(Level[,] levels, int x, int y)
    {
        Queue<P2> toVisit = new Queue<P2>();
        toVisit.Enqueue(new P2(x, y));
        int totalNines = 0;
        while (toVisit.TryDequeue(out var current))
        {
            GetNext(levels, current, ref toVisit, ref totalNines);
        }

        return totalNines;
    }

    private void GetNext(Level[,] levels, P2 current, ref Queue<P2> toVisit, ref int totalNines)
    {
        if (levels[current.X, current.Y].Visited)
            return;
        levels[current.X, current.Y].Visited = true;

        int currentHeight = levels[current.X, current.Y].Height;
        PixelRenderer?.DrawPixel(new Pixel(current.X, current.Y, _red));
        Wait(IsTest ? 0.05 : 0.005);
        Render();
        if (currentHeight == 9)
        {
            totalNines++;
            return;
        }

        GetNext(levels, currentHeight, current, ref toVisit, 0, 1);
        GetNext(levels, currentHeight, current, ref toVisit, 0, -1);
        GetNext(levels, currentHeight, current, ref toVisit, 1, 0);
        GetNext(levels, currentHeight, current, ref toVisit, -1, 0);
    }

    private void GetNext(Level[,] levels, int currentLevel, P2 current, ref Queue<P2> toVisit, int x, int y)
    {
        int xMove = current.X + x;
        int yMove = current.Y + y;
        if (!Inbounds(xMove, yMove) || levels[xMove, yMove].Visited) 
            return;

        if (levels[xMove, yMove].Height == currentLevel + 1)
            toVisit.Enqueue(new P2(xMove, yMove));
    }

    public bool Inbounds(int x, int y) => !(x < 0 || y < 0 || x >= _width || y >= _height);

    struct P2(int x, int y)
    {
        public readonly int X = x;
        public readonly int Y = y;
    }
}