using System;
using System.Collections.Generic;
using System.Linq;

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
                if (!Days.ContainsKey(instance.Year))
                    Days[instance.Year] = new List<Day>();

                Days[instance.Year].Add(instance as Day);
            }

            foreach (var year in Days.Values)
            {
                year.Sort();
            }

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
