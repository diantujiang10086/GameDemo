[Event]
public class CollisionEnterEvent : AEvent<CollisionEnter>
{
    protected override void Run(CollisionEnter a)
    {
		var unitA = UnitManager.Instance.GetUnit(a.unitA);
		var unitB = UnitManager.Instance.GetUnit(a.unitB);

		if (unitA.GetComponent<Collision2DComponent>().IsCollisionDestory)
		{
			unitA.Dispose();
		}

		if (unitB.GetComponent<Collision2DComponent>().IsCollisionDestory)
		{
			unitB.Dispose();
		}
	}
}
