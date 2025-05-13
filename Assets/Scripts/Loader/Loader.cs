using System;
using System.Collections.Generic;
using System.Reflection;

public static class Loader
{
    private static HashSet<string> loadAssemblys = new HashSet<string> 
    {
        "Logic","View"
    };
    public static void LoadAll()
    {
        Assembly startAssembly = default;
        List<Assembly> list = new List<Assembly>();
        foreach (var assemblies in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (loadAssemblys.Contains(assemblies.GetName().Name))
            {
                list.Add(assemblies);
                if(assemblies.GetName().Name == "Logic")
                {
                    startAssembly = assemblies;
                }
            }
        }
        AssemblyHelper.Assemblies = list.ToArray();

        var type = startAssembly.GetType("Entry");
        var method = type.GetMethod("Start", BindingFlags.Public | BindingFlags.Static);
        method.Invoke(null, new object[] { } );
    }
}
