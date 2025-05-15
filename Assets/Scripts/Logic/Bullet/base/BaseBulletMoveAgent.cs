public class BaseBulletMoveAgent
{
    protected Unit unit;
    protected BulletConfig config;
    protected BulletArguments bulletArguments;
    public virtual void Initialize(Unit unit, BulletConfig bulletConfig, BulletArguments bulletArguments)
    {
        this.unit = unit;
        this.config = bulletConfig;
        this.bulletArguments = bulletArguments;
        OnInitialize();
    }

    public void FixedUpdate(float elaspedTime)
    {
        OnFixedUpdate(elaspedTime);
    }

    protected virtual void OnInitialize()
    {
        
    }
    protected virtual void OnFixedUpdate(float elaspedTime)
    {
    }
}
