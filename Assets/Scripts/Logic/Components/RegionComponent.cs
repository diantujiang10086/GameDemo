using Unity.Mathematics;

public class RegionDestoryComponent : Entity, IAwake, IFixedUpdate
{
    private Unit unit;
    private float2 min;
    private float2 max;
    public void Awake()
    {
        unit = GetParent<Unit>();
        min = new float2(-15, -10);
        max = new float2(15, 10);
    }

    public void FixedUpdate(float elaspedTime)
    {
        float2 pos2D = unit.position.xy;

        if (pos2D.x < min.x || pos2D.x > max.x || pos2D.y < min.y || pos2D.y > max.y)
        {
            unit.Dispose();
        }

    }
}