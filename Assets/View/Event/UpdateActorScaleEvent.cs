[Event]
public class UpdateActorScaleEvent : ActorEvent<UpdateActorScale>
{
    protected override void Run(Actor actor, UpdateActorScale a)
    {
        var actorDisplayManagerSystem = World.inst.GetComponent<ActorDisplayManagerSystem>();
        var actorDisplay = actorDisplayManagerSystem.GetActorDisplay(actor.ActorDisplayId);
        actorDisplay.UpdateLocalScale(actor.LocalScale);
    }
}