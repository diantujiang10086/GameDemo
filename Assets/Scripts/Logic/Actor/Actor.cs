using System;
using Unity.Mathematics;

public class Actor : IDisposable
{
    private int m_animationIndex;
    private float m_moveSpeed;
    private float m_rotationSpeed;
    private float3 m_position;
    private float3 m_scale = new float3(1, 1, 1);
    private float3 m_rotation;
    public int id { get; private set; }
    public int configId { get; private set; }
    public int animationIndex 
    { 
        get=>m_animationIndex;
        set
        {
            m_animationIndex = value;
            EventSystem.Instance.Publish(new ActorAnimationIndex { actorId = id, value = value });
        }
    }
    public float3 position
    {
        get => m_position;
        set
        {
            EventSystem.Instance.Publish(new ActorChangedPosition { actorId = id, oldValue = m_position, newValue = value });
            m_position = value;
        }
    }

    public float3 scale
    {
        get => m_scale;
        set
        {
            m_scale = value;
            EventSystem.Instance.Publish(new ActorChangedScale { actorId = id, oldValue = m_position, newValue = value });
        }
    }

    public float3 rotation
    {
        get => m_rotation;
        set
        {
            m_rotation = value;
            EventSystem.Instance.Publish(new ActorChangedRotation { actorId = id, oldValue = m_position, newValue = value });
        }
    }

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

    public void Dispose()
    {
        EventSystem.Instance.Publish(new NewActor { actorId = id });
    }

    internal void Initialize(int id, int configId)
    {
        this.id = id;
        this.configId = configId;
    }
}
