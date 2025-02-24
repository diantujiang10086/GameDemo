using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool
{
    private string modelPath;
    private ViewResourceManagerSystem resourceManagerSystem;
    private ObjectPool<GameObject> pool;
    public GameObjectPool(string path)
    {
        this.modelPath = path;
        resourceManagerSystem = SystemManager.GetSystem<ViewResourceManagerSystem>();
        pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease);
    }

    public GameObject Fetch()
    {
        return pool.Get();
    }

    public void Recycle(GameObject prefab)
    {
        pool.Release(prefab);
    }

    private GameObject OnCreate()
    {
        var prefab = resourceManagerSystem.LoadGameObject(modelPath);
        var go = GameObject.Instantiate(prefab);
        go.SetActive(false);
        return go;
    }

    private void OnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    private void OnRelease(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }
}