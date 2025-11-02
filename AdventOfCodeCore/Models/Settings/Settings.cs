using System.Xml.Serialization;

namespace AdventOfCodeCore.Models.Settings;

public static class Settings
{
    public static readonly Lazy<UserSettings> User = new(ReadOrCreateSettings);
    private const string FilePath = UserSettings.Path + UserSettings.File;
    
    private static bool _hasSettings ;
    public static bool HasSettings {
        get
        {
            if(User.IsValueCreated)
                return _hasSettings;
            _ = User.Value;
            return _hasSettings;
        } 
        private set => _hasSettings = value;
    }
    
    public static void SaveSettings()
    {
        if (!Directory.Exists(UserSettings.Path))
            Directory.CreateDirectory(UserSettings.Path);

        var serializer = new XmlSerializer(typeof(UserSettings));
        using var fileStream = new FileStream(FilePath, FileMode.Create);
        serializer.Serialize(fileStream, User.Value);
    }

    private static UserSettings ReadOrCreateSettings()
    {
        if (!File.Exists(FilePath))
            return new UserSettings();

        var serializer = new XmlSerializer(typeof(UserSettings));
        using var fileStream = new FileStream(FilePath, FileMode.OpenOrCreate);

        try
        {
            if (serializer.Deserialize(fileStream) is not UserSettings userSettings)
                return new UserSettings();
            HasSettings = true;
            return userSettings;
        }
        catch(Exception exception)
        {
            HasSettings = false;
            return new UserSettings();
        }
    }
}