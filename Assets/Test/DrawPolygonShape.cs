using Unity.Mathematics;
using UnityEngine;

public class DrawPolygonShape : TestDrawShape
{
    public Vector2[] points;
    private Polygon polygon = new Polygon();

    public override IShape GetShape()
    {
        return polygon;
    }

    public override void UpdateShape()
    {
        if (points == null)
            return;
        polygon.Points = new float2[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            polygon.Points[i] = new float2(transform.position.x + points[i].x, transform.position.z + points[i].y);
        }

        ShapeVisualizer.DrawShape(polygon, Color.magenta);
    }
}
