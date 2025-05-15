public class DisplayComponent : Entity, IAwake<DisplayConfig>
{
    private DisplayConfig config;
    private int m_animationIndex;

    public int animationIndex
    {
        get => m_animationIndex;
    }

    public int DisplayId => config.id;

    public void SetAnimationIndex(int value)
    {
        m_animationIndex = value;
        EventSystem.Instance.Publish(new UnitAnimationIndex { unitId = InstanceId, value = value });
    }

    public void Awake(DisplayConfig config)
    {
        this.config = config;
        if(config.materialType == MaterialType.Atlas)
        {
            m_animationIndex = config.animationIndex;
        }
    }
}