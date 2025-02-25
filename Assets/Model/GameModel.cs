using UnityEngine;

public class GameModel 
{
    public static void Initialize()
    {
        IdGenerater.Initialize();
        UpdateSystem.Initialize();
        World.inst.AddComponent<ResourceManagerSystem>();
        World.inst.AddComponent<ActorManagerSystem>();
        World.inst.AddComponent<ControlActor>();
        GameEventSystem.inst.AddAssembly(typeof(GameModel).Assembly);
        ActorEventSystem.inst.AddAssembly(typeof(GameModel).Assembly);
    }
    public static void Run()
    {
        GameEventSystem.Publish(default(GameStartInfo));
    }
    public static void FixedUpdate()
    {
        
    }

    public static void Update()
    {
        
    }

    public static void LateUpdate()
    {
        
    }
}