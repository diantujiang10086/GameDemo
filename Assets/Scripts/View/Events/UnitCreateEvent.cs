using UnityEngine;

[Event]
public class UnitCreateEvent : AEvent<UnitCreate>
{
    protected override void Run(UnitCreate a)
    {
        var unit = UnitManager.Instance.GetUnit(a.unitId);
        var displayComponent = unit.GetComponent<DisplayComponent>();
        if(displayComponent != null )
        {
            DisplayManager.Instance.CreateDisplay(displayComponent);
        }
    }
}
