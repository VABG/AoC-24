using Avalonia.Media;

namespace AdventOfCode_24.Model.Visualization
{
    public struct Pixel(int x, int y, Color color)
    {
        public Color Color { get; } = color;
        public int X { get; } = x;
        public int Y { get; } = y;
    }
}
