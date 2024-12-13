using System.Collections.Generic;
using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.WebConnection;

namespace AdventOfCode_24.ViewModels.Sections;

public class DescriptionViewModel : DayBaseViewModel
{
    private Dictionary<string, string> _descriptions = new Dictionary<string, string>();
    
    private string? _description;
    public string? Description
    {
        get => _description;
        set
        {
            _description = value;
            if (value != null)
                _descriptions[DayToString()] = value;
            OnPropertyChanged(nameof(Description));
        }
    } 
    public async void Refresh()
    {
        if (Day == null)
            return;

        var d = await InputReader.ReadDayDescription(Day);
        Description = d;
    }
    
    protected override void UpdateDay(Day? previous)
    {
        if (Day == null)
        {
            Description = null;
            return;
        }

        string dayStr = DayToString();
        if (_descriptions.ContainsKey(dayStr))
            Description = _descriptions[dayStr];

        else Description = null;
    }

    private string DayToString()
    {
        if (Day == null)
            return string.Empty;
        
        return Day.Year.ToString() + Day.DayNumber;
        ;
    }
    
    protected override void UpdatePart(int? previous)
    {
        // do nothing
    }
}