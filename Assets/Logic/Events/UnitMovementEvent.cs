[Event]
public class UnitMovementEvent : AEvent<UnitMove>
{
    protected override void Run(UnitMove a)
    {
        var unit = World.Instance.MainScene.GetChild<Unit>(a.unitId);
        unit.GetComponent<MoveComponent>().SetTarget(a.position);
    }
}
