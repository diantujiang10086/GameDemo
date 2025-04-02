[Event]
public class UnitScaleChangedEvent : AEvent<UnitScaleChanged>
{
    protected override void Run(UnitScaleChanged a)
    {
        UnitManager.Instance.GetUnit(a.unitId)?.GetComponent<ColliderComponent>()?.ComputeAABB();
    }
}
