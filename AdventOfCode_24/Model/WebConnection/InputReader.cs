using AdventOfCode_24.Model.Days;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdventOfCode_24.Model.WebConnection;

public class InputReader
{
    private const string FilePath = @"C:/AoC/";

    public static async Task<DayData> Read(IDay day)
    {
        var path = FilePath + day.Year + "_" + day.DayNumber + ".xml";
        if (!Directory.Exists(FilePath))
            Directory.CreateDirectory(FilePath);
        if (!File.Exists(path))
        {
            var lines = await Read(day.Year, day.DayNumber);
            var data = new DayData(lines);
            WriteXml(data, path);
            return data;
        }

        return ReadXml(path);
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
        var data=  serializer.Deserialize(textReader);
        if (data is not DayData dayData)
            throw new Exception("Failed to read Serialized Day");
        return dayData;
    }

    private static async Task<string> Read(int year, int day)
    {
        return await ReadFromWeb(year, day);
    }

    private static async Task<string> ReadFromWeb(int year, int day)
    {
        var cookie = CookieData.ActiveCookie;
        if (string.IsNullOrEmpty(cookie))
            throw new Exception("Cookie is missing/empty!");

        var cookies = new CookieContainer();
        using var handler = new HttpClientHandler();
        handler.CookieContainer = cookies;

        var uri = new Uri("https://adventofcode.com");
        using var client = new HttpClient(handler);
        client.BaseAddress = uri;
        cookies.Add(uri, new Cookie("session", cookie));

        var response = await client.GetAsync($"{year}/day/{day}/input");
        var stream = await response.Content.ReadAsStreamAsync();

        StreamReader sr = new StreamReader(stream);
        return await sr.ReadToEndAsync();
    }

    public static void OpenSite(int year, int day)
    {
        var site = @"https://adventofcode.com/" + year + "/day/" + day;
        Process.Start(new ProcessStartInfo(site) { UseShellExecute = true });
    }
}