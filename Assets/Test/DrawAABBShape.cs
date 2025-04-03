using Unity.Mathematics;
using UnityEngine;

public class DrawAABBShape :  TestDrawShape
{
    public Vector2 size;
    private AABB aabb = new AABB();

    public override void UpdateShape()
    {
        float2 center = new float2(transform.position.x, transform.position.z);
        float2 sizeFloat2 = new float2(size.x, size.y);
        aabb.Min = center - sizeFloat2 * 0.5f;
        aabb.Max = center + sizeFloat2 * 0.5f;
        
    }

    public override IShape GetShape()
    {
        return aabb;
    }

}
