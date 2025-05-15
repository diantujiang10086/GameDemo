using Unity.Mathematics;

public class MoveComponent : Entity, IAwake<float, float>, IFixedUpdate
{
    private Unit m_unit;
    private bool isMoveing;
    private bool isRotating;
    private float m_moveSpeed;
    private float m_rotationSpeed;
    private float3 m_targetPosition;
    private float3 m_targetDirection;

    public float MoveSpeed
    {
        get => m_moveSpeed;
        set
        {
            m_moveSpeed = value;
        }
    }

    public float RotationSpeed
    {
        get => m_rotationSpeed;
        set
        {
            m_rotationSpeed = value;
        }
    }

    public float3 TargetPosition
    {
        get => m_targetPosition;
        set
        {
            m_targetPosition = value;
            isMoveing = true;
        }
    }

    public float3 TargetDirection
    {
        get => m_targetDirection;
        set
        {
            m_targetDirection = math.normalize(value); 
            isRotating = true;
        }
    }


    public void Awake()
    {
        m_unit = GetParent<Unit>();
    }

    public void Awake(float moveSpeed, float rotationSpeed)
    {
        m_moveSpeed = moveSpeed;
        m_rotationSpeed = rotationSpeed;
    }

    public void FixedUpdate(float elaspedTime)
    {
        if(isMoveing)
        {
            float3 currentPosition = m_unit.position;
            float3 toTarget = m_targetPosition - currentPosition;
            float distance = math.length(toTarget);

            if (distance > 0.01f)
            {
                float3 direction = math.normalize(toTarget);
                float moveDistance = m_moveSpeed * elaspedTime;

                if (moveDistance >= distance)
                {
                    m_unit.position = m_targetPosition;
                }
                else
                {
                    m_unit.MoveTo(m_unit.position + direction * moveDistance);
                }
            }
            else
            {
                isMoveing = false;
            }
        }
        

        if (isRotating && !m_targetDirection.Equals(float3.zero))
        {
            quaternion currentRotation = m_unit.rotation;

            quaternion targetRotation = quaternion.LookRotationSafe(m_targetDirection, new float3(0, 1, 0));

            float angle = math.degrees(math.acos(math.clamp(math.dot(currentRotation.value, targetRotation.value), -1f, 1f))) * 2f;

            float rotateStep = m_rotationSpeed * elaspedTime;

            if (angle <= rotateStep)
            {
                m_unit.rotation = targetRotation;
                isRotating = false;
            }
            else
            {
                float t = rotateStep / angle;
                quaternion newRotation = math.slerp(currentRotation, targetRotation, t);
                m_unit.RotationTo(newRotation);
            }
        }
    }
}