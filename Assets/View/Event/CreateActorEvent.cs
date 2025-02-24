using UnityEngine;

[Event]
public class CreateActorEvent : AEvent<CreateActorInfo>
{
    protected override void Run(CreateActorInfo info)
    {
        var system = SystemManager.GetSystem<ActorDisplayManagerSystem>();
        system.CreateActor(info.actorId);
    }
}