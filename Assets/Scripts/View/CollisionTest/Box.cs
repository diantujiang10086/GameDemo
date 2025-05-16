using Unity.Mathematics;
using UnityEngine;

public class Box : Shape
{
    public float2 size;
    public float angle;

    public override void UpdateGeometry()
    {
        geometry = new BoxGeometry(ColliderShape.Box, float2.zero, size);
        var pos = new float2(transform.position.x, transform.position.y);
        geometry.UpdateBoundBox(pos, math.radians(angle));
    }

    public override void Draw(Color color)
    {
        Gizmos.color = color;
        var pos = new float2(transform.position.x, transform.position.y);
        var size = (geometry as BoxGeometry).size;

        BoxAndBoxCollidersExtensions.CalculateBoxCorners(geometry as BoxGeometry, out float2 corners0, out float2 corners1, out float2 corners2, out float2 corners3);
        Vector2[] corners = new Vector2[4];
        corners[0] = corners0;
        corners[1] = corners1;
        corners[2] = corners2;
        corners[3] = corners3;

        for (int i = 0; i < 4; i++)
        {
            var p = new Vector3(corners[i].x, corners[i].y, 0);
            Gizmos.DrawLine(p, new Vector3(corners[(i + 1) % 4].x, corners[(i + 1) % 4].y, 0));
            UnityEditor.Handles.Label(p, $"{p}");
        }
    }
}
