public class UnitManager : Singleton<UnitManager>, IAwake
{
    private Entity units;
    public void Awake()
    {
        units = World.Instance.MainScene.AddChild<Entity>();
    }

    public Unit Create(int configId)
    {
        var config = ConfigManager.Instance.GetConfig<UnitConfig>(configId);
        if (config == null)
            return default;

        Unit unit = units.AddChild<Unit, UnitConfig>(config);
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
