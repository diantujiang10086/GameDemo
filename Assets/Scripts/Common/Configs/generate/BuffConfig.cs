// The configuration generation file cannot be modified!

using Unity.Mathematics;

public partial class BuffConfig : IConfig
{
	public int id;
}

[Config(typeof(BuffConfig))]
public partial class BuffConfigLoader : BaseLoader
{

}
