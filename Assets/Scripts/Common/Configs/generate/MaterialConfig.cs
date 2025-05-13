// The configuration generation file cannot be modified!

public partial class MaterialConfig : IConfig
{
	public int id;
	public string materialPath;
	public MaterialType materialType;
	public string atlasPath;
}

[Config(typeof(MaterialConfig))]
public partial class MaterialConfigLoader : BaseLoader
{

}
