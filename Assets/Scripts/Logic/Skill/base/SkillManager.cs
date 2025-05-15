using System.Collections.Generic;

public class SkillManager : Entity, IAwake
{
    private Unit caster;
    private Dictionary<int, SkillComponent> skills = new Dictionary<int, SkillComponent>();

    public void Awake()
    {
        caster = GetParent<Unit>();
    }

    public void AddSkill(int skillId)
    {
        if(skills.ContainsKey(skillId))
            return;

        var config = ConfigManager.Instance.GetConfig<SkillConfig>(skillId);
        if (config == null)
            return;
        
        SkillComponent skill = AddChild<SkillComponent, Unit, SkillConfig>(caster, config);
        skills[skillId] = skill;
    }

    public void RemoveSkill(int skillId)
    {
        if(skills.TryGetValue(skillId, out var skill))
        {
            skill.Dispose();
            skills.Remove(skillId);
        }
    }

    public bool ExecuteSkill(int skillId, SkillArguments skillArguments = default)
    {
        if (!skills.TryGetValue(skillId, out var skill))
            return false;

        if (skill.IsCD)
            return false;

        skill.Execute(skillArguments);

        return true;
    }

}