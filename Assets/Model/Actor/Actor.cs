using UnityEngine;

public class Actor : Entity
{
    private int actorDisplayId;
    private int actorId;
    private float moveSpeed;
    private float rotationSpeed;
    private Vector3 position = Vector3.zero;
    private Quaternion rotation = Quaternion.identity;
    private Vector3 localScale = Vector3.one;
    public string model { get; private set; }

    public int ActorId
    {
        get => actorId;
        set 
        {
            actorId = value;
        }
    }

    public float MoveSpeed
    {
        get => moveSpeed;
    }

    public float RotationSpeed
    {
        get => rotationSpeed;
    }

    public Vector3 Position
    {
        get => position;
        set
        {
            position = value;
            var data = default(UpdateActorPosition);
            data.actorId = actorId;
            ActorEventSystem.Publish(data);
        }
    }

    public Quaternion Rotation
    {
        get => rotation;
        set
        {
            rotation = value;
            var data = default(UpdateActorRotation);
            data.actorId = actorId;
            ActorEventSystem.Publish(data);
        }
    }

    public Vector3 LocalScale
    {
        get => localScale;
        set
        {
            localScale = value;
            var data = default(UpdateActorScale);
            data.actorId = actorId;
            ActorEventSystem.Publish(data);
        }
    }

    public int ActorDisplayId
    {
        get => actorDisplayId;
        set => actorDisplayId = value;
    }

    public void Initialize(int actorId, string config)
    {
        this.actorId = actorId;
        //test
        model = "Actor";
        moveSpeed = 10f;
        rotationSpeed = 10f;
    }
}
