using Avalonia.Media.Imaging;
using Avalonia;
using AdventOfCode_24.Model.Days;

namespace AdventOfCode_24.ViewModels.Sections
{
    public class VisualizationViewModel : DayBaseViewModel
    {
        public WriteableBitmap? WriteableBitmap => Day?.Renderer?.WriteableBitmap;

        private bool _wiggleState = true;
        private Thickness _wiggleThickness;

        public Thickness WiggleThickness
        {
            get => _wiggleThickness;
            set
            {
                _wiggleThickness = value;
                OnPropertyChanged(nameof(WiggleThickness));
            }
        }

        private void VisualizationOnUpdateVisuals()
        {
            OnPropertyChanged(nameof(WriteableBitmap));
            WiggleThickness = new Thickness(0, _wiggleState ? 0 : 1);
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
