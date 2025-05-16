using Unity.Mathematics;

public struct GameInitialization
{

}

public struct GameStart
{ 

}

public struct UnitCreate
{
    public long unitId;
}

public struct UnitDestory
{
    public long unitId;
}

public struct UnitChangedPosition
{
    public long unitId;
    public float3 oldValue;
    public float3 newValue;
}

public struct UnitMovePosition
{
    public long unitId;
    public float3 value;
}

public struct UnitChangedScale
{
    public long unitId;
    public float3 oldValue;
    public float3 newValue;
}
public struct UnitChangedRotation
{
    public long unitId;
    public quaternion oldValue;
    public quaternion newValue;
}

public struct UnitLookAt
{
    public long unitId;
    public quaternion value;
}

public struct UnitAnimationIndex
{
    public long unitId;
    public int value;
}

public struct CollisionEnter
{
    public long unitA;
    public long unitB;
}

public struct CollisionStay
{
    public long unitA;
    public long unitB;
}

public struct CollisionExit
{
    public long unitA;
    public long unitB;
}

public struct ColliderDetectionCheckUpdate
{
    public long unitId;
}
public struct ColliderRegister
{
    public long unitId;
}