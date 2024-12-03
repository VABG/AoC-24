using System.IO;

namespace AdventOfCode_24.Model.WebConnection
{
    public static class CookieData
    {
        public static string ActiveCookie => GetCookie();
        private const string path = @"C:/AoC/Cookie/cookie.txt";
        private static string _cookie = "";

        public static void SetCookie(string cookie)
        {
            File.WriteAllText(path, cookie);
        }

        private static string GetCookie()
        {
            if (string.IsNullOrEmpty(_cookie))
            {
                if (File.Exists(path))
                _cookie = File.ReadAllText(path);
            }
                
            return _cookie;
        }
    }
}
