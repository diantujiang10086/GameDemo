[Event]
public class ActorMoveToPositionEvent : AEvent<ActorMovePosition>
{
    protected override void Run(ActorMovePosition a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.actorId);
        if (display == null)
            return;

        display.Move(a.value);
    }
}
