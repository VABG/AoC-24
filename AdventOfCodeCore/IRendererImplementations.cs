using AdventOfCodeCore.Interfaces;

namespace AdventOfCodeCore;

public interface IRendererImplementations
{
    IPixelRenderer? GetPixelRenderer(int width, int height);
    ITextRenderer? GetCharacterRenderer(int width, int height);
}