using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AdventOfCode_24.Model.Days
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
                    Days[instance.Year] = new List<Day>();

                Days[instance.Year].Add(instance as Day);
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
                string methodName = method.Name;
                if (!methodName.StartsWith("Part"))
                    continue;

                var nrString = methodName.Substring(4, methodName.Length-4);
                if (!int.TryParse(nrString, out var nr))
                    return;
                
                Func<string> result = Expression.Lambda<Func<string>>(
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
                   .Where(type => desiredType.IsAssignableFrom(type));
        }
    }
}
