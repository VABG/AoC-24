using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace AdventOfCode_24.Model.Visualization;

public class PixelRenderer
{
    public  WriteableBitmap WriteableBitmap { get; }
    
    public PixelRenderer(int width, int height)
    {
        WriteableBitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96));
    }
    
    public void DrawPixel(Pixel pixel)
    {
        DrawPixels([pixel]);
    }

    public void DrawPixels(Pixel[] pixels)
    {
        using var lockedFrameBuffer = WriteableBitmap.Lock();
        unsafe
        {
            foreach (var pixel in pixels)
            {
                nint bufferPtr = new nint(lockedFrameBuffer.Address.ToInt64());
                bufferPtr += (int)WriteableBitmap.Size.Width * 4 * pixel.Y + pixel.X;
                *(int*)bufferPtr = (int)pixel.Color.ToUInt32();
            }
        }
    }

    public void Clear(Color color)
    {
        using var lockedFrameBuffer = WriteableBitmap.Lock();
        unsafe
        {
            nint bufferPtr = new nint(lockedFrameBuffer.Address.ToInt64());
            for (int y = 0; y < lockedFrameBuffer.Size.Height; y++)
            {
                for (int x = 0; x < lockedFrameBuffer.Size.Width; x++)
                {
                    *(int*)bufferPtr = (int)color.ToUInt32();
                    bufferPtr += 4;
                }
            }
        }
    }
}