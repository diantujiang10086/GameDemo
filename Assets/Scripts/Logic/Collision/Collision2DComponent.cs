using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

public class Collision2DComponent : Entity, IAwake<CollisionConfig>
{
    private bool isCollisionDestory;
    private bool isEnableColliderDetection;
    private Unit unit;
    private List<int2> overlayCells;
    public ColliderShape colliderShape { get; private set; }
    public int layer { get; private set; }
    public int colliderLayer { get; private set; }
    public IGeometry geometry { get; private set; }

    public long UnitId => unit.InstanceId;

    public bool IsCollisionDestory => isCollisionDestory;
    public bool IsEnableColliderDetection
    {
        get => isEnableColliderDetection;
        set
        {
            isEnableColliderDetection = value;
            EventSystem.Instance.Publish(new ColliderDetectionCheckUpdate { unitId = unit.InstanceId });
        }
    }

    public void Awake(CollisionConfig collisionConfig)
    {
        isCollisionDestory = collisionConfig.isCollisionDestory;
        isEnableColliderDetection = collisionConfig.isEnableColliderDetection;
        colliderShape = collisionConfig.colliderShape;
        layer = collisionConfig.layer;
        colliderLayer = collisionConfig.colliderLayer;
        if(colliderShape == ColliderShape.Circle)
        {
            geometry = new CircleGeometry(colliderShape, collisionConfig.offset, collisionConfig.radius);
        }
        else if (colliderShape == ColliderShape.Box)
        {
            geometry = new BoxGeometry(colliderShape, collisionConfig.offset, collisionConfig.size);
        }
        unit = GetParent<Unit>();
        overlayCells = new List<int2>(4);

        EventSystem.Instance.Publish(new ColliderRegister { unitId = unit.InstanceId });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<int2> GetOverlayCells()
    {
        foreach (var cell in overlayCells)
        {
            yield return cell;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        overlayCells.Clear();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(int2 cell)
    {
        overlayCells.Add(cell);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UpdateBounds()
    {
        var angle = math.degrees(math.EulerXYZ(unit.rotation)).z;
        geometry.UpdateBoundBox(unit.position.xy, angle);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanCollideWith(Collision2DComponent other)
    {
        return (colliderLayer & other.layer) != 0 && (other.colliderLayer & layer) != 0;
    }

}