using UnityEngine;

[Event]
public class EntityCollideEvent : AEvent<ColliderInfo>
{
    protected override void Run(ColliderInfo a)
    {
        Log.Debug($"{a.entityA} {a.entityB}");
    }
}
