using System.Runtime.CompilerServices;
using Unity.Mathematics;

public static class CollisionHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AABBOverlap(IGeometry geometryA, IGeometry geometryB)
    {
        float2 aMin = geometryA.min;
        float2 aMax = geometryA.max;
        float2 bMin = geometryB.min;
        float2 bMax = geometryB.max;

        return !(aMax.x < bMin.x || aMin.x > bMax.x ||
                 aMax.y < bMin.y || aMin.y > bMax.y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Overlap(IGeometry geometryA, IGeometry geometryB)
    {
        if(geometryA.colliderShape == ColliderShape.Circle)
        {
            if(geometryB.colliderShape == ColliderShape.Circle)
            {
                CircleAndCircleCollidersExtensions.Overlap(geometryA as CircleGeometry, geometryB as CircleGeometry);
            }
            else if (geometryB.colliderShape == ColliderShape.Box)
            {
                BoxAndCircleCollidersExtensions.Overlap(geometryA as CircleGeometry, geometryB as BoxGeometry);
            }
        }
        if(geometryA.colliderShape == ColliderShape.Box)
        {
            if (geometryB.colliderShape == ColliderShape.Circle)
            {
                BoxAndCircleCollidersExtensions.Overlap(geometryB as CircleGeometry, geometryA as BoxGeometry);
            }
            else if (geometryB.colliderShape == ColliderShape.Box)
            {
                BoxAndBoxCollidersExtensions.Overlap(geometryB as BoxGeometry, geometryA as BoxGeometry);
            }
        }
        return true;
    }
}
