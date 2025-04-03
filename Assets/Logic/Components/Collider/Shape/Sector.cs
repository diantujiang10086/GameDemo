using Unity.Mathematics;
using UnityEngine;

public class Sector : IShape
{
    public float2 Center;
    public float2 Direction;
    public float Radius;
    public float Angle;
    public Sector() { }
    public Sector(float2 center, float2 direction, float radius, float angle)
    {
        Center = center;
        Direction = math.normalize(direction);
        Radius = radius;
        Angle = angle;
    }

    public void ComputeAABB(ref AABB aabb, Unit unit)
    {
        float2 worldPosition = new float2(unit.Position.x, unit.Position.z);
        float2 worldCenter = worldPosition + Center;

        float2 leftDir = ColliderHelper.RotateVector(Direction, -Angle / 2) * Radius;
        float2 rightDir = ColliderHelper.RotateVector(Direction, Angle / 2) * Radius;

        leftDir += Center;
        rightDir += Center;

        float2 worldLeft = worldPosition + leftDir;
        float2 worldRight = worldPosition + rightDir;

        float2 min = math.min(math.min(worldCenter, worldLeft), worldRight);
        float2 max = math.max(math.max(worldCenter, worldLeft), worldRight);

        aabb.Min = min;
        aabb.Max = max;
    }

    public bool TestOverlap(float2 point) => CollisionUtils.PointToSector(point, this);

    public bool TestOverlap(IShape shape) => shape switch
    {
        Circle circleB => CollisionUtils.CircleToSector(circleB, this),
        AABB aabb => CollisionUtils.AABBToSector(aabb, this),
        OBB obb => CollisionUtils.OBBToSector(obb, this),
        Sector sector => CollisionUtils.SectorToSector(this, sector),
        LineSegment lineSeg => CollisionUtils.SectorToLineSegment(this, lineSeg),
        _ => false
    };

}
