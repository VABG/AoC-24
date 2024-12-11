using AdventOfCode_24.Model.Days;

namespace AdventOfCode_24.ViewModels.Sections
{
    public class TestDataViewModel : DayBaseViewModel
    {
        public bool CanRunTest => Day?.Data?.TestInput != null
                       && !Day.IsRunning;

        public string? TestInput
        {
            get => Day?.Data?.TestInput;
            set
            {
                if (Day != null && Day.Data != null)
                    Day.Data.TestInput = value;
                OnPropertyChanged(nameof(TestInput));
                OnPropertyChanged(nameof(CanRunTest));
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

        protected override void UpdateDay(Day? previous)
        {
            OnPropertyChanged(nameof(TestInput));
            OnPropertyChanged(nameof(CanRunTest));
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
