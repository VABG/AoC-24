using System.ComponentModel;
using ReactiveUI;

namespace AdventOfCodeUI.ViewModels;

public class ViewModelBase : ReactiveObject, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
