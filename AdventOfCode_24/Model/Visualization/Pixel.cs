using Avalonia.Media;
using System;

namespace AdventOfCode_24.Model.Visualization
{
    public struct Pixel(int x, int y, Color color)
    {
        public Color Color { get; set; } = color;
        public int X { get; } = x;
        public int Y { get; } = y;

        public Color Mix(Color other, float interpolation)
        {
            float a1 = AlphaToMult(Color.A);
            float a2 = AlphaToMult(other.A);
            return new Color(
                Interpolate(Color.A, other.A, interpolation),
                Interpolate(Color.R * a1, other.R * a2, interpolation),
                Interpolate(Color.G * a1, other.G * a2, interpolation),
                Interpolate(Color.B * a1, other.B * a2, interpolation));
        }

        private float AlphaToMult(byte alpha)
        {
            return (float)alpha / (float)255.0f;
        }

        private byte Interpolate(float b1, float b2, float i)
        {
            float v = b1 * (1.0f - i) + b2 * i;
            return (byte)v;
        }
    }
}
