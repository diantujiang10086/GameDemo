[Event]
public class UpdateActorPositionEvent : ActorEvent<UpdateActorPosition>
{
    protected override void Run(Actor actor, UpdateActorPosition a)
    {
        var actorDisplayManagerSystem = World.inst.GetComponent<ActorDisplayManagerSystem>();
        var actorDisplay = actorDisplayManagerSystem.GetActorDisplay(actor.ActorDisplayId);
        actorDisplay.UpdatePosition(actor.Position);
    }
}
