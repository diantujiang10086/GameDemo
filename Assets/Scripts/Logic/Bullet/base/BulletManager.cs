public class BulletManager : Singleton<BulletManager>, IAwake
{
    public void Awake()
    {
        
    }

    public Unit CreateBullet(int bulletId, BulletArguments bulletArguments)
    {
        var config = ConfigManager.Instance.GetConfig<BulletConfig>(bulletId);
        if (config == null)
            return default;

        var bullet = UnitManager.Instance.CreateBullet(bulletId, bulletArguments.position, bulletArguments.scale, bulletArguments.roation);
        bullet.AddComponent<BulletComponent, BulletConfig, BulletArguments>(config, bulletArguments);
        bullet.AddComponent<RegionDestoryComponent>();
        return bullet;
    }
}
