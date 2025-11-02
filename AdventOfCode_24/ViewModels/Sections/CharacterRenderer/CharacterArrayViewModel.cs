using System.Collections.ObjectModel;

namespace AdventOfCodeUI.ViewModels.Sections;

public class CharacterArrayViewModel
{
    public ObservableCollection<ObservableCollection<CharacterArrayViewModel>> Array;

    public CharacterArrayViewModel()
    {
        Array = new ObservableCollection<ObservableCollection<CharacterArrayViewModel>>();
    }
}