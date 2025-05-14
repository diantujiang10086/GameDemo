[Event]
public class GameStartEvent : AEvent<GameStart>
{
    protected override void Run(GameStart a)
    {
        var unit = UnitManager.Instance.Create(1, new Unity.Mathematics.float3(1, 1, 1));
        var unit2 = UnitManager.Instance.Create(3,new Unity.Mathematics.float3(2, 0, 0));

    }
}