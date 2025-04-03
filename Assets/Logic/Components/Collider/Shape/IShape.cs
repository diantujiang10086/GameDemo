using Unity.Mathematics;

public interface IShape
{
    bool TestOverlap(IShape shape);
    bool TestOverlap(float2 point);
    void ComputeAABB(ref AABB aabb, Unit unit);
}
