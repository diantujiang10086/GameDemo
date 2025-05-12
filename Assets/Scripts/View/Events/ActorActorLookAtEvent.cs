[Event]
public class ActorActorLookAtEvent : AEvent<ActorLookAt>
{
    protected override void Run(ActorLookAt a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.actorId);
        if (display == null)
            return;

        display.RotationLook(a.value);
    }
}
