using Unity.Mathematics;
using UnityEngine;

public class DrawOBBShape : TestDrawShape
{
    public Vector2 size = new Vector2(2, 1);
    public float rotation; 
    private OBB obb = new OBB();
    public override IShape GetShape()
    {
        return obb;
    }

    public override void UpdateShape()
    {
        obb.Center = new float2(transform.position.x, transform.position.z);
        obb.Size = size;
        obb.Rotation = math.radians(rotation);

        ShapeVisualizer.DrawShape(obb, Color.cyan);
    }
}
