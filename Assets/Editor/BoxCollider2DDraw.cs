using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoxCollider2D), isFallback = true)]
public class BoxCollider2DDraw : Editor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    static void Draw(BoxCollider2D collider, GizmoType gizmoType)
    {
        if (collider == null || !collider.enabled) return;

        ColliderEditorDraw.UpdateGizmos(gizmoType);

        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}
