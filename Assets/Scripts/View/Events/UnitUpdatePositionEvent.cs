[Event]
public class UnitUpdatePositionEvent : AEvent<UnitChangedPosition>
{
    protected override void Run(UnitChangedPosition a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.unitId);
        if (display == null)
            return;

        display.SetPosition(a.newValue);
    }
}
