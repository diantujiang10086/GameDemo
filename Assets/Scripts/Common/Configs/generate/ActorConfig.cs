// The configuration generation file cannot be modified!

public partial class ActorConfig : IConfig
{
	public int id;
	public int materialId;
}

[Config(typeof(ActorConfig))]
public partial class ActorConfigLoader : BaseLoader
{

}
