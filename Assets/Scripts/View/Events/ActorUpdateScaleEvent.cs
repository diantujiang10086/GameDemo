[Event]
public class ActorUpdateScaleEvent : AEvent<ActorChangedScale>
{
    protected override void Run(ActorChangedScale a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.actorId);
        if (display == null)
            return;

        display.SetScale(a.newValue);
    }
}
