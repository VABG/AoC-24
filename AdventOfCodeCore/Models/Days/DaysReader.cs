using System.Linq.Expressions;
using System.Reflection;

namespace AdventOfCodeCore.Models.Days
{
    public class DaysReader
    {
        public Dictionary<int, List<Day>> Days { get; } = [];

        public DaysReader()
        {
            var types = TypesImplementingInterface(typeof(IDay)).ToList();
            var instances = GetInstances(types);

            foreach (var instance in instances)
            {
                ReadParts(instance);
                if (!Days.ContainsKey(instance.Year))
                    Days[instance.Year] = [];

                if (instance is not Day day)
                    continue;
                Days[instance.Year].Add(day);
            }

            foreach (var year in Days.Values)
            {
                year.Sort();
            }
        }
        
        private void ReadParts(object day)
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

                var nrString = methodName.Substring(4, methodName.Length-4);
                if (!int.TryParse(nrString, out var nr))
                    return;
                
                var result = Expression.Lambda<Func<string>>(
                    Expression.Call(Expression.Constant(day), method)).Compile();

                partMethods.Add(nr, result);
            }
            
            d.SetParts(partMethods);
        }

        private List<IDay> GetInstances(List<Type> days)
        {
            List<IDay> implementations = [];
            foreach (var day in days)
            {
                if (!IsRealClass(day) || day.GetConstructor(Type.EmptyTypes) == null)
                    continue;
                var instance = Activator.CreateInstance(day) as IDay;
                if (instance == null)
                    continue;

                implementations.Add(instance);
            }

            return implementations;
        }

        private static bool IsRealClass(Type testType)
        {
            return testType.IsAbstract == false
                 && testType.IsGenericTypeDefinition == false
                 && testType.IsInterface == false;
        }

        private static IEnumerable<Type> TypesImplementingInterface(Type desiredType)
        {
            return AppDomain
                   .CurrentDomain
                   .GetAssemblies()
                   .SelectMany(assembly => assembly.GetTypes())
                   .Where(desiredType.IsAssignableFrom);
        }
    }
}
