public enum MaterialType
{
    Sprine,
    Atlas,
    Spine
}

public class MaterialConfig : IConfig
{
    public int id { get; set; }
    public string materialPath;
    public string atlasPath;
    public MaterialType materialType = MaterialType.Sprine;
}
