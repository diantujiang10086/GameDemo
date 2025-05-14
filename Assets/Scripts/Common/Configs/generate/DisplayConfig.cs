// The configuration generation file cannot be modified!

using Unity.Mathematics;

public partial class DisplayConfig : IConfig
{
	public int id;
	public string materialPath;
	public MaterialType materialType;
	public string atlasPath;
	public int animationIndex;
}

[Config(typeof(DisplayConfig))]
public partial class DisplayConfigLoader : BaseLoader
{

}
