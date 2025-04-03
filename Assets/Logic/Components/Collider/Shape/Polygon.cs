using Unity.Mathematics;

public class Polygon:IShape
{
    public float2[] Points;

    public void ComputeAABB(ref AABB aabb, Unit unit)
    {

    }

    public bool TestOverlap(IShape shape)
    {
        return false;
    }

    public bool TestOverlap(float2 point)
    {
        return false;
    }
}