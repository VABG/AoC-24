using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.Visualization;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode_24.Days
{
    internal class Day10 : Day
    {
        public override int Year => 2024;

        public override int DayNumber => 10;

        int width;
        int height;

        private string Part1()
        {
            width = Input[0].Length;
            height = Input.Length;
            List<Pixel> pixels = [];
            Level[,] levels = new Level[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    levels[x, y] = new Level(int.Parse(Input[y][x].ToString()));
                    pixels.Add(new Pixel(x, y, new Avalonia.Media.Color((byte)(levels[x, y].Height * 25), 255, 255, 255)));
                }

            CreateRenderer(width, height);
            Renderer.DrawPixels(pixels.ToArray());
            Render();
            int total = 0;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (levels[x, y].Height != 0)
                        continue;
                    var score = MoveAndGetScore(levels, x, y);
                    if (IsTest)
                        Log.Log(score.ToString());
                    total += score;
                    Renderer.DrawPixels(pixels.ToArray());
                    Render();
                    Reset(ref levels);
                }

            return total.ToString();
        }

        private void Reset(ref Level[,] level)
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    level[x, y].Reset();
        }


        private class Level(int height)
        {
            public bool Visited = false;
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
            if (levels[current.x, current.y].Visited)
                return;
            levels[current.x, current.y].Visited = true;

            int currentHeight = levels[current.x, current.y].Height;
            Renderer?.DrawPixel(new Pixel(current.x, current.y, Colors.Red));
            Wait(IsTest ? 0.05 : 0.005);
            Render();
            if (currentHeight == 9)
            {
                totalNines++;
                return;
            }

            GetNext(ref totalNines, levels, currentHeight, current, ref toVisit, 0, 1);
            GetNext(ref totalNines, levels, currentHeight, current, ref toVisit, 0, -1);
            GetNext(ref totalNines, levels, currentHeight, current, ref toVisit, 1, 0);
            GetNext(ref totalNines, levels, currentHeight, current, ref toVisit, -1, 0);
        }

        private void GetNext(ref int totalNines, Level[,] levels, int currentLevel, P2 current, ref Queue<P2> toVisit, int x, int y)
        {
            int xMove = current.x + x;
            int yMove = current.y + y;
            if (!Inbounds(xMove, yMove) || levels[xMove, yMove].Visited) 
                return;

            if (levels[xMove, yMove].Height == currentLevel + 1)
                toVisit.Enqueue(new P2(xMove, yMove));
        }

        public bool Inbounds(int x, int y) => !(x < 0 || y < 0 || x >= width || y >= height);

        struct P2(int x, int y)
        {
            public int x = x;
            public int y = y;
        }
    }
}
