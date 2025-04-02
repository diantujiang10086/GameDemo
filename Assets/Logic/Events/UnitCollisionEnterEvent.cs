[Event]
public class UnitCollisionEnterEvent : AEvent<CollisionEnter>
{
    protected override void Run(CollisionEnter a)
    {
        var unitA = a.entityA.As<Unit>();
        var unitB = a.entityB.As<Unit>();
        if(unitA.UnitType == UnitType.EnemyBullet)
        {
            unitA.Dispose();
        }
        if(unitB.UnitType == UnitType.EnemyBullet)
        {
            unitB.Dispose();
        }
    }
}
