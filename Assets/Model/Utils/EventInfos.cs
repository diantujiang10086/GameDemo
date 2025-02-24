using UnityEngine;

public struct GameStartInfo
{

}
public struct CreateActorInfo
{
    public int actorId;
}

public struct UpdateAcotrPosition
{
    public int actorId;
    public Vector3 value;
}
public struct UpdateAcotrRotation
{
    public int actorId;
    public Quaternion value;
}
public struct UpdateAcotrScale
{
    public int actorId;
    public Vector3 value;
}