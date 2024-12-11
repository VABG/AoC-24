using System;
using System.Collections.Generic;
using System.Threading;
using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.Visualization;
using Avalonia.Controls;
using Avalonia.Media;
using DynamicData.Kernel;

namespace AdventOfCode_24.Days;

public class Day6 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 6;

    private string Part1()
    {
        var lvl = new Level(Input);
        CreateRenderer(lvl.Width, lvl.Height);

        Log.Log("Walking...");
        Renderer.DrawPixels(lvl.GetPixels().ToArray());
        while (lvl.GuardInBounds())
        {
            lvl.MoveGuard();
            DrawLevel(lvl);
            Wait(0.001);
        }

        return lvl.GetVisitedNr().ToString();
    }

    private void DrawLevel(Level level)
    {
        Renderer.DrawPixel(level.NewPixel);
        Render();
    }

    class DataPoint
    {
        public DataPoint(Pixel pixel)
        {
            Pixel = pixel;
            if (Pixel.Color == Level.BoxColor)
                IsBox = true;
        }

        public Pixel Pixel;
        public bool Visited = false;
        public bool IsBox = false;
    }

    class Level
    {
        public static Color VisitedColor { get; } = Colors.DarkGreen;
        public static Color BoxColor{ get; }  = Colors.SaddleBrown;
        public static Color Background { get; } = new Color(10, 255,0,0);
        public int Width { get; }
        public int Height { get; }

        public DataPoint[,] Data { get; private set; }
        public Guard Guard { get; private set; }
        public Pixel NewPixel { get; private set; }

        public List<Pixel> GetPixels()
        {
            List<Pixel> pixels = [];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    pixels.Add(Data[x, y].Pixel);
                }
            }
            return pixels;
        }
    
        public Level(string[] input)
        {
            Width = input[0].Length;
            Height = input.Length;

            Data = new DataPoint[Height, Width];
            for (int y= 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var c = input[y][x];
                    var pixel = ColorAndPositionToPixel(c, x,y);
                    if (pixel.Color == VisitedColor)
                        Guard = new Guard(x, y, CharToDir(c));
                    Data[y, x] = new DataPoint(pixel);
                }
            }
        }
        public bool GuardInBounds()
        {
            return InBounds(Guard.XPos, Guard.YPos);
        }

        public bool InBounds(int x, int y)
        {
            return !(x < 0 || x >= Width || y < 0 || y >= Height);
        }

        public void MoveGuard()
        {
            Data[Guard.XPos, Guard.YPos].Visited = true;         

            Guard.GetOneForward(out var x, out var y);
            
            if (InBounds(x,y) && Data[y,x].IsBox)
            {
                Guard.RotateRight();
                if (InBounds(Guard.XPos, Guard.YPos))
                    NewPixel = new Pixel(Guard.XPos, Guard.YPos,Colors.Yellow);
            }
            else
            {
                Guard.Move();
                if (InBounds(x, y))
                {
                    NewPixel = new Pixel(x, y, Data[x, y].Pixel.Mix(VisitedColor, 0.75f));
                    Data[x, y].Pixel.Color = NewPixel.Color;
                }
            }
        }

        private Pixel ColorAndPositionToPixel(char c, int x, int y)
        {
            return c switch
            {
                '#' => new Pixel(x, y, BoxColor),
                '.' => new Pixel(x, y, Background),
                _ => new Pixel(x, y, VisitedColor)
            };
        }

        private Direction CharToDir(char c)
        {
            return c switch
            {
                '^' => Direction.Up,
                '>' => Direction.Right,
                'v' => Direction.Down,
                '<' => Direction.Left,
                _ => throw new Exception("Bad symbol: " + c)
            };
        }

        public int GetVisitedNr()
        {
            var total = 0;
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    if (Data[y, x].Visited)
                        total++;

            return total;
        }
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }

    class Guard(int xPos, int yPos, Direction facing)
    {
        public int XPos { get; private set; } = xPos;
        public int YPos { get; private set; } = yPos;
        public Direction Facing { get; private set; } = facing;
        public void Move()
        {
            GetOneForward(out var x, out var y);
            XPos = x;
            YPos = y;
        }

        public void GetOneForward(out int x, out int y)
        {
            y = YPos;
            x = XPos;
            switch (Facing)
            {
                case Direction.Up:
                    y--;
                    break;
                case Direction.Right:
                    x++;
                    break;
                case Direction.Down:
                    y++;
                    break;
                case Direction.Left:
                    x--;
                    break;
            }
        }

        public void RotateRight()
        {
            Facing++;
            if ((int)Facing > 3)
                Facing = Direction.Up;
        }

        public void RotateLeft()
        {
            Facing--;
            if ((int)Facing < 0)
                Facing = Direction.Left;
        }
    }
}