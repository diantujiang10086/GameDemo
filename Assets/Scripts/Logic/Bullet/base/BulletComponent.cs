public class BulletComponent : Entity, IAwake<BulletConfig, BulletArguments>, IFixedUpdate
{
    private Unit bullet;
    private BulletConfig config;
    private BaseBulletMoveAgent moveAgent;
    public void Awake(BulletConfig bulletConfig, BulletArguments bulletArguments)
    {
        bullet = GetParent<Unit>();
        config = bulletConfig;
        moveAgent = BulletMoveAgentManager.Instance.Create(config.moveAgent);
        moveAgent?.Initialize(bullet, bulletConfig, bulletArguments);
    }

    public void FixedUpdate(float elaspedTime)
    {
        if (moveAgent == null)
            return;
        moveAgent.FixedUpdate(elaspedTime);
    }
}
