
using System;

public class ActorDisplayManagerSystem : ISystem, IAwake
{
    private EfficientList<ActorDisplay> actors;

    public void Awake()
    {
        actors = new EfficientList<ActorDisplay>(200);
    }

    public void CreateActor(int actorId)
    {
        var actorManagerSystem = SystemManager.GetSystem<ActorManagerSystem>();
        var actor = actorManagerSystem.GetActor(actorId);
        var poolSystem = SystemManager.GetSystem<GameObjectPoolSystem>();
        var display = poolSystem.Fetch(actor.model);
        var actorDisplay = display.AddComponent<ActorDisplay>();
        actorDisplay.Initialization(actor);
        var actorDisplayId = actors.Add(actorDisplay);
        actor.ActorDisplayId = actorDisplayId;
    }
}
