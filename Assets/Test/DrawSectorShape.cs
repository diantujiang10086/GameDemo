using Unity.Mathematics;
using UnityEngine;

public class DrawSectorShape : TestDrawShape
{
    public float radius = 2f;
    public float angle = 60f;
    public float directionAngle = 0;
    private Sector sector = new Sector();
    public override IShape GetShape()
    {
        return sector;
    }
    public override void UpdateShape()
    {
        float radians = math.radians(directionAngle);
        float2 direction = new float2(math.cos(radians), math.sin(radians));
        sector.Center = new float2(transform.position.x, transform.position.z);
        sector.Radius = radius;
        sector.Angle = math.radians(angle);
        sector.Direction = math.normalize(direction);

        ShapeVisualizer.DrawShape(sector, Color.red);
    }
}
