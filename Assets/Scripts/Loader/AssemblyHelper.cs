using System;
using System.Collections.Generic;
using System.Reflection;

public static class AssemblyHelper
{
    private static Assembly[] assemblies; 
    public static Assembly[] Assemblies
    {
        get => assemblies;
        internal set
        {
            assemblies = value;
        }
    }

    public static IEnumerable<(Type type, T attribute)> GetTypes<T>() where T : class
    {
        var attrType = typeof(T);
        foreach (var assembly in Assemblies)
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