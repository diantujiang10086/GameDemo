// The configuration generation file cannot be modified!

using Unity.Mathematics;

public partial class BulletConfig : IConfig
{
	public int id;
	public float3 position;
	public float3 scale;
	public float3 rotation;
	public int displayId;
	public string moveAgent;
}

[Config(typeof(BulletConfig))]
public partial class BulletConfigLoader : BaseLoader
{

}
