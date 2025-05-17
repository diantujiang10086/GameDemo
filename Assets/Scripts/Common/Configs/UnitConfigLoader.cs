public partial class UnitConfig
{
    public CollisionConfig collisionConfig;

    public void LoadCollisionConfig()
    {
        collisionConfig = new CollisionConfig
        {
            isCollisionDestory = this.isCollisionDestory,
            isEnableColliderDetection = this.isEnableColliderDetection,
            colliderShape = this.colliderShape,
            layer = this.layer,
            colliderLayer = this.colliderLayer,
            offset = this.offset,
            radius = this.radius,
            size = this.size
        };
    }
}

public partial class UnitConfigLoader 
{

    protected override void OnEndLoad()
    {
        foreach (var item in GetAll())
        {
            (item as UnitConfig).LoadCollisionConfig();
        }
    }
}

