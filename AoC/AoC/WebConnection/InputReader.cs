using System.Net;

namespace AoC.WebConnection;

public class InputReader
{
    private const string FilePath = "C:/AoC24Input/"; // TODO: Change location to somewhere relative to solution
    public static async Task<string[]> Read(int day)
    {
        var path = FilePath + day + ".txt";
        if (!Directory.Exists(FilePath))
            Directory.CreateDirectory(FilePath);
        if (File.Exists(path))
            return await File.ReadAllLinesAsync(path);
        
        var webResponse = await ReadFromWeb(day);
        var lines = webResponse.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
        
        File.WriteAllLines(path, lines);
        return lines;
    }

    private static async Task<string> ReadFromWeb(int day)
    {
        // TODO: Get from UI, keep on drive (not in solution)
        const string cookie =
            "53616c7465645f5f9c27823efd2f74cf76d89dc71f7864b42be1a19d472a6c9a6417742f4e06c98a001e03b000cf6ec32b24ee30919fd1e5504667be157e65fd";
        var cookies = new CookieContainer();
        using var handler = new HttpClientHandler();
        handler.CookieContainer = cookies;

        var uri = new Uri("https://adventofcode.com");
        using var client = new HttpClient(handler);
        client.BaseAddress = uri;
        cookies.Add(uri, new Cookie("session", cookie));

        var response = await client.GetAsync($"2024/day/{day}/input");
        var stream = await response.Content.ReadAsStreamAsync();

        StreamReader sr = new StreamReader(stream);
        return await sr.ReadToEndAsync();
    }
}