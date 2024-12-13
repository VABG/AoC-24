using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AdventOfCode_24.Model.Visualization;

public class PixelRenderer
{
    public WriteableBitmap WriteableBitmap { get; }
    
    public PixelRenderer(int width, int height)
    {
        WriteableBitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Unpremul);
    }
    
    public void DrawPixel(Pixel pixel)
    {
        DrawPixels([pixel]);
    }

    public void DrawPixels(Pixel[] pixels)
    {
        using var lockedFrameBuffer = WriteableBitmap.Lock();
        foreach (var pixel in pixels)
            SafeDrawAt(pixel, lockedFrameBuffer);
    }

    public void DrawPixels(Pixel[,] pixels2d)
    {
        using var lockedFrameBuffer = WriteableBitmap.Lock();
        {
            for (int y = 0; y < pixels2d.GetLength(0); y++)
            {
                for (int x= 0; x < pixels2d.GetLength(1); x++)
                {
                    var pixel = pixels2d[y, x];
                    SafeDrawAt(pixel, lockedFrameBuffer);
                }
            }
        }
    }

    private void SafeDrawAt(Pixel pixel, ILockedFramebuffer lockedFrameBuffer)
    {
        if (pixel.X < 0 ||
            pixel.Y < 0 || 
            pixel.X >= lockedFrameBuffer.Size.Width ||
            pixel.Y >= lockedFrameBuffer.Size.Height)
            return;
        
        unsafe
        {
            nint bufferPtr = new nint(lockedFrameBuffer.Address.ToInt64());
            bufferPtr += (int)WriteableBitmap.Size.Width * (nint)4 * pixel.Y + (pixel.X*(nint)4);
            *(int*)bufferPtr = (int)pixel.Color.ToUInt32();
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