using Avalonia.Media.Imaging;
using System.ComponentModel;

namespace AdventOfCode_24.ViewModels;

public class MainViewModel : ViewModelBase, INotifyPropertyChanged
{
    Bitmap? _visualization = null;
    public Bitmap? Visualization
    {
        get => _visualization;
        set => OnPropertyChanged(nameof(Visualization));
    }

    private string _log = "";
    public string Log
    {
        get => _log;
        set => OnPropertyChanged(nameof(Log));
    }



}
