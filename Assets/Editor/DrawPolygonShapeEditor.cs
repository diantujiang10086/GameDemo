using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DrawPolygonShape))]
public class DrawPolygonShapeEditor : Editor
{
    private bool isEditor = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUI.color = isEditor? Color.gray: Color.white;
        isEditor = GUILayout.Toggle(isEditor, "±à¼­");
        GUI.color = Color.white;
    }

    private void OnSceneGUI()
    {
        var component = target as DrawPolygonShape;
        
        for (int i = 0; i < component.points.Length; i++)
        {
            var point = component.points[i];
            var _point = component.transform.position + new Vector3(point.x, 0, point.y);
            if(isEditor)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPoint = Handles.PositionHandle(_point, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    var _newPoint = newPoint - component.transform.position;
                    component.points[i] = new Vector2(_newPoint.x, _newPoint.z);
                }
            }
        }
    }
}