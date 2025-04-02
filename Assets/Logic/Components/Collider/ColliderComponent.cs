using Unity.Mathematics;
using UnityEngine;

public class ColliderComponent : Entity, IAwake<IShape>
{
    private Unit unit;
    private IShape shape;
    private float radius;
    private int layer;
    private int colliderLayer;
    private bool isCheckCollider = false;
    private AABB aabb;
    private GridManager.GridKey gridKey;

    public int Layer => layer;
    public float Radius=> radius;
    public bool IsCheckCollider => isCheckCollider;
    public GridManager.GridKey GridKey => gridKey;
    public Unit GetUnit() => unit;
    public AABB GetAABB() => aabb;

    public void SetGrid(GridManager.GridKey key) => gridKey = key;
    public void SetCollideLayer(int layer) => colliderLayer |= 1 << layer;
    public void ClearCollideLayer(int layer) => colliderLayer &= ~(1 << layer);
    public void Awake(IShape shape)
    {
        this.shape = shape;
        var size = shape.GetBounds().Size;
        aabb = new AABB();
        radius = math.max(size.x, size.y);
        unit = GetParent<Unit>();
        GridManager.Instance.Add(unit.InstanceId, unit.Position);
        PhysicsWorld.Instance.AddCollider(this);
        ComputeAABB();
    }

    public void EnableCheckCollider(bool enable = true)
    {
        isCheckCollider = enable;
        if(enable)
        {
            PhysicsWorld.Instance.AddCheckCollider(this);
        }
        else
        {
            PhysicsWorld.Instance.RemoveCheckCollider(this);
        }
    }
    public bool IsCollider(int layer)
    {
        return (this.colliderLayer & layer) != 0;
    }
    public void SetLayer(int layer)
    {
        this.layer = 1 << layer;
    }

    public void ComputeAABB()
    {
        shape.ComputeAABB(ref aabb, unit);
    }

    public bool TestOverlap(ColliderComponent collideComponent)
    {
        return this.shape.TestOverlap(collideComponent.shape); 
    }

    public override void Dispose()
    {
        if (!IsDisposed)
        {
            PhysicsWorld.Instance.RemoveCollider(this);
            PhysicsWorld.Instance.RemoveCheckCollider(this);
            GridManager.Instance.Remove(this.InstanceId, unit.Position);
        }

        base.Dispose();
    }
}