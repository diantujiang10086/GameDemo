using UnityEngine;

public class GameView
{
    public static void Initialize()
    {
        var gameEventSystem = SystemManager.GetSystem<GameEventSystem>();
        gameEventSystem.AddAssembly(typeof(GameView).Assembly);
        SystemManager.AddSystem<ActorDisplayManagerSystem>();
        SystemManager.AddSystem<ViewResourceManagerSystem>();
        SystemManager.AddSystem<GameObjectPoolSystem>();
        new GameObject().AddComponent<BehaviourManager>();

        GameEventSystem.Publish(default(GameStartViewInfo));
    }

}