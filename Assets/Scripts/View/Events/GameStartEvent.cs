using Unity.Mathematics;

[Event]
public class GameStartEvent : AEvent<GameStart>
{
    protected override void Run(GameStart a)
    {
        var unit = UnitManager.Instance.CreateActor(1, new float3(0, -5, 0));
        var unit2 = UnitManager.Instance.CreateActor(3,new float3(0, 5, 0));

    }
}