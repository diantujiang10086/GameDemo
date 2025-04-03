using Unity.Mathematics;
using UnityEngine;

public class AABB : IShape
{
    public float2 Min;
    public float2 Max;
    public float2 Center => (Min + Max) / 2;
    public float2 Size => Max - Min;

    public AABB() { }
    public AABB(float2 min, float2 max) { Min = min; Max = max; }

    public bool TestOverlap(float2 point) => CollisionUtils.PointToAABB(point, this);

    public bool TestOverlap(IShape shape)=> shape switch
    {
        Circle circleB => CollisionUtils.CircleToAABB(circleB, this),
        AABB aabb => CollisionUtils.AABBToAABB(aabb, this),
        OBB obb => CollisionUtils.AABBToOBB(this, obb),
        Sector sector => CollisionUtils.AABBToSector(this, sector),
        LineSegment lineSeg => CollisionUtils.AABBToLineSegment(this, lineSeg),
        _ => false
    };


    public void ComputeAABB(ref AABB aabb, Unit unit)
    {
        float2 worldPosition = new float2(unit.Position.x, unit.Position.z);
        float2 worldMin = worldPosition + Min;
        float2 worldMax = worldPosition + Max;
        
        var min = math.min(worldMin, worldMax);
        var max = math.max(worldMin, worldMax);
        aabb.Min = new float2(min.x, min.y);
        aabb.Max = new float2(min.x, min.y);

    }
}
