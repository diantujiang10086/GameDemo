[Event]
public class UnitYawChangedEvent : AEvent<UnitYawChanged>
{
    protected override void Run(UnitYawChanged a)
    {
        UnitManager.Instance.GetUnit(a.unitId)?.GetComponent<ColliderComponent>()?.ComputeAABB();
    }
}