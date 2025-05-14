// The configuration generation file cannot be modified!

using Unity.Mathematics;

public partial class SkillConfig : IConfig
{
	public int id;
	public string templateAgent;
	public float cdTime;
	public string arg0;
	public string arg1;
	public string arg2;
	public string arg3;
	public string arg4;
	public string arg5;
	public string arg6;
	public string arg7;
	public string arg8;
	public string arg9;
	public string arg10;
}

[Config(typeof(SkillConfig))]
public partial class SkillConfigLoader : BaseLoader
{

}
