[Event]
public class ActorChangedAnimationIndexEvent : AEvent<ActorAnimationIndex>
{
    protected override void Run(ActorAnimationIndex a)
    {
        var display = DisplayManager.Instance.GetDisplay(a.actorId);
        if (display == null)
            return;

        display.atlasAnimationIndex = a.value;
    }
}
