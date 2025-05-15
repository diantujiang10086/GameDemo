using System;
using System.Collections.Generic;

public class BulletMoveAgentManager : Singleton<BulletMoveAgentManager> ,IAwake
{
    private Dictionary<string, Type> agentDict = new Dictionary<string, Type>();
    public void Awake()
    {
        agentDict[string.Empty] = typeof(DefaultBulletMoveAgent);
        foreach (var item in AssemblyHelper.GetTypes<BulletMoveAttribute>())
        {
            var attribute = item.attribute;
            agentDict[item.type.Name] = item.type;
        }
    }


    public BaseBulletMoveAgent Create(string typeName)
    {
        if (!agentDict.TryGetValue(typeName, out var type))
            return default;
        var moveAgent = Activator.CreateInstance(type) as BaseBulletMoveAgent;
        return moveAgent;
    }
}
