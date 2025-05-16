using Unity.Mathematics;

public interface IGeometry
{
    float2 min { get; }
    float2 max { get; }
    float2 center { get; }
    ColliderShape colliderShape { get; }
    void UpdateBoundBox(float2 position, float angle);
}
