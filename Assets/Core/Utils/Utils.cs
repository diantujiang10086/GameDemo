using System;
using System.Collections.Generic;
using System.Reflection;

public static class Utils
{
    public static IEnumerable<(Type type, T attribute)> GetTypes<T>(List<Assembly> assemblies) where T : BaseAttribute
    {
        var attrType = typeof(T);
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                var attrs = type.GetCustomAttributes(attrType, true);
                if (attrs.Length == 0)
                    continue;

                yield return (type, attrs[0] as T);
            }
        }
    }
}