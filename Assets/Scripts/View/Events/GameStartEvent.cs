using Unity.Mathematics;

[Event]
public class GameStartEvent : AEvent<GameStart>
{
    protected override void Run(GameStart a)
    {
        //≤‚ ‘
        CreateUnit(1, new float3(0, -5, 0), 1, 2);
        CreateUnit(3, new float3(0, 5, 0), 3, 4);
    }

    private void CreateUnit(int configId, float3 pos, int layer, int colliderLayer)
    {
        var unit = UnitManager.Instance.CreateActor(configId,pos);
        var config = new CollisionConfig
        {
            isCollisionDestory = false,
            isEnableColliderDetection = true,
            colliderShape = ColliderShape.Circle,
            layer = layer,
            colliderLayer = colliderLayer,
            offset = float2.zero,
            radius = 0.6f,
            size = float2.zero
        };
        unit.AddComponent<Collision2DComponent, CollisionConfig>(config);
    }
}