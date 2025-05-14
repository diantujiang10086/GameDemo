public class BaseSkill
{
    private bool isDispose = false;
    private Unit caster;
    private SkillConfig config;
    private SkillArguments skillArguments;

    public bool IsDispose => isDispose;

    public void Initialize(Unit caster, SkillConfig config, SkillArguments skillArguments)
    {
        this.caster = caster;
        this.config = config;
        this.skillArguments = skillArguments;
        OnInitialize();
    }

    public void FixedUpdate(float elaspedTime)
    {
        OnFixedUpdate(elaspedTime);
    }

    public void Remove()
    {
        if (isDispose)
            return;

        isDispose = true;
    }

    protected virtual void OnInitialize()
    {

    }

    protected virtual void OnFixedUpdate(float elaspedTime)
    {
     
    }
}
