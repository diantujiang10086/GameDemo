using System;
using System.Collections.Generic;

public class ConfigManager : Singleton<ConfigManager>
{
    private Dictionary<Type, BaseLoader> loaderDict = new Dictionary<Type, BaseLoader>();
    private Dictionary<Type, BaseLoader> configDict = new Dictionary<Type, BaseLoader>();
    public ConfigManager()
    {
        foreach (var item in AssemblyHelper.GetTypes<ConfigAttribute>())
        {
            var attribute = item.attribute as ConfigAttribute;
            var instance = Activator.CreateInstance(item.type) as BaseLoader;
            if (instance == null)
                continue;

            instance.Load(attribute.type);
            configDict[attribute.type] = instance;
            loaderDict[item.type] = instance;

        }
    }

    public T GetConfig<T>(int id) where T : class, IConfig
    {
        if (configDict.TryGetValue(typeof(T), out var loader))
        {
            return loader.Get(id) as T;
        }
        return default;
    }

    public IEnumerable<T> GetAllConifg<T>() where T : class, IConfig
    {
        if (configDict.TryGetValue(typeof(T), out var loader))
        {
            foreach(var item in loader.GetAll())
            {
                yield return item as T;
            }   
        }
    }

    public T GetLoader<T>() where T : BaseLoader
    {
        loaderDict.TryGetValue(typeof(T), out var loader);
        return loader as T;
    }
}