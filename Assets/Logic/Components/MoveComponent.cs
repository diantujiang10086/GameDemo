using Unity.Mathematics;
using UnityEngine;

public class MoveComponent : Entity, IAwake<float>, IFixedUpdate
{
    private Unit unit;
    private bool isDirection;
    private bool isMoveing;
    public float moveSpeed;
    private float3 targetPosition;

    public bool IsMoveing => isMoveing;
    public float3 CurPosition => unit.Position;
    public float3 TargetPosition => targetPosition;

    public void Awake(float speed)
    {
        unit = GetParent<Unit>();
        moveSpeed = speed;
    }

    public void SetTarget(Vector3 value)
    {
        isDirection = false;
        targetPosition = value;
        isMoveing = true;
    }

    public void SetDirection(Vector3 direction)
    {
        isDirection = true;
        targetPosition = direction;
        isMoveing = true;
    }

    public void FixedUpdate()
    {
        if (!isMoveing)
            return;

        if(!isDirection)
        {
            unit.Position = Vector3.MoveTowards(unit.Position, targetPosition, moveSpeed * Time.deltaTime);
            isMoveing = Vector3.Distance(unit.Position, targetPosition) > 0.1f;
        }
        else
        {
            unit.Position += targetPosition * (moveSpeed * Time.deltaTime);
        }
    }
}
