public class SkillComponent : Entity, IAwake<Unit, SkillConfig>, IFixedUpdate
{
    private Unit caster;
    private SkillConfig skillConfig;
    private float cdEndTime;
    private BaseSkill skill;

    public bool IsCD => TimeSystem.Instance.GameTime < cdEndTime;

    public void Awake(Unit caster,  SkillConfig config)
    {
        this.caster = caster;
        this.skillConfig = config;
    }

    public void Execute(SkillArguments skillArguments)
    {
        skill = SkillTemplateAgentManager.Instance.GetSkillAgnet(skillConfig.templateAgent);
        skill.Initialize(caster, skillConfig, skillArguments);
        cdEndTime = TimeSystem.Instance.GameTime + skillConfig.cdTime;
    }

    public void FixedUpdate(float elaspedTime)
    {
        if (skill == null)
            return;

        if (skill.IsDisposed)
        {
            skill = null;
            return;
        }

        skill.FixedUpdate(elaspedTime);
    }
}
