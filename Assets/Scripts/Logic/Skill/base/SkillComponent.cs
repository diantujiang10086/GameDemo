public class SkillComponent : Entity, IAwake<Unit, SkillConfig>, IFixedUpdate
{
    private Unit caster;
    private SkillConfig skillConfig;
    private float startTime = -999;
    private float cdTime;
    private BaseSkill skill;

    public bool IsCD => TimeSystem.Instance.GameTime - startTime < cdTime;

    public void Awake(Unit caster,  SkillConfig config)
    {
        this.caster = caster;
        this.skillConfig = config;
        cdTime = config.cdTime;
    }

    public void Execute(SkillArguments skillArguments)
    {
        skill = SkillTemplateAgentManager.Instance.GetSkillAgnet(skillConfig.templateAgent);
        skill.Initialize(caster, skillConfig, skillArguments);
        startTime = TimeSystem.Instance.GameTime;
    }

    public void FixedUpdate(float elaspedTime)
    {
        if (skill == null)
            return;

        if (skill.IsDispose)
        {
            skill = null;
            return;
        }

        skill.FixedUpdate(elaspedTime);
    }
}
