using UnityEngine;

[Event]
public class GameInitializationEvent : AEvent<GameInitialization>
{
    protected override void Run(GameInitialization a)
    {
        new GameObject("GameBehaviour").AddComponent<GameBehaviour>();
        ResourceManager.resourceLoader = new UnityResourceLoader();

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                var actor = ActorManager.Instance.Create(0);
                actor.animationIndex = 3;
                actor.position = new Unity.Mathematics.float3(i, j, 0);
            }
        }

    }
}
