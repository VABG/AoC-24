using AdventOfCode_24.Model.Days;

namespace AdventOfCode_24.ViewModels.Sections
{
    public class TestDataViewModel : DayBaseViewModel
    {
        private readonly MainViewModel _parent;

        public string? TestInput
        {
            get => Day?.Data?.TestInput;
            set
            {
                if (Day != null && Day.Data != null)
                    Day.Data.TestInput = value;

                _parent.OnPropertyChanged(nameof(_parent.CanRunTest));
                OnPropertyChanged(nameof(TestInput));
            }
        }

        public string? TestResult
        {
            get => Day?.Data?.GetExpectedForPart(Part);
            set
            {
                if (Day is { Data: not null })
                {
                    Day.Data.SetExpectedForPart(Part, value);
                }

                OnPropertyChanged(nameof(TestResult));
            }
        }

        public TestDataViewModel(MainViewModel parent)
        {
            this._parent = parent;
        }

        protected override void UpdateDay(Day? previous)
        {
            OnPropertyChanged(nameof(TestInput));
            OnPropertyChanged(nameof(TestResult));
        }

        protected override void UpdatePart(int? previous)
        {
            OnPropertyChanged(nameof(TestResult));
        }

        public void SaveTestData()
        {
            Day?.WriteData();
        }
    }
}
