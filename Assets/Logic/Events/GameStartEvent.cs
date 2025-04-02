using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Event]
public class GameStartEvent : AEvent<GameStart>
{
    protected override void Run(GameStart a)
    {
        World.Instance.AddSigleton<GridManager, float>(5);
        World.Instance.AddSigleton<UnitManager>();
        

        var player = UnitManager.Instance.AddUnit(UnitType.Player, Vector3.zero);
        player.LocalScale = 1;
        World.Instance.AddSigleton<Player, Unit>(player);
        player.AddComponent<DisplayComponent, string>("player");
        player.AddComponent<MoveComponent, float>(10);
        player.AddComponent<InputComponent>();
        var playerColliderComponent = player.AddComponent<ColliderComponent,IShape>(new Circle { Center = 0, Radius = 1 });
        playerColliderComponent.SetLayer(1);
        playerColliderComponent.SetCollideLayer(2);
        playerColliderComponent.EnableCheckCollider(true);

       

        //var bullet = UnitManager.Instance.AddUnit(UnitType.EnemyBullet, new Vector3(10, 0, 0));
        //bullet.AddComponent<DisplayComponent, string>("bullet");
        //var moveComponent = bullet.AddComponent<MoveComponent, float>(1);
        //moveComponent.SetDirection(Vector3.left);
        //var bulletColliderComponent = bullet.AddComponent<ColliderComponent, IShape>(new Circle {Center = 0, Radius = 1 });
        //bulletColliderComponent.SetLayer(2);
        //bulletColliderComponent.SetCollideLayer(1);
        Emitter(new Vector3(10,0,0), 0,360,5,20,0.5f).Forget();
    }

    async UniTask Emitter(Vector3 startPosition, float startAngle, float endAngle, float angleStep, float speed, float fireRate)
    {
        int c = 20;
        int count = 0;
        while(c-->0)
        {
            float aimAngle = startAngle;

            for (float angle = startAngle;angle < endAngle;angle += angleStep)
            {
                float currentAngle = angle + aimAngle;
                Vector3 direction = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad),0, Mathf.Sin(currentAngle * Mathf.Deg2Rad)).normalized;

                var bullet = UnitManager.Instance.AddUnit(UnitType.EnemyBullet, startPosition);
                bullet.LocalScale = 1;
                bullet.AddComponent<DisplayComponent, string>("bullet");
                var moveComponent = bullet.AddComponent<MoveComponent, float>(1);
                moveComponent.SetDirection(direction);
                var bulletColliderComponent = bullet.AddComponent<ColliderComponent, IShape>(new Circle { Center = 0, Radius = 1 });
                bulletColliderComponent.SetLayer(2);
                bulletColliderComponent.SetCollideLayer(1);
                count++;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(fireRate));
            //Debug.Log($"current count: {count}");
        }
    }
}
