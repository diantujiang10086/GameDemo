[Event]
public class UnitCollisionStayEvent : AEvent<CollisionStay>
{
    protected override void Run(CollisionStay a)
    {
        Log.Debug($"stay:{a.entityA} , {a.entityB}");
    }
}