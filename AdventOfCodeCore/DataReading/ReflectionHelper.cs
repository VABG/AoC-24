namespace AdventOfCodeCore.DataReading;

public static class ReflectionHelper
{
    public static IEnumerable<Type> TypesImplementingInterface(Type desiredType)
    {
        return AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(desiredType.IsAssignableFrom);
    }
    
    public static bool IsRealClass(Type testType)
    {
        return testType is { IsAbstract: false, IsGenericTypeDefinition: false, IsInterface: false };
    }


}