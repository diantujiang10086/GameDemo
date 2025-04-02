using Unity.Mathematics;

public static partial class CollisionUtils
{
    public static bool PointToPoint(float2 a, float2 b)
    {
        return math.all(a == b);
    }

    public static bool PointToCircle(float2 p, Circle c)
    {
        float2 delta = p - c.Center;
        return math.lengthsq(delta) <= c.Radius * c.Radius;
    }

    public static bool PointToAABB(float2 p, AABB aabb)
    {
        return p.x >= aabb.Min.x && p.x <= aabb.Max.x &&
               p.y >= aabb.Min.y && p.y <= aabb.Max.y;
    }

    public static bool PointToOBB(float2 point, OBB obb)
    {
        float2 dir = point - obb.Center;

        for (int i = 0; i < 2; i++)
        {
            float projection = math.dot(dir, obb.GetAxis(i));
            if (math.abs(projection) > obb.GetHalfLength(i))
                return false;
        }

        return true;
    }

    public static bool PointToSector(float2 p, Sector s)
    {
        float2 delta = p - s.Center;
        float distSq = math.lengthsq(delta);
        if (distSq > s.Radius * s.Radius) return false;
        if (distSq == 0) return true;

        float projX = math.dot(delta, s.Direction);
        if (projX < 0) return false;

        float projY = math.abs(delta.x * s.Direction.y - delta.y * s.Direction.x);
        float tanHalf = math.tan(s.Angle / 2);
        return projY <= projX * tanHalf;
    }

    public static bool PointToLineSegment(float2 p, LineSegment ls)
    {
        float2 closest = ClosestPointOnSegment(ls.Start, ls.End, p);
        return PointToPoint(closest, p);
    }
}