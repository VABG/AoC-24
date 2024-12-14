using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode_24.Model.Days;
using AdventOfCode_24.Model.WebConnection;

namespace AdventOfCode_24.ViewModels.Sections;

public class DescriptionViewModel : DayBaseViewModel
{
    private Dictionary<string, string> _descriptions = new Dictionary<string, string>();
    string path = @"C:\AoC\DaySites\";

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
        if (d == null || string.IsNullOrEmpty(d))
            return;

        WriteCurrentDay();
    }
    
    protected override void UpdateDay(Day? previous)
    {
        if (_descriptions == null || _descriptions.Count == 0)
        {
            ReadAllDays();
        }


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

    private void WriteCurrentDay()
    {
        DirectoryInfo di = new DirectoryInfo(path);
        if (!di.Exists)
            di.Create();


        File.WriteAllText(path + DayToString() + ".html", Description);
    }

    private void ReadAllDays()
    {
        DirectoryInfo di = new(path);
        if (!di.Exists)
            return;

        var files = di.GetFiles();
        foreach(var f in files)
        {
            var str = File.ReadAllText(f.FullName);
            if (str == null)
                continue;
            _descriptions[f.Name.Substring(0, f.Name.Length-f.Extension.Length)] = str;
        }
    }
}