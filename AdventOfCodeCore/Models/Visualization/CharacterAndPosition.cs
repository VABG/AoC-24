namespace AdventOfCodeCore.Models.Visualization;

public class CharacterAndPosition : Character
{
    public int X { get; }
    public int Y { get; }
    public CharacterAndPosition(char c, Color color, int x, int y) : base(c, color)
    {
        X = x;
        Y = y;
    }
}