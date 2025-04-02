using Unity.Mathematics;

public class Circle : IShape
{
    public float2 Center;
    public float Radius;
    public Circle() { }
    public Circle(float2 center, float radius) { Center = center; Radius = radius; }

    public void ComputeAABB(ref AABB aabb, Unit unit)
    {
        float2 position = new float2(unit.Position.x, unit.Position.z);
        float worldRadius = Radius * unit.LocalScale;
        float2 worldCenter = position + Center;
        float2 min = worldCenter - worldRadius;
        float2 max = worldCenter + worldRadius;
        aabb.Min = min;
        aabb.Max = max;
    }

    public AABB GetBounds()
    {
        return new AABB(Center - new float2(Radius, Radius), Center + new float2(Radius, Radius));
    }

    public bool TestOverlap(float2 point) => CollisionUtils.PointToCircle(point, this);

    public bool TestOverlap(IShape shape) => shape switch
    {
        Circle circleB => CollisionUtils.CircleToCircle(this, circleB),
        AABB aabb => CollisionUtils.CircleToAABB(this, aabb),
        OBB obb => CollisionUtils.CircleToOBB(this, obb),
        Sector sector => CollisionUtils.CircleToSector(this, sector),
        LineSegment lineSeg => CollisionUtils.CircleToLineSegment(this, lineSeg),
        _ => false
    };

}
