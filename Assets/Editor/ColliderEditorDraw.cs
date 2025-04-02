using UnityEditor;
using UnityEngine;

public static class ColliderEditorDraw
{
    public static Color color = Color.red;
    public static void UpdateGizmos(GizmoType gizmoType)
    {
        Gizmos.color = gizmoType == GizmoType.Selected ? Color.green : color;
    }
}
