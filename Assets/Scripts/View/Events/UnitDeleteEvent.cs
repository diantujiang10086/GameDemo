using UnityEngine;

[Event]
public class UnitDeleteEvent : AEvent<UnitDelete>
{
    protected override void Run(UnitDelete a)
    {
        DisplayManager.Instance.RemoveDisplay(a.unitId);
    }
}
