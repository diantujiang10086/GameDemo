using Unity.Mathematics;
using UnityEngine;

public class OBB : IShape
{
    public float2 Center;
    public float2 Size;
    public float Rotation;
    public OBB() { }
    public OBB(float2 center, float2 size, float rotation)
    {
        Center = center;
        Size = size;
        Rotation = rotation;
    }

    public void ComputeAABB(ref AABB aabb, Unit unit)
    {
        float2 halfSize = Size / 2;

        // 局部坐标系的四个角
        float2[] localCorners = new float2[4]
        {
        new float2(-halfSize.x, -halfSize.y),
        new float2(halfSize.x, -halfSize.y),
        new float2(-halfSize.x, halfSize.y),
        new float2(halfSize.x, halfSize.y)
        };

        float2 worldPosition = new float2(unit.Position.x, unit.Position.z);
        float2 worldCenter = worldPosition + new float2(Center.x, Center.y);

        float2 min = new float2(float.MaxValue, float.MaxValue);
        float2 max = new float2(float.MinValue, float.MinValue);

        foreach (var corner in localCorners)
        {
            float2 rotatedCorner = ColliderHelper.RotateVector(corner, Rotation);
            var center = Center + rotatedCorner;
            float2 worldCorner = worldPosition + new float2(center.x, center.y);
            min = math.min(min, worldCorner);
            max = math.max(max, worldCorner);
        }
        aabb.Min = min;
        aabb.Max = max;
    }

    public float2 GetAxis(int index)
    {
        float2 axis = index == 0 ? new float2(1, 0) : new float2(0, 1);
        return ColliderHelper.RotateVector(axis, Rotation);
    }

    public float GetHalfLength(int index) => Size[index] / 2;

    public bool TestOverlap(float2 point) => CollisionUtils.PointToOBB(point, this);

    public bool TestOverlap(IShape shape) => shape switch
    {
        Circle circleB => CollisionUtils.CircleToOBB(circleB, this),
        AABB aabb => CollisionUtils.AABBToOBB(aabb, this),
        OBB obb => CollisionUtils.OBBToOBB(this, obb),
        Sector sector => CollisionUtils.OBBToSector(this, sector),
        LineSegment lineSeg => CollisionUtils.OBBToLineSegment(this, lineSeg),
        _ => false
    };

}
