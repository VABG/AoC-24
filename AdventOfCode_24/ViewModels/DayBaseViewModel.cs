using AdventOfCodeCore.Models.Days;

namespace AdventOfCodeUI.ViewModels
{
    public abstract class DayBaseViewModel : ViewModelBase
    {
        protected Day? Day;
        protected int? Part;

        public void SetDay(Day? day)
        {
            var previous = Day;
            Day = day;
            UpdateDay(previous);
        }
        public void SetPart(int? part)
        {
            var previous = Part;
            Part = part;
            UpdatePart(previous);
        }

        protected abstract void UpdateDay(Day? previous);
        protected abstract void UpdatePart(int? previous);
    }
}
