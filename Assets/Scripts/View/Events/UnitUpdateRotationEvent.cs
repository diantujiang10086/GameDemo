[Event]
public class UnitUpdateRotationEvent : AEvent<UnitChangedRotation>
{
    protected override void Run(UnitChangedRotation a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.unitId);
        if (display == null)
            return;

        display.SetRotation(a.newValue);
    }
}
