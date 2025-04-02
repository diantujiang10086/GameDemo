using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SphereCollider), isFallback = true)]
public class SphereColliderDraw : Editor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    static void Draw(SphereCollider collider, GizmoType gizmoType)
    {
        if (collider == null || !collider.enabled) return;

        ColliderEditorDraw.UpdateGizmos(gizmoType);

        Gizmos.DrawWireSphere(collider.bounds.center, collider.bounds.size.x / 2);
    }
}