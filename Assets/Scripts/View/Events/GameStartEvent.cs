[Event]
public class GameStartEvent : AEvent<GameStart>
{
    protected override void Run(GameStart a)
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                var actor = ActorManager.Instance.Create(1);
                actor.animationIndex = 3;
                actor.position = new Unity.Mathematics.float3(i, j, 0);
            }
        }
    }
}