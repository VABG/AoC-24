using System.Diagnostics;
using System.Net;
using System.Xml.Serialization;
using AdventOfCodeCore.Models.Days;
using HtmlAgilityPack;

namespace AdventOfCodeCore.Models.WebConnection;

public class DayInputReader
{
    private const string FilePath = @"C:/AoC/";
    private const string AoCSitePath = @"https://adventofcode.com";

    public static async Task<DayData> ReadDayData(IDay day)
    {
        var path = FilePath + day.Year + "_" + day.DayNumber + ".xml";
        if (!Directory.Exists(FilePath))
            Directory.CreateDirectory(FilePath);
        if (File.Exists(path))
            return ReadXml(path);

        var lines = await ReadInput(day.Year, day.DayNumber);
        var data = new DayData(lines);
        WriteXml(data, path);
        return data;
    }

    public static async Task<string?> ReadDayDescription(IDay day)
    {
        try
        {
            var page = await ReadAoCPage($"{day.Year}/day/{day.DayNumber}");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            var descr = doc.DocumentNode.SelectNodes("//html/body/main/article");
            var res = string.Empty;
            for (int i = 0; i < descr.Count; i++)
            {
                res += HtmlUtilities.ConvertToPlainText(descr[i].InnerHtml);
                if (i < descr.Count - 1)
                    res += "\n\n\n";
            }

            return res;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public static void WriteXml(DayData dayData, int year, int day)
    {
        var path = FilePath + year + "_" + day + ".xml";
        WriteXml(dayData, path);
    }

    private static void WriteXml(DayData data, string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DayData));
        using TextWriter textWriter = new StreamWriter(path);
        serializer.Serialize(textWriter, data);
    }

    private static DayData ReadXml(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DayData));
        using TextReader textReader = new StreamReader(path);
        var data = serializer.Deserialize(textReader);
        if (data is not DayData dayData)
            throw new Exception("Failed to read Serialized Day");
        return dayData;
    }

    private static async Task<string> ReadInput(int year, int day)
    {
        return await ReadAoCPage($"{year}/day/{day}/input");
    }

    private static async Task<string> ReadAoCPage(string page)
    {
        var cookie = CookieData.ActiveCookie;
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
        var stream = await response.Content.ReadAsStreamAsync();

        var sr = new StreamReader(stream);
        return await sr.ReadToEndAsync();
    }

    public static void OpenSite(int year, int day)
    {
        var site = AoCSitePath + year + "/day/" + day;
        Process.Start(new ProcessStartInfo(site) { UseShellExecute = true });
    }
}