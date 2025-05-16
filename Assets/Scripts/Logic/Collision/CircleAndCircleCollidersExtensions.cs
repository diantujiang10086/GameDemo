using System.Runtime.CompilerServices;
using Unity.Mathematics;

internal static class CircleAndCircleCollidersExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Overlap(CircleGeometry circleGeometryA, CircleGeometry circleGeometryB)
    {
        float2 delta = circleGeometryA.center - circleGeometryB.center;
        float distanceSq = math.lengthsq(delta);
        float radiusSum = circleGeometryA.radius + circleGeometryB.radius;
        return distanceSq <= radiusSum * radiusSum;
    }
}
