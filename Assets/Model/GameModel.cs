using UnityEngine;

public class GameModel 
{
    public static void Initialize()
    {
        var systemManager = new SystemManager();
        SystemManager.AddSystem<IdGeneraterSystem>();
        SystemManager.AddSystem<UpdateSystem>();
        SystemManager.AddSystem<ResourceManagerSystem>();
        SystemManager.AddSystem<ActorManagerSystem>();
        var gameEventSystem = SystemManager.AddSystem<GameEventSystem>();
        gameEventSystem.AddAssembly(typeof(GameModel).Assembly);
    }
    public static void Run()
    {
        var scene = Scene.CreateScene();
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