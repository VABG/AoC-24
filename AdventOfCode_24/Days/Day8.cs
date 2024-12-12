using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.Visualization;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace AdventOfCode_24.Days
{
    internal class Day8 : Day
    {
        public override int Year => 2024;

        public override int DayNumber => 8;

        private string Part1()
        {
            var lvl = new Level(Input, this);
            CreateRenderer(lvl.Width, lvl.Height);
            lvl.SetRenderer(Renderer);
            lvl.Draw();
            var total = lvl.CalculateResonance();
            return total.ToString();
        }

        private class Point
        {
            public char C;
            public bool hasResonance = false;
            public bool IsAntenna = true;
         
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
            Color background = new Color(25, 50, 0, 0);

            Point[,] _data;

            private byte[] randomBytesR;
            private byte[] randomBytesG;
            private byte[] randomBytesB;

            private PixelRenderer? _renderer;
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

            public void SetRenderer(PixelRenderer renderer)
            {
                _renderer = renderer;
            }

            private void GenerateRandomBytes()
            {
                Random rnd = new Random(0);
                randomBytesR = new byte[256];
                randomBytesG = new byte[256];
                randomBytesB = new byte[256];

                rnd.NextBytes(randomBytesR);
                rnd.NextBytes(randomBytesG);
                rnd.NextBytes(randomBytesB);
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
                        else pixels.Add(new Pixel(x, y, background));
                    }

                _renderer?.DrawPixels(pixels.ToArray());
            }

            private struct P2(int x, int y)
            {
                public int x = x;
                public int y = y;

                public P2 Opposite(P2 other)
                {
                    var offset = new P2(other.x - x, other.y - y);
                    return new P2(other.x + offset.x, other.y + offset.y);
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
                            var o1 = points[i].Opposite(points[j]);
                            var o2 = points[j].Opposite(points[i]);

                            UpdatePoint(o1);
                            UpdatePoint(o2);
                        }
                }

                for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                        if (_data[x, y].hasResonance)
                            total++;

                return total;
            }

            private void UpdatePoint(P2 p)
            {
                if (!InLevel(p))
                    return;
                
                _data[p.x, p.y].hasResonance = true;
                _renderer?.DrawPixel(new Pixel(p.x, p.y, Colors.Red));
                _d.Wait(0.01);
                _d.Render();
            }

            private bool InLevel(P2 p)
            {
                return InLevel(p.x, p.y);
            }

            private bool InLevel(int x, int y)
            {
                return !(x < 0 || y < 0 || x >= Width || y >= Height);
            }

            private Color CharacterToColor(char c)
            {
                return new Color(byte.MaxValue, randomBytesR[c], randomBytesG[c], randomBytesB[c]);
            }
        }
    }
}
