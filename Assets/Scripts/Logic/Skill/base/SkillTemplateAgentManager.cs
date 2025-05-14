
using System;
using System.Collections.Generic;

public class SkillTemplateAgentManager : Singleton<SkillTemplateAgentManager>, IAwake
{
    private Dictionary<string, Type> skillTemplateAgentDict = new Dictionary<string, Type>();
    public void Awake()
    {
        skillTemplateAgentDict[string.Empty] = typeof(DefaultSkill);
        foreach (var item in AssemblyHelper.GetTypes<SkillAttribute>())
        {
            var attribute = item.attribute;
            skillTemplateAgentDict[item.type.Name] = item.type;
        }
    }

    public BaseSkill GetSkillAgnet(string templateAgent)
    {
        if (!skillTemplateAgentDict.TryGetValue(templateAgent, out var type))
            return default;
        var skill = Activator.CreateInstance(type) as BaseSkill;
        return skill;
    }
}
