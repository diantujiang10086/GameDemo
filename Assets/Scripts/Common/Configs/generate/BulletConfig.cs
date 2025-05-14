// The configuration generation file cannot be modified!

using Unity.Mathematics;

public partial class BulletConfig : IConfig
{
	public int id;
}

[Config(typeof(BulletConfig))]
public partial class BulletConfigLoader : BaseLoader
{

}
