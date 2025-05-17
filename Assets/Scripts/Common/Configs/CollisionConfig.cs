using Unity.Mathematics;

public enum ColliderShape : int
{
    None = 0,
    Box = 1,
    Circle = 2,
}

public class CollisionConfig : IConfig
{
    public int id;
    public bool isCollisionDestory;
    public bool isEnableColliderDetection;
    public ColliderShape colliderShape;
    public int layer;
    public int colliderLayer;
    public float2 offset;
    public float radius;
    public float2 size;
}

[Config(typeof(CollisionConfig))]
public partial class CollisionConfigLoader : BaseLoader
{

}
