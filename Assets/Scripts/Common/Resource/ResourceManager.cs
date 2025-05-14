public class ResourceManager: Singleton<ResourceManager>
{
    public IResourceLoader resourceLoader = new DefaultResourceLoader();
    public T Load<T>(string path) where T : class
    {
        return resourceLoader.Load<T>(path);
    }

    public string LoadText(string path)
    {
        return resourceLoader.LoadText(path);
    }
}
