﻿public class ActorManagerSystem : Entity
{
    private EfficientList<Actor> actors = new EfficientList<Actor>();
    public Actor CreateActor(string config)
    {
        var actor = Scene.ZeroScene.AddChild<Actor>();
        var actorId = actors.Add(actor);
        actor.Initialize(actorId, config);

        var createActor = default(CreateActorInfo);
        createActor.actorId = actorId;
        GameEventSystem.Publish(createActor);

        return actor;
    }

    public Actor GetActor(int actorId)
    {
        return actors.Get(actorId);
    }
}