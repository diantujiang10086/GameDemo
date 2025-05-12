public class ResourceManager
{
    public static IResourceLoader resourceLoader;
    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        return resourceLoader.Load<T>(path);
    }
}
