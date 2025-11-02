namespace AdventOfCodeCore.Models.Settings;

[Serializable]
public class UserSettings
{
    public const string Path = @"C:/AoC/Settings/";
    public const string File = "UserSettings.xml";
    
    public string? Cookie { get; set; }
    public string? DllPath { get; set; }
}