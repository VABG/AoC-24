namespace AdventOfCodeCore.Models.Visualization
{
    public struct Pixel(int x, int y, Color color)
    {
        public Color Color { get; set; } = color;
        public int X { get; } = x;
        public int Y { get; } = y;

        public Color Mix(Color other, float interpolation)
        {
            var a1 = AlphaToMult(Color.A);
            var a2 = AlphaToMult(other.A);
            return new Color(
                Interpolate(Color.R * a1, other.R * a2, interpolation),
                Interpolate(Color.G * a1, other.G * a2, interpolation),
                Interpolate(Color.B * a1, other.B * a2, interpolation),
                Interpolate(Color.A, other.A, interpolation));
        }

        private float AlphaToMult(byte alpha)
        {
            return alpha / 255.0f;
        }

        private byte Interpolate(float b1, float b2, float i)
        {
            var v = b1 * (1.0f - i) + b2 * i;
            return (byte)v;
        }
    }
}
