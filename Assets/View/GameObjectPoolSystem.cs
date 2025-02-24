using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolSystem : ISystem
{
    private Dictionary<string, GameObjectPool> dict = new Dictionary<string, GameObjectPool>();
    
    public GameObject Fetch(string path)
    {
        if(!dict.TryGetValue(path, out var pool))
        {
            pool = new GameObjectPool(path);
            dict[path] = pool;
        }
        return pool.Fetch();
    }

    public void Recycle(string path, GameObject gameObject)
    {
        dict[path].Recycle(gameObject);
    }
}
