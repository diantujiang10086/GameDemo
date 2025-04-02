using UnityEngine;

public class DisplayComponent : Entity, IAwake<string>
{
    private Unit unit;

    public void Awake(string path)
    {
        unit = GetParent<Unit>();
        SetModel(path);
    }

    public void SetModel(string path)
    {
        var prefab = Resources.Load<GameObject>(path);
        var gameObjct = GameObject.Instantiate(prefab);
        gameObjct.transform.SetParent(unit.Transform);
        gameObjct.transform.localPosition = Vector3.zero;
    }

    public void Add(string path)
    {
        var prefab = Resources.Load<GameObject>(path);
        var gameObjct = GameObject.Instantiate(prefab);

        gameObjct.transform.SetParent(unit.Transform);
    }
}