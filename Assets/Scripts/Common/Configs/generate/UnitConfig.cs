// The configuration generation file cannot be modified!

using Unity.Mathematics;

public partial class UnitConfig : IConfig
{
	public int id;
	public float3 position;
	public float3 scale;
	public float3 rotation;
	public int displayId;
	public float moveSpeed;
	public float rotationSpeed;
	public int[] skills;
	public int[] buffs;
	public bool isAddCollider;
	public bool isEnableColliderDetection;
	public bool isCollisionDestory;
	public int layer;
	public int colliderLayer;
	public ColliderShape colliderShape;
	public float2 offset;
	public float radius;
	public float2 size;
}

[Config(typeof(UnitConfig))]
public partial class UnitConfigLoader : BaseLoader
{

}
