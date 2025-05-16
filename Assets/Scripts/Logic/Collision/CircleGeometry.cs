using Unity.Mathematics;

public class CircleGeometry : IGeometry
{
    public float2 offset;
    public float radius;
    public float2 min { get; private set; }

    public float2 max { get; private set; }
    public ColliderShape colliderShape { get; private set; }

    public float2 center { get; private set; }

    public CircleGeometry(ColliderShape colliderShape, float2 offset, float radius)
    {
        this.colliderShape = colliderShape;
        this.offset = offset;
        this.radius = radius;
    }

    public void UpdateBoundBox(float2 position, float angle)
    {
        center = position + offset;
        min = center - new float2(radius, radius);
        max = center + new float2(radius, radius);
    }
}
