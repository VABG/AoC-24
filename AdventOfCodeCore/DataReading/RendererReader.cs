using AdventOfCodeCore.Interfaces;

namespace AdventOfCodeCore.DataReading;

public static class RendererReader
{
    private static readonly Lazy<IRendererImplementations?> RenderImplementations = new Lazy<IRendererImplementations?>(GetRendererImplementations);
    
    public static IPixelRenderer? GetPixelRenderer(int width, int height)
    {
        var implementation = RenderImplementations.Value;
        return implementation?.GetPixelRenderer(width, height);
    }

    public static ITextRenderer? GetCharacterRenderer(int width, int height)
    {
        var implementation = RenderImplementations.Value;
        return implementation?.GetCharacterRenderer(width, height);
    }

    private static IRendererImplementations? GetRendererImplementations()
    {
        var implementation = ReflectionHelper.TypesImplementingInterface(typeof(IRendererImplementations)).FirstOrDefault();
        if (implementation == null)
            return null;

        if (!ReflectionHelper.IsRealClass(implementation) || implementation.GetConstructor(Type.EmptyTypes) == null)
            return null;
        return Activator.CreateInstance(implementation) as IRendererImplementations;
    }
}