using UnityEngine;

public class ActorDisplay : MonoBehaviour 
{
    public Actor actor { get; private set; }
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public void Initialization(Actor actor)
    {
        this.actor = actor;
        transform.position = actor.Position;
        transform.rotation = actor.Rotation;
        transform.localScale = actor.LocalScale;
    }

    public void UpdatePosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void UpdateRotation(Quaternion rotation)
    {
        targetRotation = rotation;
    }

    public void UpdateLocalScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    private void Update()
    {
        UpdateRotation();
        UpdateMove();
    }

    private void UpdateMove()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * actor.MoveSpeed);
    }

    private void UpdateRotation()
    {
        transform.rotation = Quaternion.Slerp(actor.Rotation, targetRotation, World.inst.DeltaTime * actor.RotationSpeed);
    }
}
