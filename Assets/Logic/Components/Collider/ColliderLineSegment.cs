using Unity.Mathematics;

public static partial class CollisionUtils
{
    public static bool LineSegmentToLineSegment(LineSegment a, LineSegment b)
    {
        float2 p = a.Start;
        float2 r = a.End - p;
        float2 q = b.Start;
        float2 s = b.End - q;

        float rxs = r.x * s.y - r.y * s.x;
        float2 qp = q - p;

        if (rxs == 0) return false; // Æ½ÐÐ

        float t = (qp.x * s.y - qp.y * s.x) / rxs;
        float u = (qp.x * r.y - qp.y * r.x) / rxs;

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }

}