using UnityEngine;

[Event]
public class GameInitializationEvent : AEvent<GameInitialization>
{
    protected override void Run(GameInitialization a)
    {
        Log.log = new UnityLog();
        World.Instance.AddSigleton<DisplayManager>();
        World.Instance.AddSigleton<DisplayBatchManager>();
        ResourceManager.Instance.resourceLoader = new UnityResourceLoader();
        new GameObject("GameManager").AddComponent<GameManager>();
    }
}
