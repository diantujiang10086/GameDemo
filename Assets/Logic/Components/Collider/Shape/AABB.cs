using Unity.Mathematics;
using UnityEngine;

public class AABB : IShape
{
    public float2 Min;
    public float2 Max;
    public float2 Center
    {
        get => (Min + Max) * 0.5f;
        set
        {
            float2 halfSize = Size * 0.5f;
            Min = value - halfSize;
            Max = value + halfSize;
        }
    }
    public float2 Size
    {
        get => Max - Min;
        set
        {
            float2 halfSize = value * 0.5f;
            float2 center = Center;
            Min = center - halfSize;
            Max = center + halfSize;
        }
    }

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
        Min = new float2(min.x, min.y);
        Max = new float2(min.x, min.y);

    }

    public void ComputeCorners(out float2[] corners)
    {
        corners = new float2[]
        {
            new Vector2(Min.x, Min.y),
            new Vector2(Max.x, Min.y),
            new Vector2(Max.x, Max.y),
            new Vector2(Min.x, Max.y)
        };
    }
}
