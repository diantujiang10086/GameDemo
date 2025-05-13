using System;
using System.Collections.Generic;
using System.Reflection;

public class BaseLoader
{
    protected readonly Dictionary<int, IConfig> dict = new Dictionary<int, IConfig>();

    public IConfig Get(int id)
    {
        dict.TryGetValue(id, out var value);
        return value;
    }

    public IEnumerable<IConfig> GetAll()
    {
        foreach (var item in dict.Values)
        {
            yield return item;
        }
    }

    public void Load(Type type)
    {
        OnBeginLoad();

        var text = ResourceManager.LoadText($"config/{type.Name}");

        if (string.IsNullOrEmpty(text))
        {
            throw new Exception($"Load {type} fail!");
        }

        var lines = text.Split('\n');
        if (lines.Length <= 1)
        {
            Log.Warning($"{type} no content");
            return;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Replace("\r", string.Empty);
        }

        var heads = lines[0].Split('\t');
        // 第一行作为字段名称
        FieldInfo idFieldInfo = default;
        FieldInfo[] fieldInfos = new FieldInfo[heads.Length];
        for (int i = 0; i < heads.Length; i++)
        {
            fieldInfos[i] = type.GetField(heads[i], BindingFlags.Public | BindingFlags.Instance);
            if (fieldInfos[i] == null)
            {
                throw new Exception($"{heads[i]} field of {type} could not be found!");
            }
            if (heads[i] == "id")
            {
                idFieldInfo = fieldInfos[i];
            }
        }
        // 其他行是数据，以\t分割列
        for (int i = 1; i < lines.Length; i++)
        {
            try
            {
                var column = lines[i].Split('\t');
                if (column.Length != heads.Length)
                {
                    Log.Error($" The {type} column {i} does not correspond to the number of table heads");
                    continue;
                }

                var instance = Activator.CreateInstance(type);

                for (int j = 0; j < heads.Length; j++)
                {
                    fieldInfos[j].SetValue(instance, ConvertHelper.ChangeType(column[j], fieldInfos[j].FieldType));
                }

                dict[(int)idFieldInfo.GetValue(instance)] = instance as IConfig;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }


        OnEndLoad();
    }
    protected virtual void OnBeginLoad() { }
    protected virtual void OnEndLoad() { }
}
