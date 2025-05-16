using Unity.Mathematics;

[Skill]
public class FanShotBulletSkill : BaseSkill
{
    protected override void OnInitialize()
    {
        int.TryParse(config.arg0, out var bulletId);
        int.TryParse(config.arg1, out var bulletCount);
        float.TryParse(config.arg2, out var startAngle);
        float.TryParse(config.arg3, out var spreadAngle);
        float.TryParse(config.arg4, out var startRadius);
        float.TryParse(config.arg5, out var bulletSize);

        float angleStep = bulletCount > 1 ? spreadAngle / (bulletCount - 1) : 0f;
        float firstAngle = startAngle - spreadAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = firstAngle + angleStep * i;
            var spawnPos = CalculateArcPosition(currentAngle, startRadius);
            CreateBullet2D(bulletId, spawnPos, currentAngle, bulletSize);
        }
        Remove();
    }

    private float3 CalculateArcPosition(float angle, float radius)
    {
        float rad = math.radians(angle);
        float3 offset = new float3(math.cos(rad), math.sin(rad), 0) * radius;

        return caster.position + offset;
    }

    private void CreateBullet2D(int id, float3 origin, float angle, float size)
    {
        var bullet = BulletManager.Instance.CreateBullet(id, new BulletArguments
        {
            owner = caster,
            position = origin,
            scale = size,
            roation = new float3(0, 0, angle),
        });
        //测试
        var config = new CollisionConfig
        {
            isCollisionDestory = true,
            isEnableColliderDetection = true,
            colliderShape = ColliderShape.Circle,
            layer = 2,
            colliderLayer = 1,
            offset = float2.zero,
            radius = 0.5f,
            size = float2.zero
        };
        bullet.AddComponent<Collision2DComponent, CollisionConfig>(config);
    }
}