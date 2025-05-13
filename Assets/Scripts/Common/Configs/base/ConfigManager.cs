using System;
using System.Collections;
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
        //var types = World.GetComponent<CodeTypes>().GetTypes(configAttribute);

        //foreach ((string _, Type type) in types)
        //{
        //    object[] attrs = type.GetCustomAttributes(configAttribute, false);
        //    if (attrs.Length == 0)
        //        continue;

        //    var attribute = attrs[0] as ConfigAttribute;
        //    var instance = Activator.CreateInstance(type) as BaseLoader;
        //    if (instance == null)
        //        continue;

        //    instance.Load(attribute.type);
        //    configDict[attribute.type] = instance;
        //    loaderDict[type] = instance;
        //}

        ////测试配置
        //var configDict = new Dictionary<int, IConfig>();

        //configDict[0] = new MaterialConfig
        //{
        //    id = 0,
        //    materialPath = "Material/Material",
        //    materialType = MaterialType.Atlas,
        //    atlasPath = "Girls"
        //};
        //configDict[1] = new MaterialConfig
        //{
        //    id = 0,
        //    materialPath = "Material/Material 1",
        //    materialType = MaterialType.Sprine,
        //    atlasPath = ""
        //};
        //configDict[2] = new MaterialConfig
        //{
        //    id = 0,
        //    materialPath = "Material/Material 2",
        //    materialType = MaterialType.Sprine,
        //    atlasPath = ""
        //};
        //configDict[3] = new MaterialConfig
        //{
        //    id = 0,
        //    materialPath = "Material/Material 3",
        //    materialType = MaterialType.Sprine,
        //    atlasPath = ""
        //};
        //dict[typeof(MaterialConfig)] = configDict;
        //configDict = new Dictionary<int, IConfig>();
        //configDict[0] = new ActorConfig
        //{
        //    id = 0,
        //    materialId = 0,
        //};
        //configDict[1] = new ActorConfig
        //{
        //    id = 1,
        //    materialId = 1
        //};
        //configDict[2] = new ActorConfig
        //{
        //    id = 2,
        //    materialId = 2
        //};
        //configDict[3] = new ActorConfig
        //{
        //    id = 3,
        //    materialId = 3
        //};
        //dict[typeof(ActorConfig)] = configDict;

    }

    //public T Get<T>(int id) where T : IConfig
    //{
    //    if (dict.TryGetValue(typeof(T), out var configDic))
    //    {
    //        configDic.TryGetValue(id, out var config);
    //        return (T)config;
    //    }
    //    return default;
    //}

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