using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AdventOfCodeCore.Models.Days;
using AdventOfCodeCore.Models.WebConnection;

namespace AdventOfCodeUI.ViewModels.Sections;

public class DescriptionViewModel : DayBaseViewModel
{
    private readonly Dictionary<string, string> _descriptions = [];
    private const string Path = @"C:\AoC\DaySites\";

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

    public async Task Refresh()
    {
        if (Day == null)
            return;

        var d = await SiteDataReader.ReadDayDescription(Day);

        Description = d.Item1;
        
        if (!d.Item2)
            return;
        
        Description = d.Item1;
        WriteCurrentDay();
    }
    
    protected override void UpdateDay(Day? previous)
    {
        if (_descriptions.Count == 0)
            ReadAllDays();

        if (Day == null)
        {
            Description = null;
            return;
        }

        var dayStr = DayToString();
        Description = _descriptions.GetValueOrDefault(dayStr);
    }

    private string DayToString()
    {
        if (Day == null)
            return string.Empty;
        
        return Day.Year.ToString() + Day.DayNumber;
    }
    
    protected override void UpdatePart(int? previous)
    {
        // do nothing
    }

    private void WriteCurrentDay()
    {
        var di = new DirectoryInfo(Path);
        if (!di.Exists)
            di.Create();

        File.WriteAllText(Path + DayToString() + ".txt", Description);
    }

    private void ReadAllDays()
    {
        DirectoryInfo di = new(Path);
        if (!di.Exists)
            return;

        var files = di.GetFiles();
        foreach(var f in files)
        {
            var str = File.ReadAllText(f.FullName);
            if (str.Length == 0)
                continue;
            _descriptions[f.Name.Substring(0, f.Name.Length-f.Extension.Length)] = str;
        }
    }
}