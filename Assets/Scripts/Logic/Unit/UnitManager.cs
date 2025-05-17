using Unity.Mathematics;

public class UnitManager : Singleton<UnitManager>, IAwake
{
    private Entity units;
    public void Awake()
    {
        units = World.Instance.MainScene.AddChild<Entity>();
    }

    public Unit CreateActor(int configId, float3 position)
    {
        return CreateActor(configId, position, new float3(1, 1, 1), default);
    }

    public Unit CreateActor(int configId, float3 position, float3 scale, float3 rotation)
    {
        var config = ConfigManager.Instance.GetConfig<UnitConfig>(configId);
        if (config == null)
            return default;

        var unit = CreateUnit(config, config.displayId, config.position + position, config.scale + scale, config.rotation + rotation);
        AddCollider(unit, config);
        if (config.moveSpeed > 0)
        {
            unit.AddComponent<MoveComponent,float,float>(config.moveSpeed, config.rotationSpeed);
        }

        var skillManager = unit.AddComponent<SkillManager>();
        if(config.skills != null)
        {
            foreach (var skillId in config.skills)
            {
                skillManager.AddSkill(skillId);
            }
        }

        var buffManager = unit.AddComponent<BuffManager>();
        if(config.buffs != null)
        {
            foreach (var buffId in config.buffs)
            {
                buffManager.AddBuff(buffId);
            }
        }

        EventSystem.Instance.Publish(new UnitCreate { unitId = unit.InstanceId });
        return unit;
    }

    public Unit CreateBullet(int bulletId, float3 position, float3 scale, float3 rotation)
    {
        var config = ConfigManager.Instance.GetConfig<BulletConfig>(bulletId);
        if (config == null)
            return default;

        var unit = CreateUnit(config, config.displayId, config.position + position, config.scale+ scale, config.rotation + rotation);
        AddCollider(unit, config);
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

    private Unit CreateUnit<T>(T config, int displayId, float3 position, float3 scale, float3 rotation) where T: IConfig
    {
        Unit unit = units.AddChild<Unit, T>(config);
        unit.position = position;
        unit.scale = scale;
        unit.rotation = quaternion.Euler(math.radians(rotation));

        if (displayId != 0)
        {
            var displayConfig = ConfigManager.Instance.GetConfig<DisplayConfig>(displayId);
            if (displayConfig != null)
            {
                unit.AddComponent<DisplayComponent, DisplayConfig>(displayConfig);
            }
        }
        return unit;
    }

    private void AddCollider(Unit unit, UnitConfig unitConfig)
    {
        var config = new CollisionConfig
        {
            isCollisionDestory = unitConfig.isCollisionDestory,
            isEnableColliderDetection = unitConfig.isEnableColliderDetection,
            colliderShape = unitConfig.colliderShape,
            layer = unitConfig.layer,
            colliderLayer = unitConfig.colliderLayer,
            offset = unitConfig.offset,
            radius = unitConfig.radius,
            size = unitConfig.size
        };
        unit.AddComponent<Collision2DComponent, CollisionConfig>(config);
    }

    private void AddCollider(Unit unit, BulletConfig unitConfig)
    {
        var config = new CollisionConfig
        {
            isCollisionDestory = unitConfig.isCollisionDestory,
            isEnableColliderDetection = unitConfig.isEnableColliderDetection,
            colliderShape = unitConfig.colliderShape,
            layer = unitConfig.layer,
            colliderLayer = unitConfig.colliderLayer,
            offset = unitConfig.offset,
            radius = unitConfig.radius,
            size = unitConfig.size
        };
        unit.AddComponent<Collision2DComponent, CollisionConfig>(config);
    }
}
