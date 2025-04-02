using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CapsuleCollider2D), isFallback = true)]
public class CapsuleCollider2DDraw : Editor
{
    static private int segments = 20;
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    static void Draw(CapsuleCollider2D collider, GizmoType gizmoType)
    {
        if (collider == null || !collider.enabled) return;

        ColliderEditorDraw.UpdateGizmos(gizmoType);

        DrawCapsule(collider);
    }

    private static void DrawCapsule(CapsuleCollider2D collider)
    {
        DrawLine(collider);
        var center = collider.bounds.center;
        if (collider.direction == CapsuleDirection2D.Vertical)
        {
            var radius = collider.size.x / 2;
            var radiusCenter = collider.size.x <= collider.size.y ? collider.size.y / 2 - radius : 0;
            DrawHalfCircle(center + new Vector3(0, radiusCenter, 0), radius, segments, true);
            DrawHalfCircle(center + new Vector3(0, -radiusCenter, 0), radius, segments, false);
        }
        else
        {
            var radius = collider.size.y / 2;
            var radiusCenter = collider.size.y <= collider.size.x ? collider.size.x / 2 - radius : 0;
            DrawHalfCircle(center + new Vector3(-radiusCenter, 0, 0), radius, segments, true, true);
            DrawHalfCircle(center + new Vector3(radiusCenter, 0, 0), radius, segments, false, true);
        }
    }

    static void DrawLine(CapsuleCollider2D collider)
    {
        var center = collider.bounds.center;
        bool isVertical = collider.direction == CapsuleDirection2D.Vertical;
        if ((isVertical && collider.size.x >= collider.size.y) || (!isVertical && collider.size.y >= collider.size.x))
            return;

        if (collider.direction == CapsuleDirection2D.Vertical)
        {
            var x = collider.size.x / 2;
            var y = collider.size.y / 2 - x;
            Gizmos.DrawLine(center + new Vector3(-x, y), center + new Vector3(-x, -y));
            Gizmos.DrawLine(center + new Vector3(x, y), center + new Vector3(x, -y));
        }
        else
        {
            var y = collider.size.y / 2;
            var x = collider.size.x / 2 - y;
            Gizmos.DrawLine(center + new Vector3(-x, y), center + new Vector3(x, y));
            Gizmos.DrawLine(center + new Vector3(-x, -y), center + new Vector3(x, -y));
        }
    }

    private static void DrawHalfCircle(Vector3 center, float radius, int segments, bool isTop, bool isHorizontal = false)
    {
        float angleOffset = isTop ? 0 : Mathf.PI;
        if (isHorizontal) angleOffset += Mathf.PI / 2;

        Vector3 prevPoint = center + new Vector3(Mathf.Cos(angleOffset) * radius, Mathf.Sin(angleOffset) * radius, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleOffset + (Mathf.PI / segments) * i;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
