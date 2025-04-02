using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>, IAwake
{
    private Dictionary<long, Unit> units = new Dictionary<long, Unit>();

    public void Awake()
    {

    }

    public Unit AddUnit(UnitType unitType, Vector3 initPosition)
    {
        var unit = World.Instance.MainScene.AddChild<Unit,UnitType, Vector3>(unitType, initPosition);
        units[unit.InstanceId] = unit;
        return unit;
    }

    public Unit GetUnit(long id)
    {
        units.TryGetValue(id, out var unit);
        return unit;
    }

    public IEnumerable<Unit> GetUnits()
    {
        foreach (var kv in units)
        {
            yield return kv.Value;
        }
    }

    public void RemoveUnit(long id)
    {
        if(units.TryGetValue(id, out var unit))
        {
            units.Remove(id);
        }
    }
}