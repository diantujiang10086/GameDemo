using UnityEngine;

[Event]
public class CreateActorEvent : AEvent<CreateActorInfo>
{
    protected override void Run(CreateActorInfo info)
    {
        var system = World.inst.GetComponent<ActorDisplayManagerSystem>();
        system.CreateActor(info.actorId);
    }
}