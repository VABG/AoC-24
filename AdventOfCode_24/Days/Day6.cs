using System;
using System.Threading;
using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.Visualization;
using Avalonia.Media;

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
        const int skipTime = 5;
        int skipCounter = skipTime;
        while (lvl.GuardInBounds())
        {
            lvl.MoveGuard();
            if (skipCounter == skipTime)
            {
                DrawLevel(lvl);
                skipCounter = 0;
            }

            skipCounter++;
        }
        DrawLevel(lvl);
        
        return lvl.GetVisitedNr().ToString();
    }

    private void DrawLevel(Level level)
    {
        Renderer.DrawPixels(level.Data);
        Render();
    }

    class Level
    {
        public Color VisitedColor { get; } = Colors.DarkGreen;
        public Color BoxColor{ get; }  = Colors.SaddleBrown;
        public Color Background { get; } = new Color(10, 255,0,0);
        public int Width { get; }
        public int Height { get; }

        public Pixel[,] Data { get; private set; }
        public Guard Guard { get; private set; }

    
        public Level(string[] input)
        {
            Width = input[0].Length;
            Height = input.Length;

            Data = new Pixel[Height, Width];
            for (int y= 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var c = input[y][x];
                    var pixel = ColorAndPositionToPixel(c, x,y);
                    if (pixel.Color == VisitedColor)
                        Guard = new Guard(x, y, CharToDir(c));
                    Data[y, x] = pixel;
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
            Guard.GetOneForward(out var x, out var y);
            
            if (InBounds(x,y) && Data[y,x].Color == BoxColor)
                Guard.RotateRight();
            else
            {
                Guard.Move();

                if (InBounds(x,y))
                    Data[y, x] = new Pixel(x, y, VisitedColor);
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
                    if (Data[y, x].Color == VisitedColor)
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