[Event]
public class ActorUpdatePositionEvent : AEvent<ActorChangedPosition>
{
    protected override void Run(ActorChangedPosition a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.actorId);
        if (display == null)
            return;

        display.SetPosition(a.newValue);
    }
}
