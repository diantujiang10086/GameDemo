using Unity.Mathematics;

public class CollisionDetection
{
    public static bool TestOverlap(IShape a, IShape b)
    {
        return a.TestOverlap(b);
    }

    public static bool TestOverlap(IShape shape, float2 point)
    {
        return shape.TestOverlap(point);
    }
}