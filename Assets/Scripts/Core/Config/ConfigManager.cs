using System;
using System.Collections.Generic;

public class ConfigManager : Singleton<ConfigManager>
{
    private Dictionary<Type, Dictionary<int, IConfig>> dict = new Dictionary<Type, Dictionary<int, IConfig>>();
    public ConfigManager()
    {
        //测试配置
        var configDict = new Dictionary<int, IConfig>();

        configDict[0] = new MaterialConfig
        {
            id = 0,
            materialPath = "Material/Material",
            materialType = MaterialType.Atlas,
            atlasPath = "Girls"
        };
        configDict[1] = new MaterialConfig
        {
            id = 0,
            materialPath = "Material/Material 1",
            materialType = MaterialType.Sprine,
            atlasPath = ""
        };
        configDict[2] = new MaterialConfig
        {
            id = 0,
            materialPath = "Material/Material 2",
            materialType = MaterialType.Sprine,
            atlasPath = ""
        };
        configDict[3] = new MaterialConfig
        {
            id = 0,
            materialPath = "Material/Material 3",
            materialType = MaterialType.Sprine,
            atlasPath = ""
        };
        dict[typeof(MaterialConfig)] = configDict;
        configDict = new Dictionary<int, IConfig>();
        configDict[0] = new ActorConfig
        {
            id = 0,
            materialId = 0,
        };
        configDict[1] = new ActorConfig
        {
            id = 1,
            materialId = 1
        };
        configDict[2] = new ActorConfig
        {
            id = 2,
            materialId = 2
        };
        configDict[3] = new ActorConfig
        {
            id = 3,
            materialId = 3
        };
        dict[typeof(ActorConfig)] = configDict;

    }

    public T Get<T>(int id) where T : IConfig
    {
        if (dict.TryGetValue(typeof(T), out var configDic))
        {
            configDic.TryGetValue(id, out var config);
            return (T)config;
        }
        return default;
    }
}