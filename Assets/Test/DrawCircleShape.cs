public class DrawCircleShape :  TestDrawShape
{
    public float radius;
    private Circle circle = new Circle();
    public override IShape GetShape()
    {
        return circle;
    }

    public override void UpdateShape()
    {
        circle.Center = new Unity.Mathematics.float2(transform.position.x, transform.position.z);
        circle.Radius = radius;
    }

}
