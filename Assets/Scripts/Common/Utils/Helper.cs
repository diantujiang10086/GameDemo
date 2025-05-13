using Unity.Mathematics;

public static class Helper
{
    private const float epsilon = 1e-6f;

    public static bool Equal(this float3 a, float3 b)
    {
        return math.all(math.abs(a - b) < epsilon);
    }

    public static bool Equal(this quaternion a, quaternion b)
    {
        return math.all(math.abs(a.value - b.value) < new float4(epsilon));
    }
}

