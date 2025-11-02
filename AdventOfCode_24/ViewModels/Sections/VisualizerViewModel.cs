using AdventOfCodeCore.Models.Days;
using AdventOfCodeUI.ViewModels.Rendering;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace AdventOfCodeUI.ViewModels.Sections
{
    public class VisualizerViewModel : DayBaseViewModel
    {
        public WriteableBitmap? WriteableBitmap { get; private set; }

        private bool _wiggleState = true;
        private Color _background;

        public Color Background
        {
            get => _background;
            set
            {
                _background = value;
                OnPropertyChanged(nameof(Background));
            }
        }

        private void VisualizationOnUpdateVisuals()
        {
            UpdateBitmap();
            Background = new Color(_wiggleState ? (byte)0 : (byte)1, 0, 0, 0);
            _wiggleState = !_wiggleState;
        }

        protected override void UpdateDay(Day? previous)
        {
            UpdateBitmap();
            
            if (previous != null)
                previous.UpdateVisuals -= VisualizationOnUpdateVisuals;

            if (Day == null)
                return;

            Day.UpdateVisuals += VisualizationOnUpdateVisuals;
        }

        private void UpdateBitmap()
        {
            if (Day?.PixelRenderer is PixelRenderer pixelRenderer)
                WriteableBitmap = pixelRenderer.WriteableBitmap;
            OnPropertyChanged(nameof(WriteableBitmap));
        }

        protected override void UpdatePart(int? previous)
        {
            // Do nothing
        }
    }
}
