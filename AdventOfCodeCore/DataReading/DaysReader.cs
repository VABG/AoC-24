using System.Linq.Expressions;
using System.Reflection;
using AdventOfCodeCore.Models.Days;

namespace AdventOfCodeCore.DataReading
{
    public static class DaysReader
    {
        public static Dictionary<int, List<Day>> ReadYearsAndDays(string? dllFolderPath)
        {
            if (string.IsNullOrWhiteSpace(dllFolderPath))
                return [];

            var instances = TryGetAssemblyWithDays(dllFolderPath);
            if (instances == null || instances.Length == 0)
                return [];
            
            Dictionary<int, List<Day>> days = [];
            foreach (var instance in instances)
            {
                ReadParts(instance);
                if (!days.ContainsKey(instance.Year))
                    days[instance.Year] = [];

                if (instance is not Day day)
                    continue;
                days[instance.Year].Add(day);
            }

            foreach (var year in days.Values)
                year.Sort();

            return days;
        }

        private static IDay[]? TryGetAssemblyWithDays(string dllFolderPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dllFolderPath);
            var dlls = dirInfo.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

            return dlls.Select(dll => TryGetAssemblyFromDll(dll.FullName))
                .OfType<IDay[]>()
                .FirstOrDefault(days => days.Length != 0);
        }

        private static IDay[]? TryGetAssemblyFromDll(string dllFilePath)
        {
            var dll = TryLoadAssembly(dllFilePath);
            if (dll is null)
                return null;
            var types = dll.GetTypes().Where(t => typeof(IDay).IsAssignableFrom(t)).ToArray();
            var instances = GetInstances(types);
            return instances.Length != 0 ? instances : null;
        }

        private static Assembly? TryLoadAssembly(string dllFilePath)
        {
            try
            {
                return Assembly.LoadFrom(dllFilePath);
            }
            catch
            {
                return null;
            }
        }

        private static void ReadParts(object day)
        {
            if (day is not Day d)
                return;

            var methods = day.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            Dictionary<int, Func<string>> partMethods = [];
            foreach (var method in methods)
            {
                var methodName = method.Name;
                if (!methodName.StartsWith("Part") || method.ReturnType != typeof(string))
                    continue;

                var nrString = methodName.Substring(4, methodName.Length - 4);
                if (!int.TryParse(nrString, out var nr))
                    return;

                var result = Expression.Lambda<Func<string>>(
                    Expression.Call(Expression.Constant(day), method)).Compile();

                partMethods.Add(nr, result);
            }

            d.SetParts(partMethods);
        }

        private static IDay[] GetInstances(Type[] days)
        {
            List<IDay> implementations = [];
            foreach (var day in days)
            {
                if (!ReflectionHelper.IsRealClass(day) || day.GetConstructor(Type.EmptyTypes) == null)
                    continue;
                if (Activator.CreateInstance(day) is not IDay instance)
                    continue;

                implementations.Add(instance);
            }

            return implementations.ToArray();
        }
    }
}