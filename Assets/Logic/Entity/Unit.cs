using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public enum UnitType
{
    Player,
    PlayerBullet,
    Enemy,
    EnemyBullet
}


public class Unit : Entity, IAwake<UnitType, Vector3>
{
    private string name;
    private float3 position;
    private float scale;
    private float yaw;
    private UnitType unitType;
    private Transform transform;

    public UnitType UnitType => unitType;
    public Transform Transform => transform;

    public float3 Position
    {
        get => position;
        set
        {
            if (math.all(position == value))
                return;

            var oldPosition = position;
            position = value;
            transform.position = position;
            EventSystem.Instance.Publish(new UnitPositionChanged { unitId = InstanceId, oldValue = oldPosition, newValue = value });
        }
    }

    public float LocalScale
    {
        get => scale;
        set
        {
            if (scale == value)
                return;
            var oldValue = scale;
            scale = value;
            transform.localScale = new Vector3(scale, scale, scale);
            EventSystem.Instance.Publish(new UnitScaleChanged { unitId = InstanceId, oldValue = oldValue, newValue = value });
        }
    }

    public float Yaw
    {
        get => yaw;
        set
        {
            if(yaw == value) 
                return;
            var oldValue = yaw;
            yaw = value;
            transform.rotation = Quaternion.Euler(0, yaw, 0);
            EventSystem.Instance.Publish(new UnitYawChanged { unitId = InstanceId, oldValue = oldValue, newValue = value });
        }
    }

    public Quaternion Rotation
    {
        get => transform.rotation;
        set
        {
            transform.rotation = value;
        }
    }

    public void Awake(UnitType type, Vector3 position)
    {
        this.unitType = type;
        transform = new GameObject().transform;
        Position = position;
        name = $"[U]{InstanceId}";
        transform.name = name;
    }

    public override string ToString()
    {
        return name;
    }

    public override void Dispose()
    {
        if (!IsDisposed)
        {
            if (transform != null)
            {
                GameObject.Destroy(transform.gameObject);
            }
            UnitManager.Instance.RemoveUnit(this.InstanceId);
        }

        base.Dispose();
    }
}
