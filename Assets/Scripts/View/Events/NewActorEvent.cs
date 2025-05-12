using UnityEngine;

[Event]
public class NewActorEvent : AEvent<NewActor>
{
    protected override void Run(NewActor a)
    {
        var actor = ActorManager.Instance.Get(a.actorId);
        DisplayManager.Instance.CreateDisplay(actor);
    }
}
