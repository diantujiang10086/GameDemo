
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


    static void DrawSector(Sector s, int segments = 32)
    {
        Vector3 center = new Vector3(s.Center.x, 0, s.Center.y);
        Vector3 forward = new Vector3(s.Direction.x, 0, s.Direction.y);

        float halfAngle = s.Angle * 0.5f;

        // 起始旋转角度
        float startAngle = -halfAngle;
        float endAngle = halfAngle;

        // 绘制圆弧线段
        Vector3 prevPoint = center + Quaternion.Euler(0, math.degrees(startAngle), 0) * forward * s.Radius;
        for (int i = 1; i <= segments; i++)
        {
            float t = math.lerp(startAngle, endAngle, i / (float)segments);
            Vector3 nextPoint = center + Quaternion.Euler(0, math.degrees(t), 0) * forward * s.Radius;

            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        // 绘制两条边界线
        Vector3 left = Quaternion.Euler(0, math.degrees(-halfAngle), 0) * forward * s.Radius;
        Vector3 right = Quaternion.Euler(0, math.degrees(halfAngle), 0) * forward * s.Radius;
        Gizmos.DrawLine(center, center + left);
        Gizmos.DrawLine(center, center + right);

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
