using UnityEngine;

public class UnityResourceLoader : IResourceLoader
{
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }
}
