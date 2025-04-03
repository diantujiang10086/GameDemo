using Unity.Mathematics;

public class ColliderHelper
{
    public static float2 RotateVector(float2 v, float angle)
    {
        float cosA = math.cos(angle);
        float sinA = math.sin(angle);
        return new float2(v.x * cosA - v.y * sinA, v.x * sinA + v.y * cosA);
    }

    public static float NormalizeAngle(float angle)
    {
        while (angle < 0) angle += 2 * math.PI;
        while (angle > 2 * math.PI) angle -= 2 * math.PI;
        return angle;
    }
}