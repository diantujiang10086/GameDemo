using UnityEngine;

public static class ViewHelper
{
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
}