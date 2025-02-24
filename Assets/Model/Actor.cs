using UnityEngine;

public class Actor : Entity
{
    private int actorDisplayId;
    private int actorId;
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

    public Vector3 Position
    {
        get => position;
        set
        {
            position = value;
            var data = default(UpdateAcotrPosition);
            data.actorId = actorId;
            GameEventSystem.Publish(data);
        }
    }

    public Quaternion Rotation
    {
        get => rotation;
        set
        {
            rotation = value;
            var data = default(UpdateAcotrRotation);
            data.actorId = actorId;
            GameEventSystem.Publish(data);
        }
    }

    public Vector3 LocalScale
    {
        get => localScale;
        set
        {
            localScale = value;
            var data = default(UpdateAcotrScale);
            data.actorId = actorId;
            GameEventSystem.Publish(data);
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
        model = "Cube";
    }
}
