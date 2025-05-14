using Unity.Mathematics;

public class UnitManager : Singleton<UnitManager>, IAwake
{
    private Entity units;
    public void Awake()
    {
        units = World.Instance.MainScene.AddChild<Entity>();
    }

    public Unit Create(int configId, float3 position)
    {
        return Create(configId, position, new float3(1, 1, 1), default);
    }

    public Unit Create(int configId, float3 position , float3 scale , float3 rotation )
    {
        var config = ConfigManager.Instance.GetConfig<UnitConfig>(configId);
        if (config == null)
            return default;

        Unit unit = units.AddChild<Unit, UnitConfig>(config);

        unit.position = position + config.position;
        unit.scale = scale + config.scale;
        unit.rotation =quaternion.Euler(rotation + config.rotation);

        if (config.displayId != 0)
        {
            var displayConfig = ConfigManager.Instance.GetConfig<DisplayConfig>(config.displayId);
            if (displayConfig != null)
            {
                unit.AddComponent<DisplayComponent, DisplayConfig>(displayConfig);
            }
        }

        if (config.moveSpeed > 0)
        {
            unit.AddComponent<MoveComponent>();
        }


        EventSystem.Instance.Publish(new UnitCreate { unitId = unit.InstanceId });
        return unit;
    }

    public Unit GetUnit(long instanceId)
    {
        return units.GetChild<Unit>(instanceId);
    }

    public void RemoveUnit(long instanceId)
    {
        units.RemoveChild(instanceId);
    }
}
