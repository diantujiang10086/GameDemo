using UnityEngine;

[Event]
public class DeleteActorEvent : AEvent<DeleteActor>
{
    protected override void Run(DeleteActor a)
    {
        DisplayManager.Instance.RemoveDisplay(a.actorId);
    }
}
