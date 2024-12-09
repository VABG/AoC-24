using System;
using AdventOfCode_24.Model.Days;

namespace AdventOfCode_24.Days;

public class Day4 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 4;

    private string Part1()
    {
        World w = new World(Input);
        int total = 0;
        for (int y = 0; y < w.Height; y++)
        {
            for (int x = 0; x < w.Width; x++)
            {
                if (w.At(x, y) != 'X')
                    continue;
                int count = FindXMasInAllDirections(x, y, w);
                total += count;
            }
        }

        return total.ToString();
    }

    private string Part2()
    {
        World w = new World(Input);
        int total = 0;
        for (int y = 0; y < w.Height; y++)
        {
            for (int x = 0; x < w.Width; x++)
            {
                if (w.At(x, y) != 'A')
                    continue;
                if (FindMasX(x, y, w))
                    total++;
            }
        }

        return total.ToString();
    }

    enum Side
    {
        First,
        Second,
        None
    }

    private bool FindMasX(int x, int y, World w)
    {
        var h1 = HasCharactersInCorners(x, y, w, true, 'M');
        var h2 = HasCharactersInCorners(x, y, w, true, 'S');
        if (h1 != h2 && h1 != Side.None && h2 != Side.None)
            return true;
        
        h1 = HasCharactersInCorners(x, y, w, false, 'M');
        h2 = HasCharactersInCorners(x, y, w, false, 'S');
        if (h1 != h2 && h1 != Side.None && h2 != Side.None)
            return true;
        
        return false;
    }

    private Side HasCharactersInCorners(int x, int y, World w, bool horizontal, char c)
    {
        if (horizontal)
        {
            if (w.At(x + 1, y + 1) == c && w.At(x - 1, y + 1) == c)
                return Side.First;

            if (w.At(x + 1, y - 1) == c && w.At(x - 1, y - 1) == c)
                return Side.Second;
        }
        else
        {
            if (w.At(x + 1, y + 1) == c && w.At(x + 1, y - 1) == c)
                return Side.First;

            if (w.At(x - 1, y + 1) == c && w.At(x - 1, y - 1) == c)
                return Side.Second;
        }

        return Side.None;
    }

    private int FindXMasInAllDirections(int x, int y, World w)
    {
        int found = 0;
        if (FindXMasInDirection(x, y, w, 1, 0))
            found++;
        if (FindXMasInDirection(x, y, w, -1, 0))
            found++;
        if (FindXMasInDirection(x, y, w, 0, 1))
            found++;
        if (FindXMasInDirection(x, y, w, 0, -1))
            found++;

        // Diagonals
        if (FindXMasInDirection(x, y, w, 1, 1))
            found++;
        if (FindXMasInDirection(x, y, w, -1, -1))
            found++;
        if (FindXMasInDirection(x, y, w, -1, 1))
            found++;
        if (FindXMasInDirection(x, y, w, 1, -1))
            found++;

        return found;
    }

    private bool FindXMasInDirection(int x, int y, World w, int dirX, int dirY)
    {
        string lookingFor = "MAS";
        for (int i = 0; i < 3; i++)
        {
            int mult = i + 1;
            var c = w.At(x + dirX * mult, y + dirY * mult);
            if (c != lookingFor[i])
                return false;
        }

        return true;
    }

    class World(string[] input)
    {
        public int Width { get; } = input[0].Length;
        public int Height { get; } = input.Length;

        public char At(int x, int y)
        {
            if (!InRange(x, y))
                return '.';
            try
            {
                return input[y][x];
            }
            catch
            {
                throw new Exception("Somehow failed, what?");
            }
        }

        public bool InRange(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }
    }
}