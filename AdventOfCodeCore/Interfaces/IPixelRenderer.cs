using AdventOfCodeCore.Models.Visualization;

namespace AdventOfCodeCore.Interfaces;

public interface IPixelRenderer
{
    void DrawPixel(Pixel pixel);
    void DrawPixels(Pixel[] pixels);
    void DrawPixels(Pixel[,] pixels2d);
    void Clear(Color color);
}