[Event]
public class UnitCollisionExitEvent : AEvent<CollisionExit>
{
    protected override void Run(CollisionExit a)
    {
        //Log.Debug($"exit:{a.entityA} , {a.entityB}");
    }
}
