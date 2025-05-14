
using Unity.Mathematics;
using UnityEngine;


public class Display 
{
    private bool isMove = false;
    private bool isRotation = false;
    private float3 targetPosition;
    private quaternion targetRotation;
    private Unit unit;
    private DisplayComponent displayComponent;
    private MoveComponent moveComponent;

    public int atlasAnimationIndex;
    public float animationStartTime;
    public int batchId;
    public int batchIndex;
    public float3 position;
    public float3 scale;
    public quaternion rotation;
    public bool isDirty { get; private set; } = false;

    public void SetDisplayComponent(DisplayComponent displayComponent)
    {
        this.displayComponent = displayComponent;
        
        unit = displayComponent.GetParent<Unit>();
        position = unit.position;
        scale = unit.scale;
        rotation = unit.rotation;
        
        atlasAnimationIndex = displayComponent.animationIndex;
        animationStartTime = Time.time;

        moveComponent = unit.GetComponent<MoveComponent>();
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

    public void RotationLook(quaternion roation)
    {
        targetRotation = roation;
        isRotation = true;
        isDirty = true;
    }

    public void SetScale(float3 scale)
    {
        this.scale = scale;
        isDirty = true;
    }

    public void SetRotation(quaternion rotation)
    {
        this.rotation = rotation;
        isDirty = true;
    }

    public void Update()
    {
        if (isMove)
        {
            position = math.lerp(position, targetPosition, Time.fixedTime * moveComponent.MoveSpeed);
            isDirty = true;
            if (position.Equals(targetPosition))
            {
                isMove = false;
            }
        }

        if(isRotation)
        {
            rotation = math.slerp(rotation, targetRotation, Time.fixedTime * moveComponent.RotationSpeed);
            isDirty = true;
            if (rotation.Equals(targetRotation))
            {
                isRotation = false;
            }
        }
    }
}
