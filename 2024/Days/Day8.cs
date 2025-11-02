using AdventOfCodeCore.Interfaces;
using AdventOfCodeCore.Models.Days;
using AdventOfCodeCore.Models.Visualization;

namespace _2024.Days;

internal class Day8 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 8;

    protected override string Part1()
    {
        var lvl = new Level(Input, this);
        CreatePixelRenderer(lvl.Width, lvl.Height);
        lvl.SetRenderer(PixelRenderer);
        lvl.Draw();
        var total = lvl.CalculateResonance();
        return total.ToString();
    }


    protected override string Part2()
    {
        var lvl = new Level(Input, this);
        CreatePixelRenderer(lvl.Width, lvl.Height);
        lvl.SetRenderer(PixelRenderer);
        lvl.Draw();
        lvl.Part2 = true;
        var total = lvl.CalculateResonance();
        return total.ToString();
    }

    private class Point
    {
        public readonly char C;
        public bool HasResonance;
        public readonly bool IsAntenna = true;
         
        public Point(char c)
        {
            C = c;
            if (c == '.')
                IsAntenna = false;
        }
    }

    private class Level
    {
        public int Width { get; }
        public int Height { get; }
        private readonly Color _background = new Color(25, 50, 0, 0);

        private readonly Point[,] _data;

        private byte[] _randomBytesR;
        private byte[] _randomBytesG;
        private byte[] _randomBytesB;
        public bool Part2;

        private IPixelRenderer? _renderer;
        private Day _d;
        public Level(string[] input, Day d)
        {
            _d = d;
            Height = input.Length;
            Width = input[0].Length;
            _data = new Point[Width, Height];

            for (int y = 0; y < input.Length; y++)
            for (int x = 0; x < input.Length; x++) 
                _data[x, y] = new Point(input[y][x]);

            GenerateRandomBytes();
        }

        public void SetRenderer(IPixelRenderer renderer)
        {
            _renderer = renderer;
        }

        private void GenerateRandomBytes()
        {
            Random rnd = new Random(0);
            _randomBytesR = new byte[256];
            _randomBytesG = new byte[256];
            _randomBytesB = new byte[256];

            rnd.NextBytes(_randomBytesR);
            rnd.NextBytes(_randomBytesG);
            rnd.NextBytes(_randomBytesB);
        }

        public void Draw()
        {
            if (_renderer == null) 
                return;
            List<Pixel> pixels = [];

            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                if (_data[x, y].C != '.')
                    pixels.Add(new Pixel(x, y, CharacterToColor(_data[x, y].C)));
                else pixels.Add(new Pixel(x, y, _background));
            }

            _renderer?.DrawPixels(pixels.ToArray());
        }

        private struct P2(int x, int y)
        {
            public readonly int X = x;
            public readonly int Y = y;

            public P2 Opposite(P2 other)
            {
                var offset = new P2(other.X - X, other.Y - Y);
                return new P2(other.X + offset.X, other.Y + offset.Y);
            }
        }

        public int CalculateResonance()
        {
            Dictionary<char, List<P2>> antennaGroups = new Dictionary<char, List<P2>>();

            int total = 0;
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                if (!_data[x, y].IsAntenna)
                    continue;

                if (!antennaGroups.ContainsKey(_data[x,y].C))
                    antennaGroups[_data[x,y].C] = [];

                antennaGroups[_data[x,y].C].Add(new P2(x,y));
            }

            foreach(var points in antennaGroups.Values)
            {
                for (int i = 0; i < points.Count; i++)
                for (int j = i+1; j < points.Count; j++)
                {
                    if (!Part2)
                    {
                        var o1 = points[i].Opposite(points[j]);
                        var o2 = points[j].Opposite(points[i]);

                        UpdatePoint(o1);
                        UpdatePoint(o2);
                    }
                    else 
                        DrawLines(points[i], points[j]);
                }
            }

            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                if (_data[x, y].HasResonance)
                    total++;

            return total;
        }

        private void UpdatePoint(P2 p)
        {
            if (!InLevel(p))
                return;
                
            _data[p.X, p.Y].HasResonance = true;
            _renderer?.DrawPixel(new Pixel(p.X, p.Y, Colors.Red));
            _d.Wait(0.003);
            _d.Render();
        }

        private bool InLevel(P2 p)
        {
            return InLevel(p.X, p.Y);
        }

        private bool InLevel(int x, int y)
        {
            return !(x < 0 || y < 0 || x >= Width || y >= Height);
        }

        private Color CharacterToColor(char c)
        {
            return new Color(byte.MaxValue, _randomBytesR[c], _randomBytesG[c], _randomBytesB[c]);
        }

        private void DrawLines(P2 p1, P2 p2)
        {
            var offset = new P2(p2.X - p1.X, p2.Y - p1.Y);
            var gcd = Gcd(Math.Abs(offset.X), Math.Abs(offset.Y));
            if (gcd > 1)
                offset = new P2(offset.X / gcd, offset.Y / gcd);


            P2 currentPoint = p1;
            // Do steps in both directions (inverse offset)
            while(InLevel(currentPoint))
            {
                UpdatePoint(currentPoint);
                currentPoint = new P2(currentPoint.X + offset.X, currentPoint.Y + offset.Y);
            }

            offset = new P2(-offset.X, -offset.Y);
            currentPoint = p1;
            while (InLevel(currentPoint))
            {
                UpdatePoint(currentPoint);
                currentPoint = new P2(currentPoint.X + offset.X, currentPoint.Y + offset.Y);
            }

        }

        private static int Gcd(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }
    }

}