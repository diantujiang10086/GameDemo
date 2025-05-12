[Event]
public class ActorUpdateRotationEvent : AEvent<ActorChangedRotation>
{
    protected override void Run(ActorChangedRotation a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.actorId);
        if (display == null)
            return;

        display.SetRotation(a.newValue);
    }
}
