[Event]
public class UnitPositionChangedEvent : AEvent<UnitPositionChanged>
{
    protected override void Run(UnitPositionChanged a)
    {
        var colliderComponent = UnitManager.Instance.GetUnit(a.unitId)?.GetComponent<ColliderComponent>();
        if(colliderComponent != null)
        {
            colliderComponent.SetGrid(GridManager.Instance.Move(a.unitId, a.oldValue, a.newValue));
            colliderComponent?.ComputeAABB();
        }
    }
}
