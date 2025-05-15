public class BaseSkill
{
    private bool isDisposed = false;
    protected Unit caster;
    protected SkillConfig config;
    protected SkillArguments skillArguments;

    public bool IsDisposed => isDisposed;

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
        if (isDisposed)
            return;

        isDisposed = true;
    }

    protected virtual void OnInitialize()
    {

    }

    protected virtual void OnFixedUpdate(float elaspedTime)
    {
     
    }
}
