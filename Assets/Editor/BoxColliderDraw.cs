using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoxCollider), isFallback =true)]
public class BoxColliderDraw : Editor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    static void Draw(BoxCollider collider, GizmoType gizmoType)
    {
        if (collider == null || !collider.enabled) return;

        ColliderEditorDraw.UpdateGizmos(gizmoType);

        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}
