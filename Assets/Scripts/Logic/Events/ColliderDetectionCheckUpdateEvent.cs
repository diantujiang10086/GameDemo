[Event]
public class ColliderDetectionCheckUpdateEvent : AEvent<ColliderDetectionCheckUpdate>
{
    protected override void Run(ColliderDetectionCheckUpdate a)
    {
        var unit = UnitManager.Instance.GetUnit(a.unitId);
		var collision2DComponent = unit.GetComponent<Collision2DComponent>();
		if(collision2DComponent != null)
		{
			SparseGridCollision2DManager.Instance.UpdateCollisionDetection(collision2DComponent);
		}
    }
}
[Event]
public class ColliderRegisterEvent : AEvent<ColliderRegister>
{
    protected override void Run(ColliderRegister a)
    {
        var unit = UnitManager.Instance.GetUnit(a.unitId);
        var collision2DComponent = unit.GetComponent<Collision2DComponent>();
        if (collision2DComponent != null)
        {
            SparseGridCollision2DManager.Instance.Register(collision2DComponent);
        }
    }
}
