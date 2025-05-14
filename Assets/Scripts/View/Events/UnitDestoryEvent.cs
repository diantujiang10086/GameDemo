using UnityEngine;

[Event]
public class UnitDestoryEvent : AEvent<UnitDestory>
{
    protected override void Run(UnitDestory a)
    {
        DisplayManager.Instance.RemoveDisplay(a.unitId);
    }
}
