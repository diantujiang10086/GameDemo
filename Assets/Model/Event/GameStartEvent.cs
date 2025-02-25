[Event]
public class GameStartEvent : AEvent<GameStartInfo>
{
    protected override void Run(GameStartInfo _)
    {
        Log.Debug("Game Start");
        var actorManagerSystem = World.inst.GetComponent<ActorManagerSystem>();
        var actor = actorManagerSystem.CreateActor("");
        actor.AddComponent<ActorMoveComponent>();
        World.inst.GetComponent<ControlActor>().SetActor(actor .ActorId);
    }
}
