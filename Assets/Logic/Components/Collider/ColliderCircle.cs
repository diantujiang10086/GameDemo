using Unity.Mathematics;

public static partial class CollisionUtils
{
    public static bool CircleToCircle(Circle a, Circle b)
    {
        float2 delta = a.Center - b.Center;
        float dist = math.length(delta);
        return dist <= a.Radius + b.Radius;
    }

    public static bool CircleToAABB(Circle c, AABB aabb)
    {
        float2 closest = math.clamp(c.Center, aabb.Min, aabb.Max);
        float2 delta = c.Center - closest;
        return math.lengthsq(delta) <= c.Radius * c.Radius;
    }

    public static bool CircleToOBB(Circle circle, OBB obb)
    {
        // 转换到OBB局部空间
        float2 localPos = circle.Center - obb.Center;
        float2 closest = float2.zero;

        for (int i = 0; i < 2; i++)
        {
            float2 axis = obb.GetAxis(i);
            float projection = math.dot(localPos, axis);
            float halfLength = obb.GetHalfLength(i);

            projection = math.clamp(projection, -halfLength, halfLength);
            closest += axis * projection;
        }

        float2 delta = localPos - closest;
        return math.lengthsq(delta) <= circle.Radius * circle.Radius;
    }

    public static bool CircleToSector(Circle circle, Sector sector)
    {
        float centerDistSq = math.lengthsq(circle.Center - sector.Center);
        float totalRadius = circle.Radius + sector.Radius;
        if (centerDistSq > totalRadius * totalRadius)
            return false;

        if (PointToSector(circle.Center, sector))
            return true;

        if (centerDistSq <= circle.Radius * circle.Radius)
            return true;

        float2 leftBound = ColliderHelper.RotateVector(sector.Direction, -sector.Angle / 2);
        float2 rightBound = ColliderHelper.RotateVector(sector.Direction, sector.Angle / 2);

        if (CircleToLineSegment(circle,
        new LineSegment(sector.Center, sector.Center + leftBound * sector.Radius)) ||
        CircleToLineSegment(circle,
            new LineSegment(sector.Center, sector.Center + rightBound * sector.Radius)))
        {
            return true;
        }

        float2 circleToSector = circle.Center - sector.Center;
        float distanceToArc = math.length(circleToSector) - sector.Radius;

        if (math.abs(distanceToArc) <= circle.Radius)
        {
            float angleToCircle = math.atan2(circleToSector.y, circleToSector.x);
            float sectorStartAngle = math.atan2(sector.Direction.y, sector.Direction.x) - sector.Angle / 2;
            float sectorEndAngle = sectorStartAngle + sector.Angle;

            angleToCircle = ColliderHelper.NormalizeAngle(angleToCircle);
            sectorStartAngle = ColliderHelper.NormalizeAngle(sectorStartAngle);
            sectorEndAngle = ColliderHelper.NormalizeAngle(sectorEndAngle);

            if (sectorStartAngle <= sectorEndAngle)
            {
                if (angleToCircle >= sectorStartAngle && angleToCircle <= sectorEndAngle)
                    return true;
            }
            else
            {
                if (angleToCircle >= sectorStartAngle || angleToCircle <= sectorEndAngle)
                    return true;
            }

            float angleDiff1 = math.abs(ColliderHelper.NormalizeAngle(angleToCircle - sectorStartAngle));
            float angleDiff2 = math.abs(ColliderHelper.NormalizeAngle(angleToCircle - sectorEndAngle));
            if (math.min(angleDiff1, angleDiff2) <= math.asin(circle.Radius / math.length(circleToSector)))
                return true;
        }

        return false;
    }

    public static bool CircleToLineSegment(Circle c, LineSegment ls)
    {
        float2 closest = ClosestPointOnSegment(ls.Start, ls.End, c.Center);
        return PointToCircle(closest, c);
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