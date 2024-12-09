using System.IO;

namespace AdventOfCode_24.Model.WebConnection
{
    public static class CookieData
    {
        public static string ActiveCookie => GetCookie();
        private const string Path = @"C:/AoC/Cookie/";
        private const string File = "cookie.txt";
        private static string _cookie = "";

        public static void SetCookie(string cookie)
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            System.IO.File.WriteAllText(Path+File, cookie);
        }

        private static string GetCookie()
        {
            if (!string.IsNullOrEmpty(_cookie)) 
                return _cookie;
            if (System.IO.File.Exists(Path+File))
                _cookie = System.IO.File.ReadAllText(Path+File);

            return _cookie;
        }
    }
}
