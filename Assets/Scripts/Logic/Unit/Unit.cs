using Unity.Mathematics;

public class Unit : Entity , IAwake<UnitConfig>
{
    private UnitConfig config;
    private float3 m_position;
    private float3 m_scale = new float3(1, 1, 1);
    private quaternion m_rotation;
    public float3 position
    {
        get => m_position;
        set
        {
            EventSystem.Instance.Publish(new UnitChangedPosition { unitId = InstanceId, oldValue = m_position, newValue = value });
            m_position = value;
        }
    }

    public float3 scale
    {
        get => m_scale;
        set
        {
            EventSystem.Instance.Publish(new UnitChangedScale { unitId = InstanceId, oldValue = m_scale, newValue = value });
            m_scale = value;
        }
    }

    public quaternion rotation
    {
        get => m_rotation;
        set
        {
            EventSystem.Instance.Publish(new UnitChangedRotation { unitId = InstanceId, oldValue = m_rotation, newValue = value });
            m_rotation = value;
        }
    }

    public UnitConfig Config
    {
        get => config;
    }

    public void MoveTo(float3 value)
    {
        m_position = value;
        EventSystem.Instance.Publish(new UnitMovePosition { unitId = InstanceId, value = value });
    }

    public void RotationTo(quaternion value)
    {
        m_rotation = value;
        EventSystem.Instance.Publish(new UnitLookAt { unitId = InstanceId, value = value });
    }

    public void Awake(UnitConfig config)
    {
        this.config = config;
        position = config.position;
        scale = config.scale;
        rotation = quaternion.Euler(config.rotation);

        if(config.displayId != 0)
        {
            var displayConfig = ConfigManager.Instance.GetConfig<DisplayConfig>(config.displayId);
            if(displayConfig != null)
            {
                AddComponent<DisplayComponent, DisplayConfig>(displayConfig);
            }
        }

        if(config.moveSpeed > 0)
        {
            AddComponent<MoveComponent>();
        }
    }

    protected override void OnDestory()
    {
        EventSystem.Instance.Publish(new UnitDestory { unitId = InstanceId });    
    }
}
