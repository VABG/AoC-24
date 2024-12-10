using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace AdventOfCode_24.Model.Visualization
{
    public class RenderControl : Control
    {
        private WriteableBitmap? _writeableBitmap = null;
        
        public void UpdateBitmap(WriteableBitmap? bitmap)
        {
            if (_writeableBitmap == bitmap)
                return;
            
            _writeableBitmap = bitmap;

            if (bitmap == null)
            {
                this.Height = 128;
                this.Height = 128;
                return;
            }
            
            this.Width = bitmap.Size.Width;
            this.Height = bitmap.Size.Height;
        }
        public override void Render(DrawingContext context)
        {
            base.Render(context);
            if (_writeableBitmap != null)
                context.DrawImage(_writeableBitmap, Bounds);
        }

        public void Update()
        {
            InvalidateVisual();
        }
    }
}
