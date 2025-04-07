using Unity.Mathematics;

public static partial class CollisionUtils
{
    public static bool OBBToOBB(OBB a, OBB b)
    {
        a.GetAllAxis(out var aAxisX, out var aAxisY);
        b.GetAllAxis(out var bAxisX, out var bAxisY);

        // 预计算扩展轴
        var aExtents = a.Extents;
        float2 aAxisXExt = aAxisX * aExtents.x;
        float2 aAxisYExt = aAxisY * aExtents.y;

        var bExtents = b.Extents;
        float2 bAxisXExt = bAxisX * bExtents.x;
        float2 bAxisYExt = bAxisY * bExtents.y;

        aAxisX = math.normalize(aAxisX);
        aAxisY = math.normalize(aAxisY);
        bAxisX = math.normalize(bAxisX);
        bAxisY = math.normalize(bAxisY);

        // 检查所有分离轴
        return CheckAxis(a, b, aAxisX, aAxisXExt, aAxisYExt, bAxisXExt, bAxisYExt) &&
               CheckAxis(a, b, aAxisY, aAxisXExt, aAxisYExt, bAxisXExt, bAxisYExt) &&
               CheckAxis(a, b, bAxisX, aAxisXExt, aAxisYExt, bAxisXExt, bAxisYExt) &&
               CheckAxis(a, b, bAxisY, aAxisXExt, aAxisYExt, bAxisXExt, bAxisYExt);

    }
    private static bool CheckAxis(
        OBB a, OBB b, float2 axis,
        float2 aAxisXExt, float2 aAxisYExt,
        float2 bAxisXExt, float2 bAxisYExt)
    {
        // 计算A的投影
        float aCenterProj = math.dot(a.Center, axis);
        float aProj0 = math.dot(aAxisXExt, axis);
        float aProj1 = math.dot(aAxisYExt, axis);
        float aExtent = math.abs(aProj0) + math.abs(aProj1);

        // 计算B的投影
        float bCenterProj = math.dot(b.Center, axis);
        float bProj0 = math.dot(bAxisXExt, axis);
        float bProj1 = math.dot(bAxisYExt, axis);
        float bExtent = math.abs(bProj0) + math.abs(bProj1);

        // 检查重叠
        return (aCenterProj + aExtent) >= (bCenterProj - bExtent) &&
               (bCenterProj + bExtent) >= (aCenterProj - aExtent);
    }

    public static bool OBBToSector(OBB obb, Sector sector)
    {
        if (PointToOBB(sector.Center, obb))
            return true;

        obb.GetAllAxis(out var axisX, out var axisY);
        obb.ComputeCorners(obb.Center, out var corners, out var min, out var max);
        float2 dir = math.normalize(sector.Direction);
        float cosHalfAngle = math.cos(sector.Angle * 0.5f);
        float radiusSqr = sector.Radius * sector.Radius;

        // 1. 顶点在扇形内
        foreach (var point in corners)
        {
            float2 toPoint = point - sector.Center;
            if (math.lengthsq(toPoint) > radiusSqr) continue;

            float dot = math.dot(math.normalize(toPoint), dir);
            if (dot >= cosHalfAngle)
                return true;
        }

        for (int i = 0; i < 4; i++)
        {
            float2 p0 = corners[i];
            float2 p1 = corners[(i + 1) % 4];

            if (SegmentIntersectsArc(p0, p1, sector.Center, sector.Radius, dir, sector.Angle))
                return true;
        }
        float2 leftDir = math.normalize(SectorRotate(sector.Direction, -sector.Angle * 0.5f));
        float2 rightDir = math.normalize(SectorRotate(sector.Direction, +sector.Angle * 0.5f));
        float2 leftEnd = sector.Center + leftDir * sector.Radius;
        float2 rightEnd = sector.Center + rightDir * sector.Radius;

        for (int i = 0; i < 4; i++)
        {
            float2 p0 = corners[i];
            float2 p1 = corners[(i + 1) % 4];

            if (SegmentIntersect(sector.Center, leftEnd, p0, p1) ||
                SegmentIntersect(sector.Center, rightEnd, p0, p1))
                return true;
        }

        return false;

    }
    public static bool SegmentIntersectsArc(
    float2 p0, float2 p1,
    float2 center, float radius,
    float2 direction, float angle)
    {
        float2 d = p1 - p0;
        float2 f = p0 - center;

        float a = math.dot(d, d);
        float b = 2 * math.dot(f, d);
        float c = math.dot(f, f) - radius * radius;

        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
            return false;

        discriminant = math.sqrt(discriminant);

        float t1 = (-b - discriminant) / (2 * a);
        float t2 = (-b + discriminant) / (2 * a);

        for (int i = 0; i < 2; i++)
        {
            float t = (i == 0) ? t1 : t2;

            if (t >= 0f && t <= 1f)
            {
                float2 hit = p0 + t * d;
                float2 toHit = math.normalize(hit - center);
                float dot = math.dot(toHit, math.normalize(direction));
                float actualAngle = math.acos(math.clamp(dot, -1f, 1f));
                if (actualAngle <= angle * 0.5f)
                    return true;
            }
        }

        return false;
    }
    static bool SegmentIntersect(float2 a1, float2 a2, float2 b1, float2 b2)
    {
        float2 d1 = a2 - a1;
        float2 d2 = b2 - b1;
        float2 delta = b1 - a1;

        float cross = d1.x * d2.y - d1.y * d2.x;
        if (math.abs(cross) < 1e-6f)
            return false; // 平行

        float t = (delta.x * d2.y - delta.y * d2.x) / cross;
        float u = (delta.x * d1.y - delta.y * d1.x) / cross;

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }
    static float2 SectorRotate(float2 dir, float angleRad)
    {
        float sin = math.sin(angleRad);
        float cos = math.cos(angleRad);
        return new float2(
            dir.x * cos - dir.y * sin,
            dir.x * sin + dir.y * cos
        );
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
    
}