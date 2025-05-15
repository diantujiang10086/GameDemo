// The configuration generation file cannot be modified!

using Unity.Mathematics;

public partial class BuffConfig : IConfig
{
	public int id;
	public string templateAgent;
	public float duration;
	public float tickInterval;
	public int buffType;
	public bool canStack;
	public bool canRefresh;
	public int maxStacks;
	public string args0;
	public string args1;
	public string args2;
	public string args3;
	public string args4;
	public string args5;
	public string args6;
	public string args7;
	public string args8;
	public string args9;
}

[Config(typeof(BuffConfig))]
public partial class BuffConfigLoader : BaseLoader
{

}
