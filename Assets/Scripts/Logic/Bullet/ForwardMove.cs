using Unity.Mathematics;

[BulletMove]
public class ForwardMove : BaseBulletMoveAgent
{
    private float3 direction;
    protected override void OnInitialize()
    {
        
        var s = math.degrees(math.EulerXYZ(unit.rotation));
        var angle = math.degrees(math.EulerXYZ(unit.rotation)).z;
        float angleRad = math.radians(angle);
        direction = new float3(math.cos(angleRad), math.sin(angleRad), 0);
    }
    protected override void OnFixedUpdate(float elaspedTime)
    {
        unit.position += direction * (config.moveSpeed * elaspedTime);
    }
}