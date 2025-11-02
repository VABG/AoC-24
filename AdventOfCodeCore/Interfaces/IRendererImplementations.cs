namespace AdventOfCodeCore.Interfaces;

public interface IRendererImplementations
{
    IPixelRenderer? GetPixelRenderer(int width, int height);
    ITextRenderer? GetCharacterRenderer(int width, int height);
}