using UnityEngine;

public class ActorMoveComponent : Entity, IAwake, IFixedUpdate
{
    private Actor actor;
    private bool isRotation;
    private Vector3 deltaMove;
    public void Awake()
    {
        actor = GetParent<Actor>();
    }

    public void SetDeltaMove(Vector2 value)
    {
        isRotation = value.sqrMagnitude > 0.0001f;
        deltaMove = new Vector3(value.x, 0, value.y);
    }

    public void FixedUpdate()
    {
        actor.Position += deltaMove * (World.inst.DeltaTime * actor.MoveSpeed);

        if (isRotation)
        {
            Quaternion targetRotation = Quaternion.LookRotation(deltaMove);
            actor.Rotation = Quaternion.Slerp(actor.Rotation, targetRotation, World.inst.DeltaTime * actor.RotationSpeed);
        }
    }
}