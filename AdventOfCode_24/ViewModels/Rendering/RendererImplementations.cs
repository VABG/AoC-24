using AdventOfCodeCore;
using AdventOfCodeCore.Interfaces;

namespace AdventOfCodeUI.ViewModels.Rendering;

public class RendererImplementations : IRendererImplementations
{
    public IPixelRenderer? GetPixelRenderer(int width, int height)
    {
        return new PixelRenderer(width, height);
    }

    public ITextRenderer? GetCharacterRenderer(int width, int height)
    {
        return new TextRenderer(width, height);
    }
}
