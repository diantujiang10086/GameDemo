public class BulletManager : Singleton<BulletManager>, IAwake
{
    public void Awake()
    {
        
    }

    public bool CreateBullet(int bulletId, BulletArguments bulletArguments)
    {
        var config = ConfigManager.Instance.GetConfig<BulletConfig>(bulletId);
        if (config == null)
            return false;

        var bullet = UnitManager.Instance.CreateBullet(bulletId, bulletArguments.position, bulletArguments.scale, bulletArguments.roation);
        bullet.AddComponent<BulletComponent, Unit>(bulletArguments.owner);

        return true;
    }
}
