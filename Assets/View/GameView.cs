using UnityEngine;

public class GameView
{
    public static void Initialize()
    {
        GameEventSystem.inst.AddAssembly(typeof(GameView).Assembly);
        ActorEventSystem.inst.AddAssembly(typeof(GameView).Assembly);
        World.inst.AddComponent<ActorDisplayManagerSystem>();
        World.inst.AddComponent<ViewResourceManagerSystem>();
        World.inst.AddComponent<GameObjectPoolSystem>();
        World.inst.AddComponent<InputManagerSystem>();

        new GameObject().AddComponent<BehaviourManager>();
        GameEventSystem.Publish(default(GameStartViewInfo));
    }

}