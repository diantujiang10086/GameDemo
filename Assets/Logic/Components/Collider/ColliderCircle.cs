using Unity.Mathematics;

public static partial class CollisionUtils
{
    public static bool CircleToCircle(Circle a, Circle b)
    {
        // 计算圆心距离的平方
        float dx = a.Center.x - b.Center.x;
        float dy = a.Center.y - b.Center.y;
        float sqDist = dx * dx + dy * dy;

        // 计算半径和的平方
        float sumRadius = a.Radius + b.Radius;
        float sqRadius = sumRadius * sumRadius;

        // 比较平方距离
        return sqDist <= sqRadius;
    }

    public static bool CircleToAABB(Circle c, AABB aabb)
    {
        float2 closest;
        closest.x = math.max(aabb.Min.x, math.min(c.Center.x, aabb.Max.x));
        closest.y = math.max(aabb.Min.y, math.min(c.Center.y, aabb.Max.y));

        // 计算差值向量并比较平方距离
        float dx = c.Center.x - closest.x;
        float dy = c.Center.y - closest.y;
        return dx * dx + dy * dy <= c.Radius * c.Radius;

    }

    public static bool CircleToOBB(Circle circle, OBB obb)
    {
        // 计算圆心与OBB中心的差值向量
        float2 diff = circle.Center - obb.Center;

        // 获取OBB的两个轴向
        obb.GetAllAxis(out var axisX, out var axisY);

        // 将差值向量投影到OBB的局部坐标系
        float projX = math.dot(diff, axisX);
        float projY = math.dot(diff, axisY);
        float2 extents = obb.Extents;

        // 在OBB范围内找到最近点
        float closestX = math.clamp(projX, -extents.x, extents.x);
        float closestY = math.clamp(projY, -extents.y, extents.y);

        // 计算圆心到最近点的平方距离
        float dx = projX - closestX;
        float dy = projY - closestY;
        float sqDist = dx * dx + dy * dy;

        // 与圆形半径平方比较(避免开平方运算)
        return sqDist <= circle.Radius * circle.Radius;

    }

    public static bool CircleToSector(Circle circle, Sector sector)
    {
        float2 delta = circle.Center - sector.Center;
        float distSqr = math.lengthsq(delta);
        float combinedRadius = sector.Radius + circle.Radius;

        // 如果距离平方大于总半径平方，则肯定不相交
        if (distSqr > combinedRadius * combinedRadius)
            return false;

        float halfAngle = sector.Angle * 0.5f;
        float cosHalfAngle = math.cos(halfAngle);
        float sinHalfAngle = math.sin(halfAngle);

        // 转到扇形局部坐标系：前方向为 x 轴，垂直方向为 y 轴
        float forwardDist = math.dot(delta, sector.Direction); // 圆心在扇形方向上的投影
        float sideDist = math.abs(math.dot(delta, new float2(-sector.Direction.y, sector.Direction.x))); // 侧向投影

        // 如果点在扇形夹角范围内直接返回 true
        if (forwardDist > math.length(delta) * cosHalfAngle)
            return true;

        float2 arcEnd = sector.Radius * new float2(cosHalfAngle, sinHalfAngle);
        float2 localPos = new float2(forwardDist, sideDist);

        // 判断圆心投影点是否在扇形边缘线段附近
        return SegmentPointSqrDistance(float2.zero, arcEnd, localPos) <= circle.Radius * circle.Radius;
    }

    // 计算点到线段的最短距离平方
    static float SegmentPointSqrDistance(float2 start, float2 end, float2 point)
    {
        float2 segment = end - start;
        float t = math.dot(point - start, segment) / math.lengthsq(segment);
        t = math.clamp(t, 0f, 1f);
        float2 projection = start + t * segment;
        return math.lengthsq(point - projection);
    }

    public static bool CircleToLineSegment(Circle circle, LineSegment segment)
    {
        float2 seg = segment.End - segment.Start;
        float2 toCenter = circle.Center - segment.Start;
        float t = math.dot(toCenter, seg) / math.lengthsq(seg);
        t = math.clamp(t, 0f, 1f);
        float2 closest = segment.Start + seg * t;
        return math.lengthsq(circle.Center - closest) <= circle.Radius * circle.Radius;
    }

    private static float2 ClosestPointOnSegment(float2 a, float2 b, float2 p)
    {
        float2 ab = b - a;
        float2 ap = p - a;
        float t = math.dot(ap, ab) / math.lengthsq(ab);
        t = math.clamp(t, 0, 1);
        return a + ab * t;
    }

}
