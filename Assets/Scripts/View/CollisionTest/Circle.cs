using Unity.Mathematics;
using UnityEngine;

public class Circle : Shape
{
    public float radius;
    
    public override void UpdateGeometry()
    {
        geometry = new CircleGeometry( ColliderShape.Circle, float2.zero, radius);
        var pos = new float2(transform.position.x, transform.position.y);
        geometry.UpdateBoundBox(pos, 0);
    }


    public override void Draw(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, (geometry as CircleGeometry).radius);
    }
}
