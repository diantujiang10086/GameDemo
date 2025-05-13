using UnityEngine;

[Event]
public class GameInitializationEvent : AEvent<GameInitialization>
{
    protected override void Run(GameInitialization a)
    {
        Log.log = new UnityLog();
        ResourceManager.resourceLoader = new UnityResourceLoader();
        new GameObject("GameBehaviour").AddComponent<GameBehaviour>();
    }
}
