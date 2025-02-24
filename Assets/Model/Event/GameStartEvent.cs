[Event]
public class GameStartEvent : AEvent<GameStartInfo>
{
    protected override void Run(GameStartInfo _)
    {
        Log.Debug("Game Start");
        var actorManagerSystem = SystemManager.GetSystem<ActorManagerSystem>();
        actorManagerSystem.CreateActor("");
    }
}
