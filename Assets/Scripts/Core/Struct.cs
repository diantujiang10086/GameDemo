using Unity.Mathematics;

public struct GameInitialization
{

}

public struct NewActor
{
    public int actorId;
}

public struct DeleteActor
{
    public int actorId;
}

public struct ActorChangedPosition
{
    public int actorId;
    public float3 oldValue;
    public float3 newValue;
}

public struct ActorMovePosition
{
    public int actorId;
    public float3 value;
}

public struct ActorChangedScale
{
    public int actorId;
    public float3 oldValue;
    public float3 newValue;
}
public struct ActorChangedRotation
{
    public int actorId;
    public float3 oldValue;
    public float3 newValue;
}

public struct ActorLookAt
{
    public int actorId;
    public float3 value;
}

public struct ActorAnimationIndex
{
    public int actorId;
    public int value;
}