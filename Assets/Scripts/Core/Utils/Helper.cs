using Unity.Mathematics;
using UnityEngine;

public static class Helper
{
    private const float epsilon = 1e-6f;
    private static Mesh quad;
    public static Mesh MakeQuad()
    {
        if (quad != null)
            return quad;

        quad = new Mesh();
        quad.vertices = new Vector3[] {
            new Vector3(-0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f),
        };
        quad.uv = new Vector2[] {
            new Vector2(0,0), new Vector2(1,0),
            new Vector2(1,1), new Vector2(0,1)
        };
        quad.triangles = new int[] { 2, 1, 0, 0, 3, 2 };
        return quad;

    }

    public static bool Equal(this float3 a, float3 b)
    {
        return math.all(math.abs(a - b) < epsilon);
    }

    public static bool Equal(this quaternion a, quaternion b)
    {
        return math.all(math.abs(a.value - b.value) < new float4(epsilon));
    }

}

