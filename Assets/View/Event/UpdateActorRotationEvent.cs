[Event]
public class UpdateActorRotationEvent : ActorEvent<UpdateActorRotation>
{
    protected override void Run(Actor actor, UpdateActorRotation a)
    {
        var actorDisplayManagerSystem = World.inst.GetComponent<ActorDisplayManagerSystem>();
        var actorDisplay = actorDisplayManagerSystem.GetActorDisplay(actor.ActorDisplayId);
        actorDisplay.UpdateRotation(actor.Rotation);
    }
}
