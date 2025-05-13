using System;
using System.Collections.Generic;
using System.Reflection;

public static class AssemblyHelper
{
    private static Assembly[] g_assemblies; 
    public static void SetAssemblies(Assembly[] assemblies)
    {
        g_assemblies = assemblies;
    }

    public static IEnumerable<(Type type, T attribute)> GetTypes<T>() where T : class
    {
        var attrType = typeof(T);
        foreach (var assembly in g_assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                var attrs = type.GetCustomAttributes(attrType, true);
                if (attrs.Length == 0)
                    continue;

                foreach (var attr in attrs)
                {
                    yield return (type, attr as T);
                }
            }
        }
    }

}