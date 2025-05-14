[Event]
public class UnitMoveToPositionEvent : AEvent<UnitMovePosition>
{
    protected override void Run(UnitMovePosition a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.unitId);
        if (display == null)
            return;

        display.Move(a.value);
    }
}
