using Unity.Mathematics;
using UnityEngine;

public struct GameStart
{

}

public struct UnitMove
{
    public long unitId;
    public Vector3 position;
}

public struct UnitPositionChanged
{
    public long unitId;
    public Vector3 oldValue;
    public Vector3 newValue;
}

public struct UnitScaleChanged
{
    public long unitId;
    public float oldValue;
    public float newValue;
}
public struct UnitYawChanged
{
    public long unitId;
    public float oldValue;
    public float newValue;
}
