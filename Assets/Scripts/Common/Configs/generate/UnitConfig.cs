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
}

[Config(typeof(UnitConfig))]
public partial class UnitConfigLoader : BaseLoader
{

}
