using System.Runtime.CompilerServices;
using Unity.Mathematics;

internal static class BoxAndCircleCollidersExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Overlap(CircleGeometry circleGeometry, BoxGeometry boxGeometry)
    {
        float2 circlePos = circleGeometry.center;
        float2 boxPos = boxGeometry.center;
        float2 localPos = circlePos - boxPos;

        float angle = boxGeometry.angle;
        float cos = math.cos(-angle);
        float sin = math.sin(-angle);

        float2 rotated = new float2(
            localPos.x * cos - localPos.y * sin,
            localPos.x * sin + localPos.y * cos
        );

        float2 halfSize = boxGeometry.size * 0.5f;
        float2 clamped = math.clamp(rotated, -halfSize, halfSize);

        float2 closest = boxPos + new float2(
            clamped.x * cos + clamped.y * sin,
            -clamped.x * sin + clamped.y * cos
        );

        float2 delta = circlePos - closest;

        return math.lengthsq(delta) <= circleGeometry.radius * circleGeometry.radius;
    }
}