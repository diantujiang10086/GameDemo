using UnityEngine;

public class ActorDisplay : MonoBehaviour 
{
    public Actor actor { get; private set; }

    public void Initialization(Actor actor)
    {
        this.actor = actor;
        transform.position = actor.Position;
        transform.rotation = actor.Rotation;
        transform.localScale = actor.LocalScale;
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void UpdateRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void UpdateLocalScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
