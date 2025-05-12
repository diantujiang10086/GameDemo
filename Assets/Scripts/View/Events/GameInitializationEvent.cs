using UnityEngine;

[Event]
public class GameInitializationEvent : AEvent<GameInitialization>
{
    protected override void Run(GameInitialization a)
    {
        new GameObject("GameBehaviour").AddComponent<GameBehaviour>();
        ResourceManager.resourceLoader = new UnityResourceLoader();


        var actor1 = ActorManager.Instance.Create(0);
        actor1.animationIndex = 3;
        new GameObject().AddComponent<BindActor>().actor = actor1;
        //var actor = ActorManager.Instance.Create(1);
        //actor.position = new Unity.Mathematics.float3(1,1,0);
    }
}
