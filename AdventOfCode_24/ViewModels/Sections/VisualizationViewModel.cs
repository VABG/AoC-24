using Avalonia.Media.Imaging;
using Avalonia;
using AdventOfCode_24.Model.Days;
using Avalonia.Media;

namespace AdventOfCode_24.ViewModels.Sections
{
    public class VisualizationViewModel : DayBaseViewModel
    {
        public WriteableBitmap? WriteableBitmap => Day?.Renderer?.WriteableBitmap;

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
            OnPropertyChanged(nameof(WriteableBitmap));
            Background = new Color(_wiggleState ? (byte)0 : (byte)1, 0, 0, 0);
            _wiggleState = !_wiggleState;
        }

        protected override void UpdateDay(Day? previous)
        {
            OnPropertyChanged(nameof(WriteableBitmap));

            if (previous != null)
                previous.UpdateVisuals -= VisualizationOnUpdateVisuals;

            if (Day == null)
                return;

            Day.UpdateVisuals += VisualizationOnUpdateVisuals;

        }

        protected override void UpdatePart(int? previous)
        {
            // Do nothing
        }
    }
}
