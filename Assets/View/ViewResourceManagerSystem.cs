using UnityEngine;
using Object = UnityEngine.Object;
public class ViewResourceManagerSystem : ISystem, IAwake
{
    public void Awake()
    {
        
    }

    public T Load<T>(string path) where T : Object
    {
        var obj = Resources.Load<T>(path);
        return obj;
    }

    public GameObject LoadGameObject(string path)
    {
        return Load<GameObject>(path);
    }

}