
using System;
using System.Collections.Generic;

public interface ISystem
{

}

public class SystemManager
{
    private static SystemManager instance;
    private Dictionary<Type, ISystem> dict = new Dictionary<Type, ISystem>();

    public SystemManager()
    {
        instance = this;
        dict.Clear();
    }

    public static T AddSystem<T>() where T : class, ISystem
    {
        var system = Activator.CreateInstance(typeof(T)) as ISystem;
        if(system is IAwake awake)
        {
            awake.Awake();
        }
        instance.dict[typeof(T)] = system;
        return system as T;
    }

    public static T GetSystem<T>()where T : class, ISystem
    {
        instance.dict.TryGetValue(typeof(T), out var system);
        return system as T;
    }
}