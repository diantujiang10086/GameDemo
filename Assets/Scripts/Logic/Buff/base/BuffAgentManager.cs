using System;
using System.Collections.Generic;

public class BuffAgentManager : Singleton<BuffAgentManager>, IAwake
{
    private Dictionary<string, Type> agentDict = new Dictionary<string, Type>();
    public void Awake()
    {
        agentDict[string.Empty] = typeof(DefaultBuff);
        foreach (var item in AssemblyHelper.GetTypes<BuffAttribute>())
        {
            var attribute = item.attribute;
            agentDict[item.type.Name] = item.type;
        }
    }

    public BaseBuff GetBuff(string typeName)
    {
        if (!agentDict.TryGetValue(typeName, out var type))
            return default;
        return Activator.CreateInstance(type) as BaseBuff;
    }
}
