[Event]
public class InputMoveEvent : ActorEvent<ActorMoveInfo>
{
    protected override void Run(Actor actor, ActorMoveInfo a)
    {
        actor.GetComponent<ActorMoveComponent>().SetDeltaMove(a.value);
    }
}