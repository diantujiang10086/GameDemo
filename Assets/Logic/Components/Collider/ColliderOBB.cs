using Unity.Mathematics;

public static partial class CollisionUtils
{
    public static bool OBBToOBB(OBB a, OBB b)
    {
        // 分离轴定理实现
        float2[] axes = new float2[4]
        {
            a.GetAxis(0),
            a.GetAxis(1),
            b.GetAxis(0),
            b.GetAxis(1)
        };

        foreach (var axis in axes)
        {
            if (!OverlapOnAxis(a, b, axis))
                return false;
        }

        return true;
    }
    public static bool OBBToSector(OBB obb, Sector sector)
    {
        // 1. 检查OBB是否包含扇形中心
        if (PointToOBB(sector.Center, obb)) return true;

        // 2. 检查扇形边界是否与OBB相交
        float2 leftBound = ColliderHelper.RotateVector(sector.Direction, -sector.Angle / 2) * sector.Radius;
        float2 rightBound = ColliderHelper.RotateVector(sector.Direction, sector.Angle / 2) * sector.Radius;

        LineSegment sectorEdge1 = new LineSegment(sector.Center, sector.Center + leftBound);
        LineSegment sectorEdge2 = new LineSegment(sector.Center, sector.Center + rightBound);

        if (OBBToLineSegment(obb, sectorEdge1) || OBBToLineSegment(obb, sectorEdge2))
            return true;

        // 3. 检查OBB角点是否在扇形内
        float2[] corners = GetOBBCorners(obb);
        foreach (var corner in corners)
        {
            if (PointToSector(corner, sector))
                return true;
        }

        return false;
    }

    // OBB vs LineSegment
    public static bool OBBToLineSegment(OBB obb, LineSegment line)
    {
        // 将线段转换到OBB局部空间
        float2 localStart = line.Start - obb.Center;
        float2 localEnd = line.End - obb.Center;

        // 反旋转
        localStart = ColliderHelper.RotateVector(localStart, -obb.Rotation);
        localEnd = ColliderHelper.RotateVector(localEnd, -obb.Rotation);

        // 现在处理为AABB与线段的碰撞
        AABB localAABB = new AABB(
            -obb.Size / 2,
            obb.Size / 2
        );

        LineSegment localLine = new LineSegment(localStart, localEnd);

        return AABBToLineSegment(localAABB, localLine);
    }
    private static float2[] GetOBBCorners(OBB obb)
    {
        float2 halfSize = obb.Size / 2;
        float2[] corners = new float2[4];

        corners[0] = obb.Center + ColliderHelper.RotateVector(new float2(halfSize.x, halfSize.y), obb.Rotation);
        corners[1] = obb.Center + ColliderHelper.RotateVector(new float2(-halfSize.x, halfSize.y), obb.Rotation);
        corners[2] = obb.Center + ColliderHelper.RotateVector(new float2(-halfSize.x, -halfSize.y), obb.Rotation);
        corners[3] = obb.Center + ColliderHelper.RotateVector(new float2(halfSize.x, -halfSize.y), obb.Rotation);

        return corners;
    }
    private static bool OverlapOnAxis(OBB a, OBB b, float2 axis)
    {
        // 计算两个OBB在轴上的投影范围
        (float minA, float maxA) = GetProjection(a, axis);
        (float minB, float maxB) = GetProjection(b, axis);

        return maxA >= minB && maxB >= minA;
    }
    private static (float min, float max) GetProjection(OBB obb, float2 axis)
    {
        float centerProj = math.dot(obb.Center, axis);
        float2 extents = obb.Size / 2;

        float proj0 = math.dot(obb.GetAxis(0) * extents.x, axis);
        float proj1 = math.dot(obb.GetAxis(1) * extents.y, axis);

        float extent = math.abs(proj0) + math.abs(proj1);
        return (centerProj - extent, centerProj + extent);
    }
}