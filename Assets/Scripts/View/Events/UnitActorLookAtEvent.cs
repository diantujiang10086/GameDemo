[Event]
public class UnitActorLookAtEvent : AEvent<UnitLookAt>
{
    protected override void Run(UnitLookAt a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.unitId);
        if (display == null)
            return;

        display.RotationLook(a.value);
    }
}
