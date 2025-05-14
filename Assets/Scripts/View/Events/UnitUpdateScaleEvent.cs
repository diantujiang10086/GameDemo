[Event]
public class UnitUpdateScaleEvent : AEvent<UnitChangedScale>
{
    protected override void Run(UnitChangedScale a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.unitId);
        if (display == null)
            return;

        display.SetScale(a.newValue);
    }
}
