using Unity.Mathematics;

public class BoxGeometry : IGeometry
{
    public float2 offset;
    public float2 size;

    public float2 min { get; private set; }

    public float2 max { get; private set; }

    public ColliderShape colliderShape { get; private set; }

    public float2 center { get; private set; }

    public float angle { get; private set; }

    public BoxGeometry(ColliderShape colliderShape, float2 offset, float2 size)
    {
        this.colliderShape = colliderShape;
        this.offset = offset;
        this.size = size;
    }

    public void UpdateBoundBox(float2 position, float angle)
    {
        this.angle = angle;
        center = position + offset;
        float2 halfSize = size * 0.5f;

        float2[] corners = new float2[4]
        {
        new float2(-halfSize.x, -halfSize.y),
        new float2(-halfSize.x, halfSize.y),
        new float2(halfSize.x, halfSize.y),
        new float2(halfSize.x, -halfSize.y)
        };

        float sin = math.sin(angle);
        float cos = math.cos(angle);

        float2 firstCorner = Rotate(corners[0], sin, cos) + center;
        min = firstCorner;
        max = firstCorner;

        for (int i = 1; i < 4; i++)
        {
            float2 rotated = Rotate(corners[i], sin, cos) + center;
            min = math.min(min, rotated);
            max = math.max(max, rotated);
        }
    }

    private float2 Rotate(float2 point, float sin, float cos)
    {
        return new float2(
            point.x * cos - point.y * sin,
            point.x * sin + point.y * cos
        );
    }
}
