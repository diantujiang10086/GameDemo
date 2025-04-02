using Unity.Mathematics;

public static partial class CollisionUtils
{
    public static bool SectorToSector(Sector a, Sector b)
    {
        if (!CircleToCircle(new Circle(a.Center, a.Radius), new Circle(b.Center, b.Radius)))
            return false;

        if (PointToSector(a.Center, b)) return true;

        if (PointToSector(b.Center, a)) return true;

        float2 aLeft = Helper.RotateVector(a.Direction, -a.Angle / 2);
        float2 aRight = Helper.RotateVector(a.Direction, a.Angle / 2);

        float2 bLeft = Helper.RotateVector(b.Direction, -b.Angle / 2);
        float2 bRight = Helper.RotateVector(b.Direction, b.Angle / 2);

        LineSegment aEdge1 = new LineSegment(a.Center, a.Center + aLeft * a.Radius);
        LineSegment aEdge2 = new LineSegment(a.Center, a.Center + aRight * a.Radius);

        LineSegment bEdge1 = new LineSegment(b.Center, b.Center + bLeft * b.Radius);
        LineSegment bEdge2 = new LineSegment(b.Center, b.Center + bRight * b.Radius);

        return LineSegmentToLineSegment(aEdge1, bEdge1) ||
               LineSegmentToLineSegment(aEdge1, bEdge2) ||
               LineSegmentToLineSegment(aEdge2, bEdge1) ||
               LineSegmentToLineSegment(aEdge2, bEdge2);
    }

    public static bool SectorToLineSegment(Sector sector, LineSegment line)
    {
        if (PointToSector(line.Start, sector) || PointToSector(line.End, sector))
            return true;

        float2 leftBound = Helper.RotateVector(sector.Direction, -sector.Angle / 2);
        float2 rightBound = Helper.RotateVector(sector.Direction, sector.Angle / 2);

        LineSegment radius1 = new LineSegment(sector.Center, sector.Center + leftBound * sector.Radius);
        LineSegment radius2 = new LineSegment(sector.Center, sector.Center + rightBound * sector.Radius);

        if (LineSegmentToLineSegment(line, radius1) || LineSegmentToLineSegment(line, radius2))
            return true;

        if (CircleToLineSegment(new Circle(sector.Center, sector.Radius), line))
        {
            float2? intersection = FindLineCircleIntersection(line, sector.Center, sector.Radius);
            if (intersection.HasValue)
            {
                float2 dirToIntersection = intersection.Value - sector.Center;
                float angle = math.atan2(dirToIntersection.y, dirToIntersection.x);
                float sectorStartAngle = math.atan2(sector.Direction.y, sector.Direction.x) - sector.Angle / 2;
                float sectorEndAngle = sectorStartAngle + sector.Angle;

                angle = Helper.NormalizeAngle(angle);
                sectorStartAngle = Helper.NormalizeAngle(sectorStartAngle);
                sectorEndAngle = Helper.NormalizeAngle(sectorEndAngle);

                if (sectorStartAngle <= sectorEndAngle)
                {
                    if (angle >= sectorStartAngle && angle <= sectorEndAngle)
                        return true;
                }
                else
                {
                    if (angle >= sectorStartAngle || angle <= sectorEndAngle)
                        return true;
                }
            }
        }

        return false;
    }
    private static float2? FindLineCircleIntersection(LineSegment line, float2 center, float radius)
    {
        float2 d = line.End - line.Start;
        float2 f = line.Start - center;

        float a = math.dot(d, d);
        float b = 2 * math.dot(f, d);
        float c = math.dot(f, f) - radius * radius;

        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            return null; // ÎÞ½»µã
        }

        discriminant = math.sqrt(discriminant);
        float t1 = (-b - discriminant) / (2 * a);
        float t2 = (-b + discriminant) / (2 * a);

        if (t1 >= 0 && t1 <= 1)
            return line.Start + d * t1;
        if (t2 >= 0 && t2 <= 1)
            return line.Start + d * t2;

        return null;
    }
}