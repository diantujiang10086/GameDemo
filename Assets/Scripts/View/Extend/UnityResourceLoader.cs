using UnityEngine;

public class UnityResourceLoader : IResourceLoader
{
    public T Load<T>(string path) where T : class
    {
        return Resources.Load(path) as T;
    }

    public string LoadText(string path)
    {
        return Resources.Load<TextAsset>(path)?.text;
    }
}
