using UnityEngine;

public struct GameStartInfo
{

}
public struct CreateActorInfo
{
    public int actorId;
}

public struct UpdateActorPosition
{
    public int actorId;
    public Vector3 value;
}
public struct UpdateActorRotation
{
    public int actorId;
    public Quaternion value;
}
public struct UpdateActorScale
{
    public int actorId;
    public Vector3 value;
}
public struct ActorMoveInfo
{
    public Vector2 value;
}