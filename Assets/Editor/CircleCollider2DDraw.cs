using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CircleCollider2D), isFallback = true)]
public class CircleCollider2DDraw : Editor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    static void Draw(CircleCollider2D collider, GizmoType gizmoType)
    {
        if (collider == null || !collider.enabled) return;

        ColliderEditorDraw.UpdateGizmos(gizmoType);

        var radius = collider.bounds.size.x / 2f;
        Gizmos.DrawWireSphere(collider.bounds.center, radius);
    }
}
