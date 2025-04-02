using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PolygonCollider2D), isFallback = true)]
public class PolygonCollider2DDraw : Editor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    static void Draw(PolygonCollider2D collider, GizmoType gizmoType)
    {
        if (collider == null || !collider.enabled) return;

        ColliderEditorDraw.UpdateGizmos(gizmoType);

        for (int i = 0; i < collider.pathCount; i++)
        {
            Vector2[] points = collider.GetPath(i);
            if (points.Length > 1)
            {
                Vector3 prevPoint = collider.transform.TransformPoint(points[0]);
                for (int j = 1; j < points.Length; j++)
                {
                    Vector3 currPoint = collider.transform.TransformPoint(points[j]);
                    Gizmos.DrawLine(prevPoint, currPoint);
                    prevPoint = currPoint;
                }
                Gizmos.DrawLine(prevPoint, collider.transform.TransformPoint(points[0]));
            }
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}
