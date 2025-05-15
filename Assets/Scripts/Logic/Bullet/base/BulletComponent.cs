public class BulletComponent : Entity, IAwake<Unit, BulletConfig, BulletArguments>, IFixedUpdate
{
    private Unit bullet;
    private Unit owner;
    private BulletConfig config;
    private BaseBulletMoveAgent moveAgent;
    public void Awake(Unit a, BulletConfig bulletConfig, BulletArguments bulletArguments)
    {
        bullet = GetParent<Unit>();
        owner = a;
        config = bulletConfig;
        var moveAgent = BulletMoveAgentManager.Instance.Create(config.moveAgent);
        moveAgent?.Initialize(bullet, bulletConfig, bulletArguments);
    }

    public void FixedUpdate(float elaspedTime)
    {
        if (moveAgent == null)
            return;
        moveAgent.FixedUpdate(elaspedTime);
    }
}
