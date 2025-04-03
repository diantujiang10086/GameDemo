
using Unity.Mathematics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ShapeVisualizer 
{
    static public void DrawShape(IShape shape, Color color)
    {
        Gizmos.color = color;
        switch (shape)
        {
            case AABB aabb:
#if UNITY_EDITOR
                Handles.color = color;
                Handles.DrawWireCube(new Vector3((aabb.Min.x + aabb.Max.x) * 0.5f, 0, (aabb.Min.y + aabb.Max.y) * 0.5f),
                    new Vector3(aabb.Max.x - aabb.Min.x, 0, aabb.Max.y - aabb.Min.y));
#endif
                break;

            case Circle circle:
#if UNITY_EDITOR
                Handles.color = color;
                Handles.DrawWireDisc(new Vector3(circle.Center.x, 0, circle.Center.y), Vector3.up, circle.Radius);
#endif
                break;

            case LineSegment line:
                Gizmos.DrawLine(
                    new Vector3(line.Start.x, 0, line.Start.y),
                    new Vector3(line.End.x, 0, line.End.y)
                );
                break;

            case OBB obb:
                DrawOBB(obb);
                break;

            case Sector sector:
                DrawSector(sector);
                break;

            case Polygon polygon:
                DrawPolygon(polygon);
                break;
        }
    }


    static void DrawOBB(OBB obb)
    {
        Vector2[] corners = new Vector2[4];
        float halfWidth = obb.Size.x * 0.5f;
        float halfHeight = obb.Size.y * 0.5f;
        float cos = math.cos(obb.Rotation);
        float sin = math.sin(obb.Rotation);

        corners[0] = obb.Center + new float2(-halfWidth * cos - halfHeight * sin, halfWidth * sin - halfHeight * cos);
        corners[1] = obb.Center + new float2(halfWidth * cos - halfHeight * sin, -halfWidth * sin - halfHeight * cos);
        corners[2] = obb.Center + new float2(halfWidth * cos + halfHeight * sin, -halfWidth * sin + halfHeight * cos);
        corners[3] = obb.Center + new float2(-halfWidth * cos + halfHeight * sin, halfWidth * sin + halfHeight * cos);

        for (int i = 0; i < 4; i++)
            Gizmos.DrawLine(
                new Vector3(corners[i].x,0, corners[i].y),
                new Vector3(corners[(i + 1) % 4].x,0, corners[(i + 1) % 4].y)
            );
    }


    static void DrawSector(Sector sector)
    {
        int segments = 20;
        float angleStep = sector.Angle / segments;
        Vector3 prev = new Vector3(sector.Center.x + sector.Direction.x * sector.Radius,0,
                                             sector.Center.y + sector.Direction.y * sector.Radius);

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i;
            float2 rotatedDir = new float2(
                sector.Direction.x * math.cos(angle) - sector.Direction.y * math.sin(angle),
                sector.Direction.x * math.sin(angle) + sector.Direction.y * math.cos(angle)
            );
            Vector3 next = new Vector3(sector.Center.x + rotatedDir.x * sector.Radius,0,
                                                 sector.Center.y + rotatedDir.y * sector.Radius);
            Gizmos.DrawLine(prev, next);
            prev = next;
        }

        Gizmos.DrawLine(new Vector3(sector.Center.x,0, sector.Center.y), prev);
        Gizmos.DrawLine(new Vector3(sector.Center.x,0, sector.Center.y),
                        new Vector3(sector.Center.x + sector.Direction.x * sector.Radius, 0 ,sector.Center.y + sector.Direction.y * sector.Radius));
    }


    static void DrawPolygon(Polygon polygon)
    {
        if (polygon.Points.Length < 2) return;

        for (int i = 0; i < polygon.Points.Length; i++)
        {
            Vector3 start = new Vector3(polygon.Points[i].x,0, polygon.Points[i].y);
            Vector3 end = new Vector3(polygon.Points[(i + 1) % polygon.Points.Length].x, 0, polygon.Points[(i + 1) % polygon.Points.Length].y);
            Gizmos.DrawLine(start, end);
        }
    }

}
