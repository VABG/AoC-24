namespace AdventOfCodeCore.Models.Visualization;

public readonly struct Color(byte r, byte g, byte b, byte a) : IEquatable<Color>
{
    public readonly byte R = r, G = g, B = b, A = a;

    public uint ToUInt32()
    {
        return ((uint)A << 24) | ((uint)R << 16) | ((uint)G << 8) | B;
    }

    public bool Equals(Color other)
    {
        return R == other.R && G == other.G && B == other.B && A == other.A;
    }

    public override bool Equals(object? obj)
    {
        return obj is Color other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(R, G, B, A);
    }

    public static bool operator ==(Color left, Color right) => left.Equals(right);
    
    public static bool operator !=(Color left, Color right) => !(left == right);
    
}