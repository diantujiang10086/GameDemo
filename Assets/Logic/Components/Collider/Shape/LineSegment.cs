using Unity.Mathematics;
using UnityEngine;

public class LineSegment : IShape
{
    public float2 Start, End;
    public LineSegment() { }
    public LineSegment(float2 start, float2 end) { Start = start; End = end; }

    public void ComputeAABB(ref AABB aabb, Unit unit)
    {
        float2 worldPosition = new float2(unit.Position.x, unit.Position.z);
        float2 worldStart3D = worldPosition + Start;
        float2 worldEnd3D = worldPosition + End;

        // 转换为 float2
        float2 worldStart = new float2(worldStart3D.x, worldStart3D.y);
        float2 worldEnd = new float2(worldEnd3D.x, worldEnd3D.y);

        // 线段的 AABB
        float2 min = math.min(worldStart, worldEnd);
        float2 max = math.max(worldStart, worldEnd);

        // 额外考虑 0.01 的宽度
        float2 halfWidth = new float2(0.005f, 0.005f); // 线宽 0.01f，半宽 0.005f
        min -= halfWidth;
        max += halfWidth;

        aabb.Min = min;
        aabb.Max = max;
    }

    public bool TestOverlap(float2 point) => CollisionUtils.PointToLineSegment(point, this);

    public bool TestOverlap(IShape shape) =>
        shape switch
        {
            Circle circleB => CollisionUtils.CircleToLineSegment(circleB, this),
            AABB aabb => CollisionUtils.AABBToLineSegment(aabb, this),
            OBB obb => CollisionUtils.OBBToLineSegment(obb, this),
            Sector sector => CollisionUtils.SectorToLineSegment(sector, this),
            LineSegment lineSeg => CollisionUtils.LineSegmentToLineSegment(this, lineSeg),
            _ => false
        };

}
