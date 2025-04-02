using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EdgeCollider2D), isFallback = true)]
public class EdgeCollider2DDraw : Editor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    static void Draw(EdgeCollider2D collider, GizmoType gizmoType)
    {
        if (collider == null || !collider.enabled) return;

        ColliderEditorDraw.UpdateGizmos(gizmoType);

        var transform = collider.transform;
        Vector3 start = transform.TransformPoint(collider.points[0]);
        for (int i = 1; i < collider.points.Length; i++)
        {
            Gizmos.DrawLine(start, transform.TransformPoint(collider.points[i]));
            start = transform.TransformPoint(collider.points[i]);
        }
    }
}
