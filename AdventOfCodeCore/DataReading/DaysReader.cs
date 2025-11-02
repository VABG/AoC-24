using System.Linq.Expressions;
using System.Reflection;
using AdventOfCodeCore.Models.Days;

namespace AdventOfCodeCore.DataReading
{
    public static class DaysReader
    {
        public static Dictionary<int, List<Day>> ReadYearsAndDays(string? dllFilePath)
        {
            if (string.IsNullOrWhiteSpace(dllFilePath))
                return [];
            var dll = TryLoadAssembly(dllFilePath);
            if (dll is null)
                return [];
            
            var types = dll.GetExportedTypes().Where(t => typeof(IDay).IsAssignableFrom(t)).ToArray();

            var instances = GetInstances(types);
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

        private static Assembly? TryLoadAssembly(string dllFilePath)
        {
            try
            {
                return Assembly.LoadFile(dllFilePath);
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