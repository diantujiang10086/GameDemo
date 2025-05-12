
using Unity.Mathematics;
using UnityEngine;


public class Display 
{
    private bool isMove = false;
    private bool isRotation = false;
    private float3 targetPosition;
    private quaternion targetRotation;
    private Actor actor;

    public int atlasAnimationIndex;
    public float animationStartTime;
    public int batchId;
    public int batchIndex;
    public float3 position;
    public float3 scale;
    public quaternion rotation;
    public bool isDirty { get; private set; } = false;

    public void SetActor(Actor actor)
    {
        animationStartTime = Time.time;
        position = actor.position;
        scale = actor.scale;
        rotation = quaternion.Euler(actor.rotation);
        isDirty = true;
    }

    public void ResetDirty()
    {
        isDirty = false;
    }

    public void SetPosition(float3 position)
    {
        this.position = position;
        isDirty = true;
    }

    public void Move(float3 targetPosition)
    {
        this.targetPosition = targetPosition;
        isMove = true;
        isDirty = true;
    }

    public void RotationLook(float3 roation)
    {
        targetRotation = quaternion.Euler(roation);
        isRotation = true;
        isDirty = true;
    }

    public void SetScale(float3 scale)
    {
        this.scale = scale;
        isDirty = true;
    }

    public void SetRotation(float3 rotation)
    {
        this.rotation = quaternion.Euler(rotation);
        isDirty = true;
    }

    public void FixedUpdate()
    {
        if (isMove)
        {
            position = math.lerp(position, targetPosition, Time.fixedTime * actor.MoveSpeed);
            isDirty = true;
            if (position.Equals(targetPosition))
            {
                isMove = false;
            }
        }

        if(isRotation)
        {
            rotation = math.slerp(rotation, targetRotation, Time.fixedTime * actor.RotationSpeed);
            isDirty = true;
            if (rotation.Equals(targetRotation))
            {
                isRotation = false;
            }
        }
    }
}
