using System.Diagnostics;
using System.Net;
using System.Xml.Serialization;
using AdventOfCodeCore.Models.Days;
using HtmlAgilityPack;

namespace AdventOfCodeCore.Models.WebConnection;

public static class SiteDataReader
{
    private const string FilePath = @"C:/AoC/";
    private const string AoCSitePath = @"https://adventofcode.com";

    public static async Task<(DayData?, bool)> ReadDayData(Day day)
    {
        var path = FilePath + day.Year + "_" + day.DayNumber + ".xml";
        if (!Directory.Exists(FilePath))
            Directory.CreateDirectory(FilePath);
        if (File.Exists(path))
            return (ReadXml(path), true);

        var lines = await ReadInput(day.Year, day.DayNumber);
        if(!lines.Item2)
            return (null, false);
        
        var data = new DayData(lines.Item1);
        WriteXml(data, path);
        return (data, true);
    }

    public static async Task<(string, bool)> ReadDayDescription(Day day)
    {
        try
        {
            // TODO: Add error logging
            var page = await ReadAoCPage($"{day.Year}/day/{day.DayNumber}");
            if (!page.Item2)
                return page;
            
            var doc = new HtmlDocument();
            doc.LoadHtml(page.Item1);

            var descr = doc.DocumentNode.SelectNodes("//html/body/main/article");
            var res = string.Empty;
            for (int i = 0; i < descr.Count; i++)
            {
                res += HtmlUtilities.ConvertToPlainText(descr[i].InnerHtml);
                if (i < descr.Count - 1)
                    res += "\n\n\n";
            }

            return (res, true);
        }
        catch (Exception ex)
        {
            return (ex.Message, false);
        }
    }

    public static void WriteXml(DayData dayData, int year, int day)
    {
        var path = FilePath + year + "_" + day + ".xml";
        WriteXml(dayData, path);
    }

    private static void WriteXml(DayData data, string path)
    {
        var serializer = new XmlSerializer(typeof(DayData));
        using TextWriter textWriter = new StreamWriter(path);
        serializer.Serialize(textWriter, data);
    }

    private static DayData ReadXml(string path)
    {
        var serializer = new XmlSerializer(typeof(DayData));
        using TextReader textReader = new StreamReader(path);
        var data = serializer.Deserialize(textReader);
        if (data is not DayData dayData)
            throw new Exception("Failed to read Serialized Day");
        return dayData;
    }

    private static async Task<(string, bool)> ReadInput(int year, int day)
    {
        return await ReadAoCPage($"{year}/day/{day}/input");
    }

    private static async Task<(string, bool)> ReadAoCPage(string page)
    {
        var cookie = Settings.Settings.User.Value.Cookie;
        if (string.IsNullOrEmpty(cookie))
            throw new Exception("Cookie is missing/empty!");

        var cookies = new CookieContainer();
        using var handler = new HttpClientHandler();
        handler.CookieContainer = cookies;

        var uri = new Uri(AoCSitePath);
        using var client = new HttpClient(handler);
        client.BaseAddress = uri;
        cookies.Add(uri, new Cookie("session", cookie));

        var response = await client.GetAsync(page);
        if (response.StatusCode != HttpStatusCode.OK)
            return ("Issue reading from site, please check your cookie or internet connection", false);
        
        var stream = await response.Content.ReadAsStreamAsync();
        var sr = new StreamReader(stream);
        return (await sr.ReadToEndAsync(), true);
    }

    public static void OpenSite(int year, int day)
    {
        var site = AoCSitePath + "/" + year + "/day/" + day;
        Process.Start(new ProcessStartInfo(site) { UseShellExecute = true });
    }
}