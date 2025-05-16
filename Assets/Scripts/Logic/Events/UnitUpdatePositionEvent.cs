[Event]
public class UnitUpdatePositionEvent : AEvent<UnitChangedPosition>
{
    protected override void Run(UnitChangedPosition a)
    {
        var unit = UnitManager.Instance.GetUnit(a.unitId);
        var collisionComponent = unit.GetComponent<Collision2DComponent>();
        if (collisionComponent != null)
        {
            SparseGridCollision2DManager.Instance.UpdateCollisionGrid(collisionComponent);
        }
    }
}
