
using System;

public class ActorDisplayManagerSystem : Entity, IAwake
{
    private EfficientList<ActorDisplay> actors;

    public void Awake()
    {
        actors = new EfficientList<ActorDisplay>(200);
    }

    public void CreateActor(int actorId)
    {
        var actorManagerSystem = World.inst.GetComponent<ActorManagerSystem>();
        var actor = actorManagerSystem.GetActor(actorId);
        var poolSystem = World.inst.GetComponent<GameObjectPoolSystem>();
        var display = poolSystem.Fetch(actor.model);
        var actorDisplay = display.AddComponent<ActorDisplay>();
        actorDisplay.Initialization(actor);
        var actorDisplayId = actors.Add(actorDisplay);
        actor.ActorDisplayId = actorDisplayId;
    }

    public ActorDisplay GetActorDisplay(int actorId)
    {
        return actors[actorId];
    }
}
