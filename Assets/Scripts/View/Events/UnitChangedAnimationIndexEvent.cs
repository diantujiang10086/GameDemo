[Event]
public class UnitChangedAnimationIndexEvent : AEvent<UnitAnimationIndex>
{
    protected override void Run(UnitAnimationIndex a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.unitId);
        if (display == null)
            return;

        display.atlasAnimationIndex = a.value;
    }
}
